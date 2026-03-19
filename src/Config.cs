using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RDP_Portal
{
    public class Config : IDisposable
    {
        private static Config? _instance;
        private readonly DatabaseContext _db;
        private readonly ProfileRepository _profileRepo;

        public static string rdpDir = "rdp-files";

        private Config()
        {
            _db = new DatabaseContext();
            _profileRepo = new ProfileRepository(_db);
            Profiles = new BindingList<Profile>();
            Groups = new List<Group>();
        }

        public static Config GetConfig()
        {
            if (_instance == null)
            {
                _instance = new Config();
                _instance.Load();
            }
            return _instance;
        }

        public static async Task<Config> GetConfigAsync()
        {
            if (_instance == null)
            {
                _instance = new Config();
                await Task.Run(() => _instance.Load());
            }
            return _instance;
        }

        public BindingList<Profile> Profiles { get; private set; }
        public List<Group> Groups { get; set; }
        public bool KeepOpening { get; set; } = true;

        public void ImportProfiles(List<Profile> profiles)
        {
            try
            {
                foreach (var profile in profiles)
                {
                    profile.Id = 0;
                    profile.Id = _profileRepo.InsertProfile(profile);
                    Profiles.Add(profile);
                }
                Logger.Info($"Imported {profiles.Count} profiles");
            }
            catch (Exception ex)
            {
                Logger.Error("Failed to import profiles", ex);
                throw;
            }
        }

        private void Load()
        {
            try
            {
                if (!Directory.Exists(rdpDir))
                {
                    Directory.CreateDirectory(rdpDir);
                    Logger.Info($"Created RDP directory: {rdpDir}");
                }

                var profiles = _profileRepo.GetAllProfiles();
                Profiles = new BindingList<Profile>(profiles);

                var groups = _profileRepo.GetAllGroups();
                Groups = groups;

                LoadSettings();

                Logger.Info($"Loaded {Profiles.Count} profiles and {Groups.Count} groups");
            }
            catch (Exception ex)
            {
                Logger.Error("Failed to load configuration", ex);
                throw;
            }
        }

        private void LoadSettings()
        {
            try
            {
                var setting = _profileRepo.GetSetting("KeepOpening");
                if (setting != null && bool.TryParse(setting, out bool keepOpening))
                {
                    KeepOpening = keepOpening;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Failed to load settings", ex);
            }
        }

        public void Save()
        {
            try
            {
                foreach (var profile in Profiles)
                {
                    if (profile.Id == 0)
                    {
                        profile.Id = _profileRepo.InsertProfile(profile);
                        Logger.Debug($"Inserted new profile: {profile.Name}");
                    }
                    else
                    {
                        _profileRepo.UpdateProfile(profile);
                        Logger.Debug($"Updated profile: {profile.Name}");
                    }
                }

                _profileRepo.SetSetting("KeepOpening", KeepOpening.ToString());
                Logger.Info("Configuration saved successfully");
            }
            catch (Exception ex)
            {
                Logger.Error("Failed to save configuration", ex);
                throw;
            }
        }

        public void SaveGroups()
        {
            try
            {
                var existingGroups = _profileRepo.GetAllGroups();
                var existingNames = existingGroups.Select(g => g.GroupName).ToList();

                foreach (var group in Groups)
                {
                    if (!existingNames.Contains(group.GroupName))
                    {
                        _profileRepo.InsertGroup(group);
                        Logger.Info($"Created new group: {group.GroupName}");
                    }
                }

                foreach (var existing in existingGroups)
                {
                    if (!Groups.Any(g => g.GroupName == existing.GroupName))
                    {
                        _profileRepo.DeleteGroup(existing.Id);
                        Logger.Info($"Deleted group: {existing.GroupName}");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Failed to save groups", ex);
                throw;
            }
        }

        public void DeleteProfile(Profile profile)
        {
            try
            {
                if (profile.Id > 0)
                {
                    _profileRepo.DeleteProfile(profile.Id);
                }
                Profiles.Remove(profile);
                Logger.Info($"Deleted profile: {profile.Name}");
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to delete profile: {profile.Name}", ex);
                throw;
            }
        }

        public void Dispose()
        {
            try
            {
                _db?.Dispose();
            }
            catch (Exception ex)
            {
                Logger.Error("Error disposing config", ex);
            }
        }
    }
}
