using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using CaseStudy.DAL.DomainClasses;
namespace CaseStudy.DAL
{
    public class DataUtility
    {
        private AppDbContext _db;
        public DataUtility(AppDbContext context)
        {
            _db = context;
        }

        public async Task<bool> loadProductInfoFromWebToDb(string stringJson)
        {
            bool brandsLoaded = false;
            bool productsLoaded = false;
            try
            { // an element that is typed as dynamic is assumed to support any operation
                dynamic objectJson = JsonSerializer.Deserialize<Object>(stringJson);
                brandsLoaded = await loadBrands(objectJson);
                productsLoaded = await loadProducts(objectJson);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return brandsLoaded && productsLoaded;
        }
        private async Task<bool> loadBrands(dynamic jsonObjectArray)
        {
            bool loadedBrands = false;
            try
            {
                // clear out the old rows
                _db.Brands.RemoveRange(_db.Brands);
                await _db.SaveChangesAsync();
                List<String> allBrands = new List<String>();
                foreach (JsonElement element in jsonObjectArray.EnumerateArray())
                {
                    if (element.TryGetProperty("Brand", out JsonElement productItemJson))
                    {
                        allBrands.Add(productItemJson.GetString());
                    }
                }
                IEnumerable<String> brands = allBrands.Distinct<String>(); foreach (string brandname in brands)
                {
                    Brand cat = new Brand();
                    cat.Name = brandname;
                    await _db.Brands.AddAsync(cat);
                    await _db.SaveChangesAsync();
                }
                loadedBrands = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error - " + ex.Message);
            }
            return loadedBrands;
        }

        private async Task<bool> loadProducts(dynamic jsonObjectArray)
        {
            bool loadedProducts = false;
            try
            {
                List<Brand> brands = _db.Brands.ToList();
                // clear out the old
                _db.Products.RemoveRange(_db.Products);
                await _db.SaveChangesAsync();
                foreach (JsonElement element in jsonObjectArray.EnumerateArray())
                {
                    Product pro = new Product();
                    pro.Id = element.GetProperty("Id").GetString();
                    pro.ProductName = element.GetProperty("ProductName").GetString();
                    pro.GraphicName = element.GetProperty("GraphicName").GetString();
                    pro.CostPrice = Convert.ToDecimal(element.GetProperty("CostPrice"));
                    pro.MSRP = Convert.ToDecimal(element.GetProperty("MSRP"));
                    pro.QtyOnHand = Convert.ToInt32(element.GetProperty("QtyOnHand"));
                    pro.QtyOnBackOrder = Convert.ToInt32(element.GetProperty("QtyOnHandOnBackOrder"));
                    pro.Description = element.GetProperty("Description").GetString();
                    string cat = element.GetProperty("Brand").GetString();
                    // add the FK here
                    foreach (Brand brand in brands)
                    {
                        if (brand.Name == cat)
                        {
                            pro.Brand = brand;
                            break;
                        }
                    }
                    await _db.Products.AddAsync(pro);
                    await _db.SaveChangesAsync();
                }
                loadedProducts = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error - " + ex.Message);
            }
            return loadedProducts;
        }

    }
}