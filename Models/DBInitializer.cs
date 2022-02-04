using Project40_API_Dot_NET.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project40_API_Dot_NET.Models
{
    public static class DBInitializer
    {
        public static void Initialize(PlantContext context)
        {
            context.Database.EnsureCreated();

            // Look for any products.
            if (context.Users.Any())
            {
                return; // DB has been seeded
            }

            //Add Users
            User user = new User()
            {
                Name = "Niels",
                Role = Role.Admin,
                Email = "niels@msn.com",
                Password = "Niels123*"
            };
            context.Add(user);

            //Add cameraboxes
            CameraBox cameraBox = new CameraBox()
            {
                QrCode = "1234567890",
                UserId= 1
            };
            context.Add(cameraBox);

            //Results
            Result result = new Result()
            {
                Accuracy = 84.3,
                Prediction = "WEEK2"
            };

            context.Add(result);

            //Plants
            Plant plant = new Plant()
            {
                FotoPath="imgLink.blob",
                Location= "@51.1609429,4.9592103,17z",
                ResultId = 1,
                UserId = 1
            };

            context.Add(plant);

            context.SaveChanges();
        }
    }
}
