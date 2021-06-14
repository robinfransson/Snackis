using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Win32.SafeHandles;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace SnackisForum.Injects
{
    public class SetupDb : IDisposable
    {
        private readonly SnackisDB.Models.SnackisContext _context;
        private readonly ILogger<SetupDb> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;


        bool disposed = false;
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);

        public SetupDb(ILogger<SetupDb> logger, SnackisDB.Models.SnackisContext context, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _logger = logger;
            _roleManager = roleManager;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                handle.Dispose();
                // Free any other managed objects here.
                //
            }

            disposed = true;
        }

        public void Setup()
        {
            bool canConnect = _context.Database.CanConnect();
            _logger.LogInformation($"Checking database... can connect: {canConnect} ");
            bool dbCreatedNow = _context.Database.EnsureCreated();
            _logger.LogInformation($"Checking database... database exists: {!dbCreatedNow}");
            
            if (!dbCreatedNow)
            {

                _logger.LogInformation("Db already created, checking roles..");

                CheckIfRolesExists().Wait();
            }
            else
            {
                try
                {

                        CheckIfRolesExists().Wait();
                        _logger.LogInformation("Done setting up the database!");
                }
                catch (Exception e)
                {
                    _logger.LogError(e.ToString());
                }
                return;
            }
        }

        #region Create roles

        private async Task CheckIfRolesExists()
        {
            bool exists = await _roleManager.RoleExistsAsync("Admin");
            if (!exists)
            {
                _logger.LogInformation("Adding roles...");
                IdentityRole[] roles = { new IdentityRole
            {
                Name = "Admin"
            }, new IdentityRole
            {
                Name = "User"
            }
            };
                for (int i = 0; i < 2; i++)
                {
                    var role = roles[i];
                    _logger.LogInformation("Created role " + role.Name);
                    _roleManager.CreateAsync(role).Wait();//funkade inte med en foreach av någon anledning, eller await, då var role manager disposed

                }

            }
            else
            {
                _logger.LogInformation("The roles already exist!");
            }
            return;
        }

        #endregion
    }
}