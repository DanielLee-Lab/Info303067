using CaseStudy.DAL;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

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
        private async Task<String> getProductJsonFromWebAsync()
        {
            string url = "https://raw.githubusercontent.com/DanielLee-Lab/Info303067/master/CaseStudy/CaseStudyJsonFile.json";
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(url);
            var result = await response.Content.ReadAsStringAsync();
            return result;
        }

        public async Task<ActionResult<String>> Index()
        {
            DataUtility util = new DataUtility(_ctx);
            string payload = "";
            var json = await getProductJsonFromWebAsync();
            try
            {
                payload = (await util.loadProductInfoFromWebToDb(json)) ? "tables loaded" : "problem loading tables";
            }
            catch (Exception ex)
            {
                payload = ex.Message;
            }
            return JsonSerializer.Serialize(payload);
        }
    }
}
