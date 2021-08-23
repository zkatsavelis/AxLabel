using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace label
{
    class Program
    {
        public string newLabelTxt;
        public Config config;
        static async Task Main(string[] args)
        {
            Program pg = new Program()
            {
                newLabelTxt = String.Join(" ", args),
                config = JsonSerializer.Deserialize<Config>(File.ReadAllText("config.json"))
            };

            var label = await pg.run();
            Console.WriteLine(String.Format("@{0}", label));
        }

        public async Task<string> run()
        {
            string label = await this.findTextInFile(this.getPrimaryLabelFile(), this.newLabelTxt);
            if (label == String.Empty)
            {
                label = this.getLabelFromText(this.newLabelTxt);

                await Task.WhenAll(this.createLabelCreationTasks(label));
            }

            return String.Format("{0}:{1}", this.config.ModelPrefix, label);
        }

        public IEnumerable<Task> createLabelCreationTasks(string label)
        {
            List<Task> ret = new List<Task>();
            foreach (var labelFile in this.getLabelFiles())
            {
                ret.Add(this.createLabelToFile(label, this.newLabelTxt, labelFile));
            }

            return ret;
        }

        public async Task createLabelToFile(string label, string labelText, string labelFilePath)
        {
            string line = String.Format("{0}={1}", label, labelText);
            List<string> lines = new List<string>();
            lines.Add(line);

            await File.AppendAllLinesAsync(labelFilePath, lines);
        }

        public string getLabelFromText(string labelText)
        {
            string ret = labelText;

            TextInfo ti = new CultureInfo(this.config.UserCulture, false).TextInfo;
            ret = ti.ToTitleCase(ret);
            ret = ret.Replace(" ", String.Empty);
            ret = ret.Replace("!", String.Empty);
            ret = ret.Replace("@", String.Empty);
            ret = ret.Replace("#", String.Empty);
            ret = ret.Replace("$", String.Empty);
            ret = ret.Replace("^", String.Empty);
            ret = ret.Replace("&", String.Empty);
            ret = ret.Replace("*", String.Empty);
            ret = ret.Replace("-", String.Empty);
            ret = ret.Replace("+", String.Empty);
            ret = ret.Replace("=", String.Empty);
            ret = ret.Replace(".", String.Empty);
            ret = ret.Replace(",", String.Empty);
            ret = ret.Replace("'", String.Empty);
            ret = ret.Replace("\"", String.Empty);
            ret = ret.Replace("<", String.Empty);
            ret = ret.Replace(">", String.Empty);
            ret = ret.Replace("(", String.Empty);
            ret = ret.Replace(")", String.Empty);
            ret = ret.Replace("%", "_");

            return ret;
        }

        public IEnumerable<String> getLabelFiles()
        {
            return this.config.LabelFiles;
        }

        public string getPrimaryLabelFile()
        {
            return this.config.PrimaryFile;
        }
        public async Task<string> findTextInFile(string _filePath, string textToSearch)
        {
            string line;
            string key = String.Empty;
            StreamReader file = new StreamReader(_filePath);
            while ((line = await file.ReadLineAsync()) != null)
            {
                var lineParts = line.Split('=');

                foreach (var linePart in lineParts)
                {
                    if (linePart.Equals(textToSearch, StringComparison.OrdinalIgnoreCase))
                    {
                        key = lineParts[0];
                        break;
                    }
                }
                if (key != String.Empty)
                {
                    break;
                }
            }

            file.Close();

            return key;
        }
    }
}

