﻿using CryptoWatcher.Application.Indicators.Requests;

namespace CryptoWatcher.Application.Indicators.FakeRequests
{
    public static class UpdateIndicatorFakeRequest
    {
        public static UpdateIndicatorRequest GetFake_1()
        {
            return new UpdateIndicatorRequest
            {
                IndicatorId = "johny.melavo-hype",
                Name = "Hype",
                Description = "It identifies immediate hypes within 24 Hrs in your portfolio",
                Formula = "C# formula"
            };
        }       
    }
}