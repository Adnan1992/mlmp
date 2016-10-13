using LearningManagementSystem.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace LearningManagementSystem.Data
{
    public class LMSContext : IdentityDbContext<ApplicationUser>
    {
        public LMSContext() : base("name=LMSContext") { }
        public DbSet<Course> courses { get; set; }
        public DbSet<Instructor> Instrouctor { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Assignmet> Assignemt { get; set; }
        public DbSet<Enrollment> tempenrollments { get; set; }
        public DbSet<Batch> batch { get; set; }
        public DbSet<Attendance> attendance { get; set; }
        public DbSet<Payment> Payment { get; set; }
        public DbSet<Comment> Comment { get; set; }
        public DbSet<StudentFee> Fee { get; set; }
        public DbSet<Gallery> Gallery { get; set; }
        public DbSet<Lectures> Lecture { get; set; }
        public static LMSContext Create()
        {
            return new LMSContext();
        }
    }
}