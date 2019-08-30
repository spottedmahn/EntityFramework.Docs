using System;
using System.Linq;

namespace OwnedEntities
{
    public static class Program
    {
        static void Main(string[] args)
        {
            using (var context = new OwnedEntityContext())
            {
                Console.WriteLine("Deleting the DB");
                context.Database.EnsureDeleted();
                Console.WriteLine("Creating the DB");
                context.Database.EnsureCreated();

                context.Add(new DetailedOrder
                {
                    Status = OrderStatus.Pending,
                    OrderDetails = new OrderDetails
                    {
                        ShippingAddress = new StreetAddress
                        {
                            City = "London",
                            Street = "221 B Baker St"
                        }
                        //testing 3.0: "Yes, all dependents are now optional"
                        //reference: https://github.com/aspnet/EntityFrameworkCore/issues/9005#issuecomment-477741082
                        //NULL Owned Type Testing
                        //BillingAddress = new StreetAddress
                        //{
                        //    City = "New York",
                        //    Street = "11 Wall Street"
                        //}
                    }
                });
                Console.WriteLine("Saving changes");
                context.SaveChanges();
            }

            using (var context = new OwnedEntityContext())
            {
                #region DetailedOrderQuery
                var order = context.DetailedOrders.First(o => o.Status == OrderStatus.Pending);
                Console.Write("NULL Owned Type Test, Is Billing Address NULL?");
                //PRINTS FALSE
                Console.WriteLine($"{order.OrderDetails.BillingAddress == null}");
                #endregion
            }
        }
    }
}
