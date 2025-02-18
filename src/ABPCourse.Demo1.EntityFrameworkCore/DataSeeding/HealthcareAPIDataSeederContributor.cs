using ABPCourse.Demo1.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;

namespace ABPCourse.Demo1.DataSeeding
{
    public class HealthcareAPIDataSeederContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IIdentityRoleRepository _roleRepository;
        private readonly IdentityRoleManager _roleManager;
        private readonly IdentityUserManager _userManager;
        public HealthcareAPIDataSeederContributor(
            IIdentityRoleRepository roleRepository,
            IdentityRoleManager roleManager,
            IdentityUserManager userManager)
        {
            _roleRepository = roleRepository;
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public async Task SeedAsync(DataSeedContext context)
        {
            await CreateRoleIfNotExistsAsync(UserRole.Admin);
            await CreateRoleIfNotExistsAsync(UserRole.Doctor);
            await CreateRoleIfNotExistsAsync(UserRole.Nurse);
            await CreateRoleIfNotExistsAsync(UserRole.Receptionist);
            await CreateRoleIfNotExistsAsync(UserRole.Patient);

            // Add default admin user
            var adminUser = await _userManager.FindByEmailAsync("admin@healthcare.com");
            if (adminUser == null)
            {
                adminUser = new IdentityUser(Guid.NewGuid(), "admin@healthcare.com", "admin@healthcare.com");
                await _userManager.CreateAsync(adminUser, "Admin123!");
                await _userManager.AddToRoleAsync(adminUser, UserRole.Admin);
            }
        }

        private async Task CreateRoleIfNotExistsAsync(string roleName)
        {
            if (await _roleRepository.FindByNormalizedNameAsync(roleName) == null)
            {
                await _roleManager.CreateAsync(new IdentityRole(Guid.NewGuid(), roleName));
            }
        }
    }
}
