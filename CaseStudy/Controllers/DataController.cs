using CaseStudy.DAL;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net.Http;

namespace CaseStudy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        AppDbContext _ctx;
        public DataController(AppDbContext context) // injected here
        {
            _ctx = context;
        }
        private async Task<String> getMenuItemJsonFromWebAsync()
        {
            string url = "https://gist.github.com/ddcb94b93f256228cd32fbe19a95712e.git";
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(url);
            var result = await response.Content.ReadAsStringAsync();
            return result;
        }

        public async Task<IActionResult> Index()
        {
            var json = await getMenuItemJsonFromWebAsync();
            return Content(json);
        }
    }
}
