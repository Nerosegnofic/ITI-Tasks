using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace WebApplication1.Helpers
{
    public class PaginatedList<T> : List<T>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }

        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            AddRange(items);
        }

        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;

        // Generic CreateAsync that works with EF IQueryable or in-memory IQueryable
        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            int count;
            List<T> items;

            // Check if the provider supports EF Core async
            if (source.Provider is IAsyncQueryProvider)
            {
                // EF Core query
                count = await source.CountAsync();
                items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            }
            else
            {
                // In-memory query
                count = source.Count();
                items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            }

            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }
    }
}
