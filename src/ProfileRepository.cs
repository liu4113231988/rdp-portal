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
            try
            {
                using var connection = _context.GetConnection();
                var profiles = connection.Query<Profile>("SELECT * FROM Profiles ORDER BY Id").ToList();
                return profiles;
            }
            catch (Exception ex)
            {
                Logger.Error("Failed to get all profiles", ex);
                throw;
            }
        }

        public int InsertProfile(Profile profile)
        {
            try
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
                var id = connection.ExecuteScalar<int>(sql, profile);
                Logger.Debug($"Inserted profile with ID {id}: {profile.Name}");
                return id;
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to insert profile: {profile.Name}", ex);
                throw;
            }
        }

        public void UpdateProfile(Profile profile)
        {
            try
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
                Logger.Debug($"Updated profile: {profile.Name}");
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to update profile: {profile.Name}", ex);
                throw;
            }
        }

        public void DeleteProfile(int id)
        {
            try
            {
                using var connection = _context.GetConnection();
                connection.Execute("DELETE FROM Profiles WHERE Id = @Id", new { Id = id });
                Logger.Info($"Deleted profile with ID: {id}");
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to delete profile with ID: {id}", ex);
                throw;
            }
        }

        public List<Group> GetAllGroups()
        {
            try
            {
                using var connection = _context.GetConnection();
                return connection.Query<Group>("SELECT * FROM Groups ORDER BY GroupName").ToList();
            }
            catch (Exception ex)
            {
                Logger.Error("Failed to get all groups", ex);
                throw;
            }
        }

        public int InsertGroup(Group group)
        {
            try
            {
                using var connection = _context.GetConnection();
                var sql = "INSERT INTO Groups (GroupName) VALUES (@GroupName); SELECT last_insert_rowid();";
                var id = connection.ExecuteScalar<int>(sql, group);
                Logger.Info($"Created group: {group.GroupName}");
                return id;
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to insert group: {group.GroupName}", ex);
                throw;
            }
        }

        public void UpdateGroup(Group group)
        {
            try
            {
                using var connection = _context.GetConnection();
                connection.Execute("UPDATE Groups SET GroupName = @GroupName WHERE Id = @Id", group);
                Logger.Info($"Updated group: {group.GroupName}");
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to update group: {group.GroupName}", ex);
                throw;
            }
        }

        public void DeleteGroup(int id)
        {
            try
            {
                using var connection = _context.GetConnection();
                connection.Execute("DELETE FROM Groups WHERE Id = @Id", new { Id = id });
                Logger.Info($"Deleted group with ID: {id}");
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to delete group with ID: {id}", ex);
                throw;
            }
        }

        public void DeleteGroupByName(string groupName)
        {
            try
            {
                using var connection = _context.GetConnection();
                connection.Execute("DELETE FROM Groups WHERE GroupName = @GroupName", new { GroupName = groupName });
                Logger.Info($"Deleted group: {groupName}");
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to delete group: {groupName}", ex);
                throw;
            }
        }

        public string? GetSetting(string key)
        {
            try
            {
                using var connection = _context.GetConnection();
                return connection.ExecuteScalar<string?>("SELECT Value FROM Settings WHERE Key = @Key", new { Key = key });
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to get setting: {key}", ex);
                return null;
            }
        }

        public void SetSetting(string key, string value)
        {
            try
            {
                using var connection = _context.GetConnection();
                var sql = "INSERT OR REPLACE INTO Settings (Key, Value) VALUES (@Key, @Value)";
                connection.Execute(sql, new { Key = key, Value = value });
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to set setting: {key}", ex);
                throw;
            }
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
