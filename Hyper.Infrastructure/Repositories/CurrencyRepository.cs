﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Hyper.Domain.Models;
using Hyper.Domain.Repositories;

namespace Hyper.Infrastructure.Repositories
{
    public class CurrencyRepository : ICurrencyRepository
    {
        private IEnumerable<Currency> _currencies;

        public CurrencyRepository()
        {
            _currencies = new List<Currency>();
        }

        public async Task<IEnumerable<Currency>> GetAllCurrencies()
        {
            // Get  all currencies
            return await Task.FromResult(_currencies);
        }
        public async Task SetAllCurrencies(IEnumerable<Currency> currencies)
        {
            // Set all currencies
            _currencies = currencies;
            await Task.CompletedTask;
        }
    }
}
