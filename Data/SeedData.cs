using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MegoTimeTracker.Data {
    public static class SeedData {
        public static async Task InitializeAsync(IServiceProvider serviceProvider) {
            using var context = new StudentDb(serviceProvider.GetRequiredService<DbContextOptions<StudentDb>>());
            UserManager<IdentityUser> users = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            RoleManager<IdentityRole> roles = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            if (!users.Users.Any()) {
                IdentityUser user = new() { UserName = "Admin" };
                string admin = "admin";
                IdentityRole role = new() { Name = admin };
                await roles.CreateAsync(role);

                var result = await users.CreateAsync(user);
                if (!result.Succeeded) {
                    throw new ApplicationException("Failed to create role");
                }

                await users.AddPasswordAsync(user, "123qwe!@#QWE");
                await users.AddToRoleAsync(user, admin);
                await context.SaveChangesAsync();
            }
        }
    }
}