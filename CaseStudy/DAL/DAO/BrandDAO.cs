using CaseStudy.DAL.DomainClasses;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace CaseStudy.DAL.DAO
{
    public class CategoryDAO
    {
        private AppDbContext _db;
        public CategoryDAO(AppDbContext ctx)
        {
            _db = ctx;
        }
        public async Task<List<Brand>> GetAll()
        {
            return await _db.Brands.ToListAsync<Brand>();
        }
    }
}