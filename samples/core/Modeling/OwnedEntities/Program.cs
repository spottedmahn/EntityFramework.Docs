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
                        //BillingAddress = new StreetAddress()
                        //BillingAddress = new StreetAddress { City = "New York", Street = "11 Wall Street", IgnoreMe = "Hello World" }
                    }
                });
                //System.InvalidOperationException: 'The entity of type 'OrderDetails' is sharing the table 'OrderDetails' with entities of type 'OrderDetails.BillingAddress#StreetAddress', but there is no entity of this type with the same key value that has been marked as 'Added'. Consider using 'DbContextOptionsBuilder.EnableSensitiveDataLogging' to see the key values.'
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
