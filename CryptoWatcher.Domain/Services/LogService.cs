﻿using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoWatcher.Domain.Messages;
using CryptoWatcher.Domain.Models;
using CryptoWatcher.Domain.Repositories;
using CryptoWatcher.Shared.Exceptions;

namespace CryptoWatcher.Domain.Services
{
    public class LogService
    {
        private readonly ILogRepository _logRepository;

        public LogService(ILogRepository logRepository)
        {
            _logRepository = logRepository;
        }

        public async Task<List<Log>> GetLog()
        {
            // Get Log
            return await _logRepository.GetAll();
        }
        public async Task<Log> GetLog(string id)
        {
            // Get Log by key
            var log = await _logRepository.GetByKey(id);

            // Throw NotFound exception if it does not exist
            if (log == null) throw new NotFoundException(LogMessages.NotFound);

            // Return
            return log;
        }
        public void Log(Log log)
        {
            // Add log
            _logRepository.Add(log);
        }
    }
}