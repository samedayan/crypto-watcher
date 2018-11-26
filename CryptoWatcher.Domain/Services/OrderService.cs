﻿using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoWatcher.Domain.Messages;
using CryptoWatcher.Domain.Models;
using CryptoWatcher.Domain.Repositories;
using CryptoWatcher.Shared.Exceptions;

namespace CryptoWatcher.Domain.Services
{
    public class OrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly UserService _userService;
        private readonly WatcherService _watcherService;

        public OrderService(
            IOrderRepository orderRepository,
            UserService userService,
            WatcherService watcherService)
        {
            _orderRepository = orderRepository;
            _userService = userService;
            _watcherService = watcherService;
        }

        public async Task<List<Order>> GetOrders(string userId)
        {
            // Get user
            var user = await _userService.GetUser(userId);

            // Get user orders
            var userOrders = await _orderRepository.GetByUserId(user.UserId);

            // Return
            return userOrders;
        }
        public async Task<Order> GetOrder(string orderId)
        {
            // Get order
            var order = await _orderRepository.GetByOrderId(orderId);

            // Throw NotFound exception if it does not exist
            if (order == null) throw new NotFoundException(OrderMessages.OrderNotFound);

            // Return
            return order;
        }
        public async Task<Order> AddOrder(OrderType operationType, string userId, string currencyId, string watcherId, decimal orderQuantity)
        {
            // Add order
            var order = new Order(operationType, userId, currencyId, watcherId, orderQuantity);
            _orderRepository.Add(order);

            // Return
            return await Task.FromResult(order);
        }
        public async Task AddOrdersFromWatchers()
        {
            // Get watchers willing to buy
            var watchersBuys = await _watcherService.GetWatchersWillingToBuy();
            foreach (var watcher in watchersBuys)
            {
                // Get ongoing orders
                var orders = await _orderRepository.GetByUserIdAndCurrencId(watcher.UserId, watcher.CurrencyId);

                // Add order if there is no conflict
                if (orders.Count == 0)
                {
                    var order = new Order(OrderType.BuyLimit, watcher.UserId, watcher.CurrencyId, watcher.WatcherId, 100);
                    _orderRepository.Add(order);
                }
            }

            // Get watchers willing to sell
            var watchersSells = await _watcherService.GetWatchersWillingToSell();
            foreach (var watcher in watchersSells)
            {
                // Get ongoing orders
                var orders = await _orderRepository.GetByUserIdAndCurrencId(watcher.UserId, watcher.CurrencyId);

                // Add order if there is no conflict
                if (orders.Count == 0)
                {
                    var order = new Order(OrderType.SellLimit, watcher.UserId, watcher.CurrencyId, watcher.WatcherId, 100);
                    _orderRepository.Add(order);
                }
            }
        }
    }
}
