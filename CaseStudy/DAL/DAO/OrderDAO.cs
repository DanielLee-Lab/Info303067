using CaseStudy.DAL.DomainClasses;
using CaseStudy.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace CaseStudy.DAL.DAO
{
    public class OrderDAO
    {
        private AppDbContext _db;
        public OrderDAO(AppDbContext ctx)
        {
            _db = ctx;
        }
        public async Task<int> AddOrder(int userid, OrderSelectionHelper[] selections)
        {
            int orderId = -1;
            using (_db)
            {
                // we need a transaction as multiple entities involved
                using (var _trans = await _db.Database.BeginTransactionAsync())
                {
                    try
                    {
                        Order order = new Order();
                        order.UserId = userid;
                        order.OrderDate = System.DateTime.Now;
                        order.OrderAmount = 0;
                        
                        // calculate the totals and then add the tray row to the table
                        foreach (OrderSelectionHelper selection in selections)
                        {
                            order.OrderAmount += selection.Qty;
                            
                        }
                        await _db.Orders.AddAsync(order);
                        await _db.SaveChangesAsync();
                        // then add each item to the trayitems table
                        foreach (OrderSelectionHelper selection in selections)
                        {
                            OrderLineItem oItem = new OrderLineItem();
                            Product pItem = selection.item;

                            oItem.OrderId = order.Id;
                            oItem.ProductId = selection.item.Id;
                            oItem.SellingPrice = pItem.CostPrice;
                            pItem.Id = selection.item.Id;

                            if(selection.Qty < selection.item.QtyOnHand)
                            {
                                pItem.QtyOnHand =- selection.Qty;
                                oItem.QtySold = selection.Qty;
                                oItem.QtyOrdered = selection.Qty;
                                oItem.QtyBackOrdered = 0;
                            }
                            else if (selection.Qty > selection.item.QtyOnHand)
                            {
                                selection.item.QtyOnHand = 0;
                                selection.item.QtyOnBackOrder += selection.Qty - selection.item.QtyOnHand;
                                oItem.QtySold = selection.item.QtyOnHand;
                                oItem.QtyOrdered = selection.Qty;
                                oItem.QtyBackOrdered = selection.Qty - selection.item.QtyOnHand;
                            }

                            await _db.OrderLineItems.AddAsync(oItem);
                            await _db.SaveChangesAsync();
                        }
                        // test trans by uncommenting out these 3 lines
                        //int x = 1;
                        //int y = 0;
                        //x = x / y; 
                        await _trans.CommitAsync();
                        orderId = order.Id;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        await _trans.RollbackAsync();
                    }
                }
            }
            return orderId;
        }
    }
}
