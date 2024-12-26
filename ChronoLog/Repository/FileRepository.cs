using ChronoLog.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ChronoLog.Repository
{
    class FileRepository
    {
        private readonly string _pathHistory;
        private readonly string _pathConfig;

        public FileRepository(string path, string pathConfig)
        {
            _pathHistory = path;
            _pathConfig = pathConfig;
        }

        public List<History> GetHistories()
        {
            List<History> histories = new List<History>();

            if (File.Exists(_pathHistory))
                using (FileStream fl = new FileStream(_pathHistory, FileMode.Open, FileAccess.Read))
                {
                    using (StreamReader sr = new StreamReader(fl, Encoding.Unicode))
                    {
                        while (sr.Peek() > 0)
                        {

                            string[] parts = sr.ReadLine().Split(',');
                            if (parts.Length == 2)
                            {
                                DateTime date = DateTime.ParseExact(parts[0], "o", CultureInfo.InvariantCulture);
                                TimeSpan duration = TimeSpan.Parse(parts[1]);
                                histories.Add(new History { Date = date, Duration = duration.Duration() });
                            }
                        }
                    }
                }

            return histories;
        }

        public void WriteHistory(History history)
        {
            string line = $"{DateTime.Now.Date:o},{history.Duration}";

            if (File.Exists(_pathHistory))
            {
                using (FileStream fl = new FileStream(_pathHistory, FileMode.Append, FileAccess.Write))
                {
                    using (StreamWriter sr = new StreamWriter(fl, Encoding.Unicode))
                    {

                        sr.WriteLine(line);
                    }
                }
            }
            else
            {
                using (FileStream fl = new FileStream(_pathHistory, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    using (StreamWriter sr = new StreamWriter(fl, Encoding.Unicode))
                    {
                        sr.WriteLine(line);
                    }
                }
            }
        }

        public string ReadConfig()
        {
            if (File.Exists(_pathConfig))
            {
                using (FileStream fl = new FileStream(_pathConfig, FileMode.Open, FileAccess.Read))
                using (StreamReader sr = new StreamReader(fl, Encoding.Unicode))
                {
                    return sr.ReadLine();
                }
            }
            return null;
        }

        public void WriteConfig(string color)
        {
            using (FileStream fl = new FileStream(_pathConfig, FileMode.Create, FileAccess.Write))
            using (StreamWriter sr = new StreamWriter(fl, Encoding.Unicode))
            {
                sr.WriteLine(color);
            }
        }
    }
}
