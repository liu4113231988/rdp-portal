using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace RDP_Portal
{
    public class Profile
    {
        public int Id { get; set; }

        private string _name = "";

        public string Name
        {
            get
            {
                if (_name == "")
                {
                    return "<New Profile>";
                }
                return _name;
            }
            set => _name = value;
        }

        public string Filename { get; set; } = "";
        public string Computer { get; set; }
        public string Username { get; set; }

        /**
         * Encrypted Password used by mstsc.exe
         */
        public string GetRDPEncryptedPassword()
        {
            var mstscpw = new Mstscpw();
            return mstscpw.encryptpw(this.Password);
        }

        /**
         * Encrypted Password in config.json
         */
        public string EncryptedPassword { get; set; } = "";

        [JsonIgnore]
        public string Password
        {
            get
            {
                if (EncryptedPassword == "")
                {
                    return EncryptedPassword;
                }
                return EncryptedPassword.Decrypt();
            }
            set => EncryptedPassword = value.Encrypt();
        }

        public string Domain { get; set; }

        // Optional grouping and tagging for profiles
        public string Group { get; set; } = "";

        // Advanced settings
        public int DesktopWidth { get; set; } = 1280;
        public int DesktopHeight { get; set; } = 720;
        public int ScreenMode { get; set; } = 1;
        public int UseMultiMon { get; set; } = 0;
        public int ColorDepth { get; set; } = 24;
        public int AudioMode { get; set; } = 0;
        public int RedirectPrinters { get; set; } = 1;
        public int RedirectClipboard { get; set; } = 1;
        public int RedirectDrives { get; set; } = 0;
        public int RedirectPorts { get; set; } = 0;
        public int RedirectSmartCards { get; set; } = 0;
        public int PromptForCredentials { get; set; } = 0;
        public int AuthenticationLevel { get; set; } = 0;
        public int EnableCredSSPSupport { get; set; } = 1;

        // Tags removed per UX decision

        public void PrepareRdpFile()
        {
            var justCreated = false;

            try
            {
                if (String.IsNullOrWhiteSpace(Filename) || Filename == "")
                {
                    var name = Name;
                    if (String.IsNullOrWhiteSpace(name))
                    {
                        name = "profile-" + Guid.NewGuid().ToString().Substring(0, 8);
                    }

                    name = name.Trim();
                    foreach (var c in Path.GetInvalidFileNameChars())
                    {
                        name = name.Replace(c, '_');
                    }
                    Filename = Path.Combine(Config.rdpDir, name + ".rdp");
                }

                if (!File.Exists(Filename))
                {
                    var file = File.Create(Filename);
                    file.Close();
                    justCreated = true;
                    Logger.Info($"Created new RDP file: {Filename}");
                }

                var lines = File.ReadAllLines(Filename);
                var removeList = new[] {
                    "full address:",
                    "username:",
                    "password",
                    "domain:",
                    "winposstr",
                    "desktopwidth:",
                    "desktopheight:",
                    "screen mode id:",
                    "use multimon:",
                    "session bpp:",
                    "audiomode:",
                    "redirectprinters:",
                    "redirectclipboard:",
                    "drivestoredirect:",
                    "redirectdrives:",
                    "redirectports:",
                    "redirectsmartcards:",
                    "prompt for credentials:",
                    "authentication level:",
                    "enablecredsspsupport:",
                };

                var result = new List<string>();

                foreach (var line in lines)
                {
                    var ok = true;

                    foreach (var startKeyword in removeList)
                    {
                        if (line.StartsWith(startKeyword))
                        {
                            ok = false;
                            break;
                        }
                    }

                    if (ok)
                    {
                        result.Add(line);
                    }
                }

                if (Computer != "")
                {
                    result.Add("full address:s:" + Computer);
                }

                if (Username != "")
                {
                    result.Add("username:s:" + Username);
                }

                if (Password != "")
                {
                    result.Add("password 51:b:" + GetRDPEncryptedPassword());
                }

                if (Domain != "")
                {
                    result.Add("domain:s:" + Domain);
                }

                result.Add("desktopwidth:i:" + DesktopWidth);
                result.Add("desktopheight:i:" + DesktopHeight);
                result.Add("screen mode id:i:" + ScreenMode);
                result.Add("use multimon:i:" + UseMultiMon);
                result.Add("session bpp:i:" + ColorDepth);
                result.Add("audiomode:i:" + AudioMode);
                result.Add("redirectprinters:i:" + RedirectPrinters);
                result.Add("redirectclipboard:i:" + RedirectClipboard);
                result.Add("redirectdrives:i:" + RedirectDrives);
                result.Add("redirectports:i:" + RedirectPorts);
                result.Add("redirectsmartcards:i:" + RedirectSmartCards);
                result.Add("prompt for credentials:i:" + PromptForCredentials);
                result.Add("authentication level:i:" + AuthenticationLevel);
                result.Add("enablecredsspsupport:i:" + EnableCredSSPSupport);

                var xBuffer = 10;
                var yBuffer = 25;

                Rectangle resolution = Screen.PrimaryScreen.Bounds;
                var left = resolution.Size.Width / 2 - DesktopWidth / 2 - xBuffer;
                var top = resolution.Size.Height / 2 - DesktopHeight / 2 - yBuffer;
                var right = resolution.Size.Width / 2 + DesktopWidth / 2 + xBuffer;
                var bottom = resolution.Size.Height / 2 + DesktopHeight / 2 + yBuffer;
                result.Add($"winposstr:s:0,1,{left},{top},{right},{bottom}");

                if (justCreated)
                {
                    result.Add("promptcredentialonce:i:0");
                }

                var writer = new StreamWriter(Filename, false);

                foreach (var line in result)
                {
                    writer.WriteLine(line);
                }

                writer.Close();
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to prepare RDP file for profile: {Name}", ex);
                throw;
            }
        }

        [JsonIgnore] public bool JustAdded { get; set; } = false;

        public void Delete()
        {
            try
            {
                if (File.Exists(Filename))
                {
                    File.Delete(Filename);
                    Logger.Info($"Deleted RDP file: {Filename}");
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to delete RDP file: {Filename}", ex);
            }
        }
    }
}
