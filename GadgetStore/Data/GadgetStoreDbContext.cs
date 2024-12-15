using Microsoft.AspNetCore.Identity;
using GadgetStore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
namespace GadgetStore.Data
{
    public class GadgetStoreDbContext : IdentityDbContext<IdentityUser>
    {
        public GadgetStoreDbContext(DbContextOptions<GadgetStoreDbContext> options) : base(options) { }

        public DbSet<ContactUs> ContactUs { get; set; }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public static async Task SeedDataAsync(GadgetStoreDbContext context, IServiceProvider serviceProvider)
        {
            // Check if categories already exist
            if (!context.Categories.Any())
            {
                var categories = new[]
                {
                    new Category { Name = "Smartphones" },
                    new Category { Name = "Laptops" },
                    new Category { Name = "Accessories" },
                    new Category { Name = "Audio Devices" }
                };
                context.Categories.AddRange(categories);
                context.SaveChanges();
            }

            // Fetch category IDs after saving
            var smartphoneCategory = context.Categories.First(c => c.Name == "Smartphones").Id;
            var laptopCategory = context.Categories.First(c => c.Name == "Laptops").Id;
            var accessoriesCategory = context.Categories.First(c => c.Name == "Accessories").Id;
            var audioDevicesCategory = context.Categories.First(c => c.Name == "Audio Devices").Id;

            // Check if products already exist
            if (!context.Products.Any())
            {
                var products = new[]
                {
                    new Product
                    {
                        Name = "iPhone 14 Pro",
                        Description = "The ultimate smartphone with a 48MP camera, Dynamic Island, and A16 Bionic chip.",
                        Price = 999.99M,
                        ImageUrl = "/images/iphone_14_pro.jpg",
                        CategoryId = smartphoneCategory
                    },
                    new Product
                    {
                        Name = "Samsung Galaxy S23 Ultra",
                        Description = "A top-tier Android smartphone with a 200MP camera and S Pen support.",
                        Price = 1199.99M,
                        ImageUrl = "/images/galaxy_s23_ultra.jpg",
                        CategoryId = smartphoneCategory
                    },
                    new Product
                    {
                        Name = "MacBook Air M2",
                        Description = "Ultra-thin and powerful laptop with the Apple M2 chip, great for professionals.",
                        Price = 1249.99M,
                        ImageUrl = "/images/macbook_air_m2.jpg",
                        CategoryId = laptopCategory
                    },
                    new Product
                    {
                        Name = "Dell XPS 13 Plus",
                        Description = "Compact and sleek laptop with a 12th Gen Intel Core processor.",
                        Price = 1399.99M,
                        ImageUrl = "/images/dell_xps_13_plus.jpg",
                        CategoryId = laptopCategory
                    },
                    new Product
                    {
                        Name = "Sony WH-1000XM5 Headphones",
                        Description = "Industry-leading noise-canceling headphones with premium sound quality.",
                        Price = 399.99M,
                        ImageUrl = "/images/sony_wh_1000xm5.jpg",
                        CategoryId = audioDevicesCategory
                    },
                    new Product
                    {
                        Name = "JBL Flip 6 Bluetooth Speaker",
                        Description = "Portable waterproof speaker with powerful sound and long battery life.",
                        Price = 129.99M,
                        ImageUrl = "/images/jbl_flip_6.jpg",
                        CategoryId = audioDevicesCategory
                    },
                    new Product
                    {
                        Name = "Logitech MX Master 3S",
                        Description = "Precision ergonomic mouse with ultra-fast scrolling and customizable buttons.",
                        Price = 99.99M,
                        ImageUrl = "/images/logitech_mx_master_3s.jpg",
                        CategoryId = accessoriesCategory
                    },
                    new Product
                    {
                        Name = "Corsair K95 RGB Platinum XT",
                        Description = "Premium mechanical keyboard with customizable RGB lighting and macro keys.",
                        Price = 199.99M,
                        ImageUrl = "/images/corsair_k95_rgb.jpg",
                        CategoryId = accessoriesCategory
                    },
                    new Product
                    {
                        Name = "Apple Watch Series 8",
                        Description = "Advanced smartwatch with fitness tracking, crash detection, and temperature sensors.",
                        Price = 399.99M,
                        ImageUrl = "/images/apple_watch_series_8.jpg",
                        CategoryId = accessoriesCategory
                    },
                    new Product
                    {
                        Name = "Bose SoundLink Flex",
                        Description = "Portable Bluetooth speaker with unmatched sound clarity and rugged durability.",
                        Price = 149.99M,
                        ImageUrl = "/images/bose_soundlink_flex.jpg",
                        CategoryId = audioDevicesCategory
                    }
                };
                context.Products.AddRange(products);
                context.SaveChanges();
            }
            // Seed admin user
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // Ensure roles exist
            var adminRole = "Admin";
            if (!await roleManager.RoleExistsAsync(adminRole))
            {
                await roleManager.CreateAsync(new IdentityRole(adminRole));
            }

            // Ensure admin user exists
            var adminEmail = "admin@gadgetstore.com";
            var adminPassword = "Admin@123";
            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var adminUser = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, adminRole);
                }
            }
        }
    }
}
