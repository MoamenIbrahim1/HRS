using HumanResouresMangementSystem.Core.Entites;
using HumanResouresMangementSystem.Core.Interfaces;
using HumanResouresMangementSystem.Core.Models;


namespace HumanResouresMangementSystem.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Attendance> Attendances { get; }
        IGenericRepository<DeductionSetting> DeductionSettings { get; }
        IGenericRepository<Department> Departments { get; }
        IGenericRepository<Employee> Employees { get; }
        IGenericRepository<EmployeeDeduction> EmployeeDeductions { get; }
        IGenericRepository<EmployeeOvertime> EmployeeOvertimes { get; }
        IGenericRepository<GeneralSettings> GeneralSettings { get; }
        IGenericRepository<GroupPermission> GroupPermissions { get; }
        IGenericRepository<HrGroup> Groups { get; }
        IGenericRepository<OfficialHoliday> OfficialHolidays { get; }
        IGenericRepository<OvertimeSetting> OvertimeSettings { get; }
        IGenericRepository<Permission> Permissions { get; }
        IGenericRepository<User> Users { get; }
        IGenericRepository<WorkData> WorkData { get; }

        Task<int> CompleteAsync();
    }
}