using MegoTimeTracker.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MegoTimeTracker.Data {
    public class StudentDb : IdentityDbContext {
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<Attendance> Attendances { get; set; }
        public StudentDb(DbContextOptions<StudentDb> options) : base(options) { }
    }
}