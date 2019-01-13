﻿using System;
using System.Diagnostics;
using System.Threading.Tasks;
using CryptoWatcher.Domain.Builders;
using CryptoWatcher.Domain.Expressions;
using Hangfire;
using CryptoWatcher.Domain.Models;
using CryptoWatcher.Persistence.Contexts;
using CryptoWatcher.Persistence.Repositories;
using CryptoWatcher.Shared.Extensions;
using Microsoft.Extensions.Logging;

namespace CryptoWatcher.BackgroundJobs
{
    public class UpdateDefaultWatchersJob
    {
        private readonly MainDbContext _mainDbContext;
        private readonly ILogger<UpdateDefaultWatchersJob> _logger;
        private readonly ILineRepository _lineRepository;
        private readonly IRepository<Watcher> _watcherRepository;
        public UpdateDefaultWatchersJob(
            MainDbContext mainDbContext,
            ILogger<UpdateDefaultWatchersJob> logger,
            ILineRepository lineRepository,
            IRepository<Watcher> watcherRepository)
        {
            _mainDbContext = mainDbContext;
            _logger = logger;
            _lineRepository = lineRepository;
            _watcherRepository = watcherRepository;

        }

        [AutomaticRetry(OnAttemptsExceeded = AttemptsExceededAction.Delete)]
        public async Task Run()
        {
            try
            {
                // Start watch
                var stopwatch = new Stopwatch();
                stopwatch.Start();

                // Get all current lines
                var lines = await _lineRepository.GetCurrentLines();

                // Build default watchers
                var newDefaultWatchers = WatcherBuilder.BuildDefaultWatchers(lines);

                // Get all default watchers
                var defaultWatcherExpression = WatcherExpression.DefaultWatcher();
                var defaultWatchers = await _watcherRepository.GetAll(defaultWatcherExpression);

                // Update 
                _watcherRepository.UpdateCollection(defaultWatchers, newDefaultWatchers);

                // Save
                await _mainDbContext.SaveChangesAsync();

                // Stop watch
                stopwatch.Stop();

                // Log into Splunk
                _logger.LogSplunkJob(new
                {
                    newDefaultWatchers.Count,
                    ExecutionTime = stopwatch.Elapsed.TotalSeconds
                });

                // Return
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                // Log into Splunk 
                _logger.LogSplunkJob(new
                {
                    JobFailed = ex.Message
                });
                _logger.LogSplunkError(ex);
            }
        }
    }
}