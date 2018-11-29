﻿using System;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using CryptoWatcher.Domain.Models;
using CryptoWatcher.Persistence.Contexts;
using CryptoWatcher.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CryptoWatcher.Persistence.Repositories
{
    public class Repository<TEntity>: IRepository<TEntity> where TEntity : Entity
    {
        private readonly DbSet<TEntity> _dbSet;

        public Repository(MainDbContext mainDbContext)
        {
            _dbSet = mainDbContext.Set<TEntity>();
        }

        public async Task<List<TEntity>> GetAll()
        {
            // Get all
            return await _dbSet.ToListAsync();
        }
        public async Task<List<TEntity>> Get(Expression<Func<TEntity, bool>> expression)
        {
            // Get all by expression
            return await _dbSet.Where(expression).ToListAsync();
        }
        public async Task<TEntity> GetById(string id)
        {
            // Get by id
            return await _dbSet.FindAsync(id);
        }
        public void Add(TEntity entity)
        {
            // Add
            _dbSet.Add(entity);
        }
        public void Remove(TEntity entity)
        {
            // Remove
            _dbSet.Remove(entity);            
        }
    }
}
