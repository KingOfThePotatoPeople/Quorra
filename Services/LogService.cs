using System;
using System.Linq;
using OpenIddict.Core;
using Quorra.EntityFramework;
using Quorra.Interfaces;

namespace Quorra.Services
{
    
    public class LogService : ILogService
    {

        private readonly QuorraDbContext _dbContext;

        public LogService(QuorraDbContext dbContext)
        {
            _dbContext = dbContext;
        }



        public object GetAuthLogs()
        {
            return _dbContext.EventLog.ToList().Take(10);
        }
    }
}