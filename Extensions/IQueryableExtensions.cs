using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vet.BL.Pagination;

namespace Vet.Extensions
{
    public static class IQueryableExtensions
    {

        public async static Task<PagedList<TTo>> ToPagedList<TFrom,TTo>(this IQueryable<TFrom> query, IMapper mapper,PaginationData pgd)
        {
            return new PagedList<TTo> { 
                Total = query.Count(), 
                Items = mapper.Map<ICollection<TTo>>(await query.Skip(pgd.PageIndex*pgd.PageSize).Take(pgd.PageSize).ToListAsync())
            };
        }
    }
}
