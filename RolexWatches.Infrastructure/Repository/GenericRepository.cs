using Microsoft.EntityFrameworkCore;
using RolexWatches.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace RolexWatches.Infrastructure.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ApplicationDbContext Context;
        protected readonly DbSet<T> DbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            Context = context;
            DbSet = context.Set<T>();
        }
        public async Task<T?> GetByIdAsync(int id) => await DbSet.FindAsync(id);
        public async Task<IReadOnlyList<T>> GetAllAsync() => await DbSet.ToListAsync();
        public async Task AddAsync(T entity) => await DbSet.AddAsync(entity);
        public void Update(T entity) => DbSet.Update(entity);
        public void Remove(T entity) => DbSet.Remove(entity);
        public async Task<bool> SaveChangesAsync() => await Context.SaveChangesAsync() > 0;
    }

    public interface IGenericRepository<T> where T : class
    {
    }
}
