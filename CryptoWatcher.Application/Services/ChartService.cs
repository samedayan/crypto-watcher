﻿using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CesarBmx.Shared.Persistence.Repositories;
using CryptoWatcher.Domain.ModelBuilders;
using CryptoWatcher.Domain.Expressions;
using CryptoWatcher.Domain.Models;
using CryptoWatcher.Domain.Types;

namespace CryptoWatcher.Application.Services
{
    public class LineChartService
    {
        private readonly IRepository<Currency> _currencyRepository;
        private readonly IRepository<Indicator> _indicatorRepository;
        private readonly IRepository<Line> _lineRepository;
        private readonly IMapper _mapper;

        public LineChartService(
            IRepository<Currency> currencyRepository,
            IRepository<Indicator> indicatorRepository,
            IRepository<Line> lineRepository,
            IMapper mapper)
        {
            _currencyRepository = currencyRepository;
            _indicatorRepository = indicatorRepository;
            _lineRepository = lineRepository;
            _mapper = mapper;
        }
        public async Task<List<Responses.LineChart>> GetAllLineCharts(string currencyId = null, IndicatorType? indicatorType = null, string indicatorId = null, string userId = null)
        {
            // Get all currencies
            var currencies = await _currencyRepository.GetAll(CurrencyExpression.CurrencyFilter(currencyId));

            // Get all indicators
            var indicators = await _indicatorRepository.GetAll(IndicatorExpression.IndicatorFilter(indicatorType, indicatorId, userId));

            // Get all lines
            var lines = await _lineRepository.GetAll(LineExpression.LineFilter(currencyId, indicatorType, indicatorId, userId));

            // Build
            var lineCharts = LineChartBuilder.BuildLineCharts(currencies, indicators, lines);

            // Response
            var response = _mapper.Map<List<Responses.LineChart>>(lineCharts);

            // Return
            return response;
        }
    }
}
