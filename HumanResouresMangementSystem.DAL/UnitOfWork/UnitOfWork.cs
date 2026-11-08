
namespace HumanResouresMangementSystem.DAL.UnitOfWork
{
    using global::HumanResouresMangementSystem.Core.Entites;
    using global::HumanResouresMangementSystem.Core.Interfaces;
    using global::HumanResouresMangementSystem.Core.Models;
    using global::HumanResouresMangementSystem.DAL.Interfaces;
    using global::HumanResouresMangementSystem.DAL.Repository;
    using global::HumanResouresMangementSystem.Data;
    namespace HumanResouresMangementSystem.DAL.Repository
    {
        public class UnitOfWork : IUnitOfWork
        {
            private readonly AppDbContext _context;

            public IGenericRepository<Attendance> Attendances { get; private set; }
            public IGenericRepository<DeductionSetting> DeductionSettings { get; private set; }
            public IGenericRepository<Department> Departments { get; private set; }
            public IGenericRepository<Employee> Employees { get; private set; }
            public IGenericRepository<EmployeeDeduction> EmployeeDeductions { get; private set; }
            public IGenericRepository<EmployeeOvertime> EmployeeOvertimes { get; private set; }
            public IGenericRepository<GeneralSettings> GeneralSettings { get; private set; }
            public IGenericRepository<GroupPermission> GroupPermissions { get; private set; }
            public IGenericRepository<HrGroup> Groups { get; private set; }
            public IGenericRepository<OfficialHoliday> OfficialHolidays { get; private set; }
            public IGenericRepository<OvertimeSetting> OvertimeSettings { get; private set; }
            public IGenericRepository<Permission> Permissions { get; private set; }
            public IGenericRepository<User> Users { get; private set; }
            public IGenericRepository<WorkData> WorkData { get; private set; }

            public UnitOfWork(AppDbContext context)
            {
                _context = context;

                Attendances = new GenericRepository<Attendance>(_context);
                DeductionSettings = new GenericRepository<DeductionSetting>(_context);
                Departments = new GenericRepository<Department>(_context);
                Employees = new GenericRepository<Employee>(_context);
                EmployeeDeductions = new GenericRepository<EmployeeDeduction>(_context);
                EmployeeOvertimes = new GenericRepository<EmployeeOvertime>(_context);
                GeneralSettings = new GenericRepository<GeneralSettings>(_context);
                GroupPermissions = new GenericRepository<GroupPermission>(_context);
                Groups = new GenericRepository<HrGroup>(_context);
                OfficialHolidays = new GenericRepository<OfficialHoliday>(_context);
                OvertimeSettings = new GenericRepository<OvertimeSetting>(_context);
                Permissions = new GenericRepository<Permission>(_context);
                Users = new GenericRepository<User>(_context);
                WorkData = new GenericRepository<WorkData>(_context);
            }

            public async Task<int> CompleteAsync()
            {
                return await _context.SaveChangesAsync();
            }

            public void Dispose()
            {
                _context.Dispose();
            }
        }
    }
}