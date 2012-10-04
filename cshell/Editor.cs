using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections;
using System.Reflection;

namespace cshell
{
    public partial class MainWindow
    {
        static IEnumerable<string> ReadLines(string path)
        {
            using (var file = File.OpenText(path))
            {
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    yield return line;
                }
            }
        }
        
        private string GetLastLine()
        {
            string line = this.textBox1.GetLineText(this.textBox1.LineCount - 1);
            string prompt = Prompt;
            if(line.StartsWith(prompt))
                line = line.Substring(prompt.Length);
            return line;
        }

        private void Execute(string expression)
        {
            object result = Evaluate(expression);
            Write(result);            
        }

        private object Evaluate(string expression)
        {
            foreach (string key in Macros.Keys)
                expression = Regex.Replace(expression, key, Macros[key]);

            object output = null;
            try
            {
                output = Session.Execute(expression);
            }
            catch (Exception exc)
            {
                WriteLine(exc.ToString());
            }
            return output;
        }

        private void Write(object obj)
        {
            long temp;
            if (obj == null)
                ;//WriteLine();
            else if (obj is string)
                WriteLine(obj as string);
            else if (obj is char)
                WriteLine(obj.ToString());
            else if (Int64.TryParse(obj.ToString(), out temp))
                WriteLine(obj.ToString());
            else if (obj is string[])
            {
                string[] array = (obj as string[]);
                if (array.Length == 0)
                    WriteLine();
                else
                    array.ToList().ForEach(a => WriteLine(a));
            }
            else if (obj is IEnumerable)
            {
                IEnumerable enumerable = obj as IEnumerable;
                foreach (object item in enumerable)
                {
                    Write(item);
                }
            }
            else
                WriteLine(obj.ToString());
        }

        private void Write(string line)
        {
            if (!string.IsNullOrEmpty(line))
                textBox1.Text = textBox1.Text + line;
            textBox1.CaretIndex = textBox1.Text.Length;
        }

        private void WriteLine()
        {
            WriteLine(null);
        }

        private void WriteLine(string line)
        {
            Write(line);
            textBox1.Text = textBox1.Text + "\n";
            textBox1.CaretIndex = textBox1.Text.Length;
        }
    }
}
