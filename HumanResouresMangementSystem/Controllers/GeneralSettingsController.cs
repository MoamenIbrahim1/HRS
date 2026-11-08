using HumanResouresMangementSystem.Core.Authorization;
using HumanResouresMangementSystem.Core.Dto;
using HumanResouresMangementSystem.Core.Entites; // أو حسب المسار لديك
using HumanResouresMangementSystem.Core.Interfaces;
using HumanResouresMangementSystem.Core.Models;
using HumanResouresMangementSystem.Core.ViewModels;
using HumanResouresMangementSystem.DAL.Interfaces;
using HumanResouresMangementSystem.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace HumanResouresMangementSystem.Web.Controllers
{
    [Authorize]

    public class GeneralSettingsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public GeneralSettingsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HasPermission("GeneralSettings.Create")]
        [HttpGet]

        public async Task<IActionResult> Index()
        {
            
            var settings = await _unitOfWork.GeneralSettings.GetQueryable().FirstOrDefaultAsync();

            if (settings == null)
            {
                settings = new GeneralSettings
                {
                    ExtraHoursRate = 2,
                    DeductionHoursRate = 2,
                    FirstWeekendDay = DayOfWeek.Friday,
                    SecondWeekendDay = DayOfWeek.Saturday
                };

                await _unitOfWork.GeneralSettings.AddAsync(settings);
                await _unitOfWork.CompleteAsync();
            }

            var model = new GeneralSettingsViewModel
            {
                Id = settings.Id,
                ExtraHoursRate = settings.ExtraHoursRate,
                DeductionHoursRate = settings.DeductionHoursRate,
                FirstWeekendDay = settings.FirstWeekendDay,
                SecondWeekendDay = settings.SecondWeekendDay
            };

            return View(model);
        }

        [HasPermission("GeneralSettings.Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(GeneralSettingsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "من فضلك ادخال جميع البيانات المطلوبة بشكل صحيح." });
            }

            try
            {
                var settingsList = await _unitOfWork.GeneralSettings.FindAsync(s => s.Id == model.Id);
                var settings = settingsList.FirstOrDefault();

                if (settings == null)
                {
                    // في حالة عدم وجود السجل يتم إضافته
                    settings = new GeneralSettings();

                    settings.ExtraHoursRate = model.ExtraHoursRate;
                    settings.DeductionHoursRate = model.DeductionHoursRate;
                    settings.FirstWeekendDay = model.FirstWeekendDay;
                    settings.SecondWeekendDay = model.SecondWeekendDay;

                    await _unitOfWork.GeneralSettings.AddAsync(settings);
                }
                else
                {
                    // تحديث القيم
                    settings.ExtraHoursRate = model.ExtraHoursRate;
                    settings.DeductionHoursRate = model.DeductionHoursRate;
                    settings.FirstWeekendDay = model.FirstWeekendDay;
                    settings.SecondWeekendDay = model.SecondWeekendDay;

                    _unitOfWork.GeneralSettings.Update(settings);
                }

                await _unitOfWork.CompleteAsync(); // حفظ التغييرات في DB

                return Json(new { success = true, message = "تم حفظ وتحديث البيانات بنجاح" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "حدث خطأ أثناء الحفظ: " + ex.Message });
            }
        }
    }
}