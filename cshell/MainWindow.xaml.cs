using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Roslyn.Compilers;
using Roslyn.Scripting.CSharp;
using Roslyn.Scripting;
using System.IO;

namespace cshell
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ScriptEngine Engine = null;
        Session Session = null;
        Host Host = new Host();

        public MainWindow()
        {
            InitializeComponent();
            
            Engine = new ScriptEngine(
            new[] {"System",
                "System.Core",
                   "PresentationCore",
                   this.GetType().Assembly.Location});

            // Create the host object model and seed the Session with it.
            Session = Session.Create(this.Host);
        }

        

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string command = GetLastLine();

                WriteLine();

                object result = Evaluate(command);
                if (result != null)
                    Write(result);

                ShowPrompt();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.textBox1.Focus();
            RunProfiles();
            ShowPrompt();
        }

        private void textBox1_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back)
            {
                string command = GetLastLine();
                if (Prompt.StartsWith(command))
                {
                    e.Handled = true;
                    return;
                }

            }
        }
    }

    public class Host
    {
        public string Output = null;
    }
}
