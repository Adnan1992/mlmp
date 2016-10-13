using LearningManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PagedList;

namespace LearningManagementSystem.Interfacelasses
{
    interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        IPagedList<Batch> GetAllCourses(int? page);
        Task<T> GetById(object Id);
        void Insert(T obj);
        void Update(T obj);
        Task Delete(Object Id);
        void Save();
    }
}