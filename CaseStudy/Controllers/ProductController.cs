using System.Collections.Generic;
using System.Threading.Tasks;
using CaseStudy.DAL;
using CaseStudy.DAL.DAO;
using CaseStudy.DAL.DomainClasses;
using Microsoft.AspNetCore.Mvc;
namespace CaseStudy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        AppDbContext _db;
        public ProductController(AppDbContext context)
        {
            _db = context;
        }
        [Route("{catid}")]
        public async Task<ActionResult<List<Product>>> Index(int catid)
        {
            ProductDAO dao = new ProductDAO(_db);
            List<Product> itemsForBrand = await dao.GetAllByBrand(catid);
            return itemsForBrand;
        }
    }
}
