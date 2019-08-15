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
                        ShippingAddress = new StreetAddress { City = "London", Street = "221 B Baker St", IgnoreMe = "Hello World" },
                        BillingAddress = new StreetAddress { City = "New York", Street = "11 Wall Street", IgnoreMe = "Hello World" }
                    }
                });

                Console.WriteLine("Saving changes");
                context.SaveChanges();
            }

            using (var context = new OwnedEntityContext())
            {
                #region DetailedOrderQuery
                var order = context.DetailedOrders.First(o => o.Status == OrderStatus.Pending);
                Console.WriteLine($"First pending order will ship to: {order.OrderDetails.ShippingAddress.City}");
                Console.WriteLine($"IgnoreMe value: {order.OrderDetails.ShippingAddress.IgnoreMe}");
                #endregion
            }
        }
    }
}
