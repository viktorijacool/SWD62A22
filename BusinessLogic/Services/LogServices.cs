using Domain.Interfaces;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.Services
{
    public class LogServices
    {
        private ILogRepository _logRepository;
        public LogServices(ILogRepository logRepository)
        {
            _logRepository = logRepository;
        }

        public void LogMessage(string message, string type)
        {
            Log l = new Log();
            l.Message = message; 
            l.Type = type;
            l.Date = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            _logRepository.Log(l);
        }
    }
}
