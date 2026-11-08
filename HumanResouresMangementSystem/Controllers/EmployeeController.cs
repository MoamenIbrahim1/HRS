using HumanResouresMangementSystem.Core.Authorization;
using HumanResouresMangementSystem.Core.Dto;
using HumanResouresMangementSystem.Core.Entites;
using HumanResouresMangementSystem.Core.Interfaces;
using HumanResouresMangementSystem.Core.Models;
using HumanResouresMangementSystem.Core.ViewModels;
using HumanResouresMangementSystem.DAL.Interfaces;
using HumanResouresMangementSystem.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;


namespace HumanResouresMangementSystem.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        [HttpGet]

        [HasPermission("Employees.Index")]
        public async Task<IActionResult> Index()
        {
            var employees = await _unitOfWork.Employees.GetQueryable()
                .Include(w => w.WorkData)
                .AsNoTracking()
                .ToListAsync();

            return View(employees);
        }

        [HasPermission("Employees.Create")]
        [HttpGet]
        public async Task<IActionResult> Create(int? id)
        {
            ViewBag.Employees = await _unitOfWork.Employees.GetQueryable()
                .Include(w => w.WorkData)
                .AsNoTracking()
                .ToListAsync();

            var viewModel = new EmployeeViewModel();

            // في حالة التعديل: جلب بيانات الموظف وتعبئتها في الـ ViewModel
            if (id.HasValue && id > 0)
            {
                var employeeList = await _unitOfWork.Employees.FindAsync(
                    e => e.Id == id.Value,
                    e => e.WorkData
                );

                var employee = employeeList.FirstOrDefault();

                if (employee != null)
                {
                    viewModel.Id = employee.Id;
                    viewModel.Name = employee.Name;
                    viewModel.Address = employee.Address;
                    viewModel.Phone = employee.PhoneNumber;
                    viewModel.Gender = employee.Gender;
                    viewModel.Nationality = employee.Nationality;
                    viewModel.BirthDate = employee.BirthDate;
                    viewModel.NationalId = employee.NationalId;

                    if (employee.WorkData != null)
                    {
                        viewModel.ContractDate = employee.WorkData.ContractDate;
                        viewModel.Salary = employee.WorkData.Salary;
                        viewModel.CheckInTime = employee.WorkData.CheckInTime;
                        viewModel.CheckOutTime = employee.WorkData.CheckOutTime;
                    }
                }
            }

            return View(viewModel);
        }

        [HasPermission("Employees.Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeViewModel model)
        {
            ModelState.Remove("WorkData");

            if (!string.IsNullOrEmpty(model.Phone))
            {
                if (model.Phone.Length != 11 || !Regex.IsMatch(model.Phone, @"^\d{11}$"))
                {
                    ModelState.AddModelError("Phone", "من فضلك ادخل رقم تليفون صحيح (يحتوي على 11 رقم)");
                }
            }

            if (!string.IsNullOrEmpty(model.NationalId))
            {
                if (model.NationalId.Length != 14 || !Regex.IsMatch(model.NationalId, @"^\d{14}$"))
                {
                    ModelState.AddModelError("NationalId", "من فضلك أدخل رقم قومي صحيح (يحتوي على 14 رقم)");
                }
            }

            if (model.BirthDate > DateTime.Today.AddYears(-20))
            {
                ModelState.AddModelError("BirthDate", "عفواً، يجب أن يكون عمر الموظف 20 سنة على الأقل");
            }

            if (model.ContractDate.Year < 2008)
            {
                ModelState.AddModelError("ContractDate", "من فضلك ادخل تاريخ تعاقد صحيح (لا يمكن أن يكون قبل سنة 2008)");
            }

            if (model.Salary <= 0)
            {
                ModelState.AddModelError("Salary", "من فضلك ادخل راتب صحيح");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Employees = await _unitOfWork.Employees.GetQueryable()
                    .Include(w => w.WorkData)
                    .AsNoTracking()
                    .ToListAsync();

                return View(model);
            }

            // تعديل بيانات موظف قائم
            if (model.Id > 0)
            {
                var employeeList = await _unitOfWork.Employees.FindAsync(
                    e => e.Id == model.Id,
                    e => e.WorkData
                );

                var existingEmployee = employeeList.FirstOrDefault();

                if (existingEmployee != null)
                {
                    existingEmployee.Name = model.Name;
                    existingEmployee.Address = model.Address;
                    existingEmployee.PhoneNumber = model.Phone;
                    existingEmployee.Gender = model.Gender;
                    existingEmployee.Nationality = model.Nationality;
                    existingEmployee.BirthDate = model.BirthDate;
                    existingEmployee.NationalId = model.NationalId;

                    if (existingEmployee.WorkData != null)
                    {
                        existingEmployee.WorkData.ContractDate = model.ContractDate;
                        existingEmployee.WorkData.Salary = model.Salary;
                        existingEmployee.WorkData.CheckInTime = model.CheckInTime;
                        existingEmployee.WorkData.CheckOutTime = model.CheckOutTime;
                    }

                    _unitOfWork.Employees.Update(existingEmployee);
                    TempData["SuccessMessage"] = "تم تعديل بيانات الموظف بنجاح";
                }
            }
            
            else
            {
                var employee = new Employee
                {
                    Name = model.Name,
                    Address = model.Address,
                    PhoneNumber = model.Phone,
                    Gender = model.Gender,
                    Nationality = model.Nationality,
                    BirthDate = model.BirthDate,
                    NationalId = model.NationalId,
                    WorkData = new WorkData
                    {
                        ContractDate = model.ContractDate,
                        Salary = model.Salary,
                        CheckInTime = model.CheckInTime,
                        CheckOutTime = model.CheckOutTime
                    }
                };

                await _unitOfWork.Employees.AddAsync(employee);
                TempData["SuccessMessage"] = "تم إضافة الموظف بنجاح";
            }

            await _unitOfWork.CompleteAsync();
            return RedirectToAction(nameof(Create));
        }

        [HasPermission("Employees.Delete")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var employeeList = await _unitOfWork.Employees.FindAsync(
                e => e.Id == id,
                e => e.WorkData
            );

            var employee = employeeList.FirstOrDefault();

            if (employee != null)
            {
                if (employee.WorkData != null)
                {
                    _unitOfWork.WorkData.Delete(employee.WorkData);
                }

                _unitOfWork.Employees.Delete(employee);
                await _unitOfWork.CompleteAsync();
                TempData["SuccessMessage"] = "تم حذف الموظف بنجاح";
            }

            return RedirectToAction(nameof(Create));
        }
    }
}