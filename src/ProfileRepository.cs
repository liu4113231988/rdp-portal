using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Dapper;
using Microsoft.Data.Sqlite;

namespace RDP_Portal
{
    public class ProfileRepository
    {
        private readonly DatabaseContext _context;

        public ProfileRepository(DatabaseContext context)
        {
            _context = context;
        }

        public List<Profile> GetAllProfiles()
        {
            using var connection = _context.GetConnection();
            var profiles = connection.Query<Profile>("SELECT * FROM Profiles ORDER BY Id").ToList();
            return profiles;
        }

        public int InsertProfile(Profile profile)
        {
            using var connection = _context.GetConnection();
            var sql = @"
                INSERT INTO Profiles (
                    Name, Filename, Computer, Username, EncryptedPassword, Domain, ProfileGroup,
                    DesktopWidth, DesktopHeight, ScreenMode, UseMultiMon, ColorDepth, AudioMode,
                    RedirectPrinters, RedirectClipboard, RedirectDrives, RedirectPorts,
                    RedirectSmartCards, PromptForCredentials, AuthenticationLevel, EnableCredSSPSupport
                ) VALUES (
                    @Name, @Filename, @Computer, @Username, @EncryptedPassword, @Domain, @Group,
                    @DesktopWidth, @DesktopHeight, @ScreenMode, @UseMultiMon, @ColorDepth, @AudioMode,
                    @RedirectPrinters, @RedirectClipboard, @RedirectDrives, @RedirectPorts,
                    @RedirectSmartCards, @PromptForCredentials, @AuthenticationLevel, @EnableCredSSPSupport
                );
                SELECT last_insert_rowid();";
            return connection.ExecuteScalar<int>(sql, profile);
        }

        public void UpdateProfile(Profile profile)
        {
            using var connection = _context.GetConnection();
            var sql = @"
                UPDATE Profiles SET
                    Name = @Name,
                    Filename = @Filename,
                    Computer = @Computer,
                    Username = @Username,
                    EncryptedPassword = @EncryptedPassword,
                    Domain = @Domain,
                    ProfileGroup = @Group,
                    DesktopWidth = @DesktopWidth,
                    DesktopHeight = @DesktopHeight,
                    ScreenMode = @ScreenMode,
                    UseMultiMon = @UseMultiMon,
                    ColorDepth = @ColorDepth,
                    AudioMode = @AudioMode,
                    RedirectPrinters = @RedirectPrinters,
                    RedirectClipboard = @RedirectClipboard,
                    RedirectDrives = @RedirectDrives,
                    RedirectPorts = @RedirectPorts,
                    RedirectSmartCards = @RedirectSmartCards,
                    PromptForCredentials = @PromptForCredentials,
                    AuthenticationLevel = @AuthenticationLevel,
                    EnableCredSSPSupport = @EnableCredSSPSupport,
                    UpdatedAt = datetime('now')
                WHERE Id = @Id";
            connection.Execute(sql, profile);
        }

        public void DeleteProfile(int id)
        {
            using var connection = _context.GetConnection();
            connection.Execute("DELETE FROM Profiles WHERE Id = @Id", new { Id = id });
        }

        public List<Group> GetAllGroups()
        {
            using var connection = _context.GetConnection();
            return connection.Query<Group>("SELECT * FROM Groups ORDER BY GroupName").ToList();
        }

        public int InsertGroup(Group group)
        {
            using var connection = _context.GetConnection();
            var sql = "INSERT INTO Groups (GroupName) VALUES (@GroupName); SELECT last_insert_rowid();";
            return connection.ExecuteScalar<int>(sql, group);
        }

        public void UpdateGroup(Group group)
        {
            using var connection = _context.GetConnection();
            connection.Execute("UPDATE Groups SET GroupName = @GroupName WHERE Id = @Id", group);
        }

        public void DeleteGroup(int id)
        {
            using var connection = _context.GetConnection();
            connection.Execute("DELETE FROM Groups WHERE Id = @Id", new { Id = id });
        }

        public void DeleteGroupByName(string groupName)
        {
            using var connection = _context.GetConnection();
            connection.Execute("DELETE FROM Groups WHERE GroupName = @GroupName", new { GroupName = groupName });
        }

        public string? GetSetting(string key)
        {
            using var connection = _context.GetConnection();
            return connection.ExecuteScalar<string?>("SELECT Value FROM Settings WHERE Key = @Key", new { Key = key });
        }

        public void SetSetting(string key, string value)
        {
            using var connection = _context.GetConnection();
            var sql = "INSERT OR REPLACE INTO Settings (Key, Value) VALUES (@Key, @Value)";
            connection.Execute(sql, new { Key = key, Value = value });
        }

        public bool GetSettingBool(string key, bool defaultValue = false)
        {
            var value = GetSetting(key);
            if (value == null) return defaultValue;
            return value.Equals("true", StringComparison.OrdinalIgnoreCase) || value == "1";
        }

        public void SetSettingBool(string key, bool value)
        {
            SetSetting(key, value ? "true" : "false");
        }
    }
}
