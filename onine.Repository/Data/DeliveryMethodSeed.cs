using Microsoft.EntityFrameworkCore;
using onine.core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace onine.Repository.Data
{
    public static class DeliveryMethodSeed
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DeliveryMethod>().HasData(
                new DeliveryMethod
                {
                    Id = 1,
                    ShortName = "UPS",
                    Description = "UPS Delivery",
                    DeliveryTime = "1-2 days",
                    Price = 10.00m
                },
                new DeliveryMethod
                {
                    Id = 2,
                    ShortName = "FedEx",
                    Description = "FedEx Delivery",
                    DeliveryTime = "2-3 days",
                    Price = 15.00m
                },
                new DeliveryMethod
                {
                    Id = 3,
                    ShortName = "DHL",
                    Description = "DHL Delivery",
                    DeliveryTime = "3-5 days",
                    Price = 20.00m
                }
            );
        }
    }
}
