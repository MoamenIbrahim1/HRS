using HumanResouresMangementSystem.Core.Authorization;
using HumanResouresMangementSystem.Core.Dto;
using HumanResouresMangementSystem.Core.Models;
using HumanResouresMangementSystem.Core.ViewModels;
using HumanResouresMangementSystem.DAL.Interfaces;
using HumanResouresMangementSystem.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HumanResouresMangementSystem.Controllers
{
    [Authorize]
    public class OfficialHolidaysController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public OfficialHolidaysController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [HasPermission("OfficialHolidays.Create")]

        public async Task<IActionResult> Index(int? id)
        {
            var holidays = await _unitOfWork.OfficialHolidays.GetQueryable()
                .AsNoTracking()
                .OrderByDescending(h => h.Date)
                .Select(h => new OfficialHolidayViewModel
                {
                    Id = h.Id,
                    Name = h.Name,
                    HolidayDate = h.Date
                })
                .ToListAsync();

            var model = new OfficialHolidayViewModel
            {
                ExistingHolidays = holidays
            };

            // في حالة الضغط على تعديل لجلب البيانات في الفورم
            if (id.HasValue && id > 0)
            {
                var holiday = await _unitOfWork.OfficialHolidays.GetByIdAsync(id.Value);
                if (holiday != null)
                {
                    model.Id = holiday.Id;
                    model.Name = holiday.Name;
                    model.HolidayDate = holiday.Date;
                }
            }

            return View(model);
        }
        [HasPermission("OfficialHolidays.Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OfficialHolidayViewModel model)
        {
            if (ModelState.IsValid)
            {
                // عملية التعديل (Edit)
                if (model.Id > 0)
                {
                    var existingHoliday = await _unitOfWork.OfficialHolidays.GetByIdAsync(model.Id);
                    if (existingHoliday != null)
                    {
                        existingHoliday.Name = model.Name;
                        existingHoliday.Date = model.HolidayDate;

                        _unitOfWork.OfficialHolidays.Update(existingHoliday);
                        TempData["SuccessMessage"] = "تم تعديل الإجازة بنجاح";
                    }
                }
                // عملية الإضافة (Create)
                else
                {
                    var holiday = new OfficialHoliday
                    {
                        Name = model.Name,
                        Date = model.HolidayDate
                    };

                    await _unitOfWork.OfficialHolidays.AddAsync(holiday);
                    TempData["SuccessMessage"] = "تم إضافة الإجازة بنجاح";
                }

                await _unitOfWork.CompleteAsync();
                return RedirectToAction(nameof(Index));
            }

            // إرجاع القائمة في حالة وجود أخطاء في الـ ModelState
            model.ExistingHolidays = await _unitOfWork.OfficialHolidays.GetQueryable()
                .AsNoTracking()
                .OrderByDescending(h => h.Date)
                .Select(h => new OfficialHolidayViewModel
                {
                    Id = h.Id,
                    Name = h.Name,
                    HolidayDate = h.Date
                })
                .ToListAsync();

            return View("Index", model);
        }
        [HasPermission("OfficialHolidays.Delete")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var holiday = await _unitOfWork.OfficialHolidays.GetByIdAsync(id);
            if (holiday != null)
            {
                _unitOfWork.OfficialHolidays.Delete(holiday);
                await _unitOfWork.CompleteAsync();
                TempData["SuccessMessage"] = "تم حذف الإجازة بنجاح";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}