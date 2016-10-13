using LearningManagementSystem.Interfacelasses;
using LearningManagementSystem.Models;
using LearningManagementSystem.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace LearningManagementSystem.Data
{
    class UserManagementRepository : IUserManagementRepository
    {
        private LMSContext db = new LMSContext();
        public UserManager<ApplicationUser> UserManager { get; set; }
        public RoleManager<IdentityRole> RoleManager { get; set; }


        public UserManagementRepository():this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new LMSContext())),
            new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new LMSContext())))
        { }

        public UserManagementRepository(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }

        //public UserManagementRepository(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
        //{
        //    UserManager = userManager;
        //    RoleManager = roleManager;
        //}

        public async Task<IEnumerable<ApplicationUser>> GetAllInstructor()
        {
            return await db.Users.ToListAsync();
        }
        public async Task AddNewInstructor(EmployeeViewModel model,string Role,string Password,string UserID)
        {
            
            var find = await UserManager.FindByEmailAsync(model.Email);
            if (find == null)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.ContactNumber
                };
                model.USERID = user.Id;
                var result =  await UserManager.CreateAsync(user, Password);
                if (result.Succeeded)
                {
                    var role = await UserManager.AddToRoleAsync(user.Id, Role);
                    var instructor = new Instructor();
                    instructor.FirstName = user.FirstName;
                    instructor.LastName = user.LastName;
                    instructor.Gender = model.Gender;
                    instructor.UserId = user.Id;
                    instructor.PhoneNumber = user.PhoneNumber;

                    db.Instrouctor.Add(instructor);
                    db.SaveChanges();
                }
            }
        }

        public async Task DeleteInstructor(string model)
        {
            var find = await UserManager.FindByIdAsync(model);
            if (find != null)
            {
                var logins = find.Logins;
                foreach (var login in logins.ToList())
                {
                    await UserManager.RemoveLoginAsync(login.UserId, new UserLoginInfo(login.LoginProvider, login.ProviderKey));
                }
                var role = await UserManager.GetRolesAsync(find.Id);
                if (role.Count() > 0)
                {
                    foreach (var item in role.ToList())
                    {
                        var getrole = await UserManager.RemoveFromRolesAsync(find.Id, item);
                    }
                }
                await UserManager.DeleteAsync(find);
            }
        }
        public async Task FindUserByEmail(EmployeeViewModel model)
        {
            var find = await UserManager.FindByEmailAsync(model.Email);
        }
        public async Task FindUserById(EmployeeViewModel model)
        {
            var find = await UserManager.FindByIdAsync(model.InstructorId);
        }


        private bool _disposed = false;
        public void Dispose()
        {
            if (!_disposed)
            {
                db.Dispose();
            }
            _disposed = true;
        }

    }
}
