using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;

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

        public BindingList<Profile> Profiles { get; private set; }
        public List<Group> Groups { get; set; }
        public bool KeepOpening { get; set; } = true;

        public void ImportProfiles(List<Profile> profiles)
        {
            foreach (var profile in profiles)
            {
                profile.Id = 0;
                profile.Id = _profileRepo.InsertProfile(profile);
                Profiles.Add(profile);
            }
        }

        private void Load()
        {
            if (!Directory.Exists(rdpDir))
            {
                Directory.CreateDirectory(rdpDir);
            }

            var profiles = _profileRepo.GetAllProfiles();
            Profiles = new BindingList<Profile>(profiles);

            var groups = _profileRepo.GetAllGroups();
            Groups = groups;

            LoadSettings();
        }

        private void LoadSettings()
        {
            var setting = _profileRepo.GetSetting("KeepOpening");
            if (setting != null && bool.TryParse(setting, out bool keepOpening))
            {
                KeepOpening = keepOpening;
            }
        }

        public void Save()
        {
            foreach (var profile in Profiles)
            {
                if (profile.Id == 0)
                {
                    profile.Id = _profileRepo.InsertProfile(profile);
                }
                else
                {
                    _profileRepo.UpdateProfile(profile);
                }
            }

            _profileRepo.SetSetting("KeepOpening", KeepOpening.ToString());
        }

        public void SaveGroups()
        {
            var existingGroups = _profileRepo.GetAllGroups();
            var existingNames = existingGroups.Select(g => g.GroupName).ToList();

            foreach (var group in Groups)
            {
                if (!existingNames.Contains(group.GroupName))
                {
                    _profileRepo.InsertGroup(group);
                }
            }

            foreach (var existing in existingGroups)
            {
                if (!Groups.Any(g => g.GroupName == existing.GroupName))
                {
                    _profileRepo.DeleteGroup(existing.Id);
                }
            }
        }

        public void DeleteProfile(Profile profile)
        {
            if (profile.Id > 0)
            {
                _profileRepo.DeleteProfile(profile.Id);
            }
            Profiles.Remove(profile);
        }

        public void Dispose()
        {
            _db?.Dispose();
        }
    }
}
