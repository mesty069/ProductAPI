using Product.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Product.Infrastructure.Data;
using Product.Core.Entities;

namespace Product.Infrastructure.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BasicEntity<int>
    {
        private readonly ApplicationDbContext _context;
        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task AddAsync(T entity)
        {
            try
            {
                await _context.Set<T>().AddAsync(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            { 
            
            }
        }

        /// <summary>
        /// 資料數量
        /// </summary>
        /// <returns></returns>
        public async Task<int> CountAsync()
         => await _context.Set<T>().CountAsync();

        /// <summary>
        /// 刪除資料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// 取得全部
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T> GetAll()
            => _context.Set<T>().AsNoTracking().ToList();

        public IEnumerable<T> GetAll(params Expression<Func<T, bool>>[] includes)
            => _context.Set<T>().AsNoTracking().ToList();

        /// <summary>
        /// 非同步取得所有資料
        /// </summary>
        /// <returns></returns>
        public async Task<IReadOnlyList<T>> GetAllAsync()
        => await _context.Set<T>().AsNoTracking().ToListAsync();

        public async Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] include)
        {
            var query = _context.Set<T>().AsQueryable();
            foreach (var item in include)
            {
                query = query.Include(item);
            }
            return await query.ToListAsync();
        }

        /// <summary>
        /// 取得指定資料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<T> GetAsync(T id)
            => await _context.Set<T>().FindAsync(id);

        public async Task<T> GetAsync(int id)
            => await _context.Set<T>().FindAsync(id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public async Task<T> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>().Where(x => x.Id == id);
            foreach (var item in includes)
            { 
                query = query.Include(item);
            }
            return await query.FirstOrDefaultAsync();
        }


        public async Task UpdateAsync(int id, T entity)
        {
            var entity_value = await _context.Set<T>().FindAsync(id);
            if (entity_value is not null)
            { 
                _context.Update(entity_value);
                await _context.SaveChangesAsync();
            }
        }
    }
}
