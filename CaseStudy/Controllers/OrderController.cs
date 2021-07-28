using System;
using CaseStudy.DAL;
using CaseStudy.DAL.DAO;
using CaseStudy.DAL.DomainClasses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CaseStudy.Helpers;
using System.Collections.Generic;
namespace CaseStudy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        AppDbContext _ctx;
        public OrderController(AppDbContext context) // injected here
        {
            _ctx = context;
        }
        [HttpPost]
        [Produces("application/json")]
        public async Task<ActionResult<string>> Index(OrderHelper helper)
        {
            string retVal = "";
            try
            {
                CustomerDAO uDao = new CustomerDAO(_ctx);
                Customer orderOwner = await uDao.GetByEmail(helper.email);
                OrderDAO oDao = new OrderDAO(_ctx);
                int orderNum = 0;
                bool isBackOrder = false;
                int orderId = await oDao.AddOrder(orderOwner.Id, helper.selections);
                foreach (OrderSelectionHelper temp in helper.selections)
                {
                    orderNum = temp.item.QtyOnBackOrder;
                    if (orderNum > 0)
                        isBackOrder = true;
                }
                if (orderId > 0)
                {
                    if (isBackOrder == true)
                    {
                        retVal = "Order" + orderId + " created!\n Goods backordered!";
                    }
                    else
                    {
                        retVal = "Order" + orderId + " saved!";
                    }
                }
                else
                {
                    retVal = "Order not saved";
                }
            }
            catch (Exception ex)
            {
                retVal = "Order not saved " + ex.Message;
            }
            return retVal;
        }
    }
}
