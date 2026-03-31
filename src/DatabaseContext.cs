using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Data.Sqlite;

namespace RDP_Portal
{
    public class DatabaseContext : IDisposable
    {
        private static readonly string DbFileName = "rdp-portal.db";
        private static readonly string EncryptionKey = "RDP-Portal-2026-Secure!@#";
        private readonly SqliteConnection _connection;
        private bool _disposed;

        public static string DatabasePath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DbFileName);

        public DatabaseContext()
        {
            try
            {
                var dbExists = File.Exists(DatabasePath);

                if (!dbExists)
                {
                    Logger.Info("Database file not found, creating new database");

                    var directory = Path.GetDirectoryName(DatabasePath);
                    if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }
                    File.WriteAllBytes(DatabasePath, Array.Empty<byte>());
                }

                var connectionString = new SqliteConnectionStringBuilder
                {
                    DataSource = DatabasePath,
                    Mode = SqliteOpenMode.ReadWriteCreate
                }.ToString();

                _connection = new SqliteConnection(connectionString);
                _connection.Open();

                SetEncryptionKey();
                CreateTables();

                Logger.Info("Database initialized successfully");
            }
            catch (Exception ex)
            {
                Logger.Error("Failed to initialize database", ex);
                throw;
            }
        }

        private void SetEncryptionKey()
        {
            try
            {
                using var command = _connection.CreateCommand();
                command.CommandText = $"PRAGMA key = '{EncryptionKey}';";
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.Error("Failed to set encryption key", ex);
                throw;
            }
        }

        private void CreateTables()
        {
            try
            {
                using var command = _connection.CreateCommand();
                command.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Profiles (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT NOT NULL DEFAULT '',
                        Filename TEXT NOT NULL DEFAULT '',
                        Computer TEXT NOT NULL DEFAULT '',
                        Username TEXT NOT NULL DEFAULT '',
                        EncryptedPassword TEXT NOT NULL DEFAULT '',
                        Domain TEXT NOT NULL DEFAULT '',
                        GroupName TEXT NOT NULL DEFAULT '',
                        DesktopWidth INTEGER NOT NULL DEFAULT 1280,
                        DesktopHeight INTEGER NOT NULL DEFAULT 720,
                        ScreenMode INTEGER NOT NULL DEFAULT 1,
                        UseMultiMon INTEGER NOT NULL DEFAULT 0,
                        ColorDepth INTEGER NOT NULL DEFAULT 24,
                        AudioMode INTEGER NOT NULL DEFAULT 0,
                        RedirectPrinters INTEGER NOT NULL DEFAULT 1,
                        RedirectClipboard INTEGER NOT NULL DEFAULT 1,
                        RedirectDrives INTEGER NOT NULL DEFAULT 0,
                        RedirectPorts INTEGER NOT NULL DEFAULT 0,
                        RedirectSmartCards INTEGER NOT NULL DEFAULT 0,
                        PromptForCredentials INTEGER NOT NULL DEFAULT 0,
                        AuthenticationLevel INTEGER NOT NULL DEFAULT 0,
                        EnableCredSSPSupport INTEGER NOT NULL DEFAULT 1,
                        CreatedAt TEXT NOT NULL DEFAULT (datetime('now')),
                        UpdatedAt TEXT NOT NULL DEFAULT (datetime('now'))
                    );

                    CREATE TABLE IF NOT EXISTS Groups (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        GroupName TEXT NOT NULL UNIQUE
                    );

                    CREATE TABLE IF NOT EXISTS Settings (
                        Key TEXT PRIMARY KEY,
                        Value TEXT NOT NULL
                    );

                    CREATE TABLE IF NOT EXISTS ConnectionHistory (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        ProfileId INTEGER NOT NULL,
                        ProfileName TEXT NOT NULL,
                        Computer TEXT NOT NULL,
                        ConnectedAt TEXT NOT NULL DEFAULT (datetime('now'))
                    );
                ";
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.Error("Failed to create database tables", ex);
                throw;
            }
        }

        public SqliteConnection GetConnection()
        {
            return _connection;
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                try
                {
                    _connection?.Close();
                    _connection?.Dispose();
                    _disposed = true;
                }
                catch (Exception ex)
                {
                    Logger.Error("Error disposing database connection", ex);
                }
            }
        }
    }
}
