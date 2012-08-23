using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cshell
{
    public partial class MainWindow
    {
        public string Prompt
        {
            get
            {
                return Evaluate("prompt();") as string;
            }
            private set { }
        }

        private void ShowPrompt()
        {
            Write(Prompt);
        }
    }
}
