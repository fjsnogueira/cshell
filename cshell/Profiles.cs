using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace cshell
{
    public partial class MainWindow
    {
        public void RunProfiles()
        {
            // run built-in profile
            string[] lines = Properties.Resources.Profile
                .Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            lines.ToList().ForEach(line =>
            {
                Execute(line);
            });

            Dictionary<string, string> retval = new Dictionary<string, string>();

            // now read in user profile
            string myDocumentsFolder = Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
            if (Directory.Exists(myDocumentsFolder))
            {
                string profilePath = System.IO.Path.Combine(myDocumentsFolder, "chsell");
                if (Directory.Exists(profilePath))
                {
                    string userProfilePath = System.IO.Path.Combine(profilePath, "profile.cs");
                    if (File.Exists(userProfilePath))
                    {
                        IEnumerable<string> extLines = ReadLines(userProfilePath);
                        if (extLines != null)
                        {
                            extLines.ToList().ForEach(line =>
                            {
                                Execute(line);
                            });
                        }
                    }
                }
            }
        }
    }
}
