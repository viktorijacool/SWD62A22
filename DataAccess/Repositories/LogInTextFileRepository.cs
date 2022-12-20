using Domain.Interfaces;
using Domain.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DataAccess.Repositories
{
    public class LogInTextFileRepository : ILogRepository
    {
        private string _fileName;
        public LogInTextFileRepository(string fileName)
        {
            _fileName = fileName;
        }

        public void Log(Log l)
        {
            //true - append
            using (StreamWriter sw = new StreamWriter(_fileName, true))
            {
                //Ver.1
                //sw.WriteLine($"Type: {l.Type}, Message: {l.Message}");

                //Ver.2
                //Json serizalisation
                string logAsAString = JsonConvert.SerializeObject(l);
                sw.WriteLine(logAsAString);
            }
        }

    }
}
