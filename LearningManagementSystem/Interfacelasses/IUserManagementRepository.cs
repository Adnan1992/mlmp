using LearningManagementSystem.Models;
using LearningManagementSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Interfacelasses
{
    interface IUserManagementRepository:IDisposable
    {
        Task AddNewInstructor(EmployeeViewModel model,string role,string Password, string USERID);
        Task<IEnumerable<ApplicationUser>> GetAllInstructor();
        Task FindUserByEmail(EmployeeViewModel model);
        Task FindUserById(EmployeeViewModel model);
        Task DeleteInstructor(string model);
    }
}
