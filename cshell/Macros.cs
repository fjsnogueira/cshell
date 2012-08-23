using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace cshell
{
    public partial class MainWindow
    {
        // REWRITE THIS, MACROS SHOULD NOT BE CSV!
        // THEY SHOULD BE CSHARP AND DEFINED BY THE PROFILE
        public Dictionary<string, string> Macros
        {
            get
            {
                Dictionary<string, string> retval = new Dictionary<string, string>();

                // first read the built-ins
                string builtInGlob = Properties.Resources.BuiltInMacros;
                string[] builtInLines = builtInGlob.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                if (builtInLines != null)
                {
                    builtInLines.ToList().ForEach(line =>
                    {
                        string[] arr = line.Split(',');
                        retval.Add(arr[0].Trim('"', ' '), arr[1].Trim());
                    });
                }

                // now read in user macros
                string myDocumentsFolder = Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
                if (!Directory.Exists(myDocumentsFolder))
                    return retval;

                string profilePath = System.IO.Path.Combine(myDocumentsFolder, "chsell");
                if (!Directory.Exists(profilePath))
                    return retval;

                string macrosPath = System.IO.Path.Combine(profilePath, "macros.csv");
                if (!File.Exists(macrosPath))
                    return retval;

                IEnumerable<string> extLines = ReadLines(macrosPath);
                if (extLines != null)
                {
                    extLines.ToList().ForEach(line =>
                    {
                        string[] arr = line.Split(',');
                        string key = arr[0].Trim('"', ' ');
                        if (retval.ContainsKey(key))
                            retval.Remove(key);
                        retval.Add(key, arr[1].Trim());
                    });
                }

                return retval;
            }
            private set { }
        }
    }
}
