using HumanResouresMangementSystem.Core.Entites;
using HumanResouresMangementSystem.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace HumanResouresMangementSystem.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<HrGroup> Groups { get; set; }
        public DbSet<GroupPermission> GroupPermissions { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<WorkData> WorkData { get; set; }
        public DbSet<OfficialHoliday> OfficialHolidays { get; set; }

        public DbSet<DeductionSetting> DeductionSettings { get; set; }

        public DbSet<EmployeeDeduction> EmployeeDeductions { get; set; }

        public DbSet<OvertimeSetting> OvertimeSettings { get; set; }

        public DbSet<EmployeeOvertime> EmployeeOvertimes { get; set; }
        public DbSet<GeneralSettings> GeneralSettings { get; set; }
        public DbSet<Department>Departments { get; set; }
        public DbSet<Attendance> Attendances { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
           

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
