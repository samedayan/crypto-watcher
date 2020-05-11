﻿using CesarBmx.Shared.Domain.Models;
using CesarBmx.Shared.Persistence.Repositories;
using CryptoWatcher.Application.Jobs;
using CryptoWatcher.Application.Services;
using CryptoWatcher.Domain.Models;
using CryptoWatcher.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CryptoWatcher.Api.Configuration
{
    public static class DependecyInjectionConfig
    {
        public static IServiceCollection ConfigureDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            //Contexts
            if (bool.Parse(configuration["AppSettings:UseMemoryStorage"]))
            {
                services.AddDbContext<DbContext, MainDbContext>(options => options
                     .UseInMemoryDatabase("CryptoWatcher")
                     .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));
            }
            else
            {
                services.AddDbContext<DbContext, MainDbContext>(options => options
                    .UseSqlServer(configuration.GetConnectionString("CryptoWatcher"))
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));
            }

            // Services
            services.AddScoped<LogService, LogService>();
            services.AddScoped<CurrencyService, CurrencyService>();
            services.AddScoped<WatcherService, WatcherService>();
            services.AddScoped<UserService, UserService>();
            services.AddScoped<NotificationService, NotificationService>();
            services.AddScoped<OrderService, OrderService>();
            services.AddScoped<IndicatorService, IndicatorService>();
            services.AddScoped<LineService, LineService>();
            services.AddScoped<LineChartService, LineChartService>();
            services.AddScoped<ScriptVariableService, ScriptVariableService>();

            //// Repositories
            services.AddScoped<Repository<Log>, Repository<Log>>();
            services.AddScoped<Repository<Currency>, Repository<Currency>>();
            services.AddScoped<Repository<Watcher>, Repository<Watcher>>();
            services.AddScoped<Repository<User>, Repository<User>>();
            services.AddScoped<Repository<Notification>, Repository<Notification>>();
            services.AddScoped<Repository<Order>, Repository<Order>>();
            services.AddScoped<Repository<Indicator>, Repository<Indicator>>();
            services.AddScoped<Repository<IndicatorDependency>, Repository<IndicatorDependency>>();
            services.AddScoped<Repository<Line>, Repository<Line>>();

            //// Logger repositories
            services.AddScoped<IRepository<Log>, Repository<Log>>();
            services.AddScoped<IRepository<Currency>, LoggerRepository<Currency>>();
            services.AddScoped<IRepository<Watcher>, LoggerRepository<Watcher>>();
            services.AddScoped<IRepository<User>, LoggerRepository<User>>();
            services.AddScoped<IRepository<Notification>, LoggerRepository<Notification>>();
            services.AddScoped<IRepository<Order>, LoggerRepository<Order>>();
            services.AddScoped<IRepository<Indicator>, LoggerRepository<Indicator>>();
            services.AddScoped<IRepository<Line>, LoggerRepository<Line>>();
            services.AddScoped<IRepository<IndicatorDependency>, LoggerRepository<IndicatorDependency>>();
            services.AddScoped<IRepository<Line>, LoggerRepository<Line>>();

            // Audit repositories
            //services.AddScoped<IRepository<Log>, Repository<Log>>();
            //services.AddScoped<IRepository<Currency>, AuditRepository<Currency>>();
            //services.AddScoped<IRepository<Watcher>, AuditRepository<Watcher>>();
            //services.AddScoped<IRepository<User>, AuditRepository<User>>();
            //services.AddScoped<IRepository<Notification>, AuditRepository<Notification>>();
            //services.AddScoped<IRepository<Order>, AuditRepository<Order>>();
            //services.AddScoped<IRepository<Indicator>, AuditRepository<Indicator>>();
            //services.AddScoped<IRepository<Line>, AuditRepository<Line>>();
            //services.AddScoped<IRepository<IndicatorDependency>, AuditRepository<IndicatorDependency>>();
            //services.AddScoped<IRepository<Line>, LineAuditRepository>();

            // Jobs
            services.AddScoped<MainJob, MainJob>();
            services.AddScoped<SendWhatsappNotificationsJob, SendWhatsappNotificationsJob>();
            services.AddScoped<SendTelgramNotificationsJob, SendTelgramNotificationsJob>();
            services.AddScoped<RemoveObsoleteLinesJob, RemoveObsoleteLinesJob>();

            // Other
            //services.AddScoped<DateTimeProvider, DateTimeProvider>();
            //services.AddScoped<HttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<CoinpaprikaAPI.Client, CoinpaprikaAPI.Client>();

            return services;
        }
    }
}