using HumanResouresMangementSystem.Core.Authorization;
using HumanResouresMangementSystem.Core.Entites;
using HumanResouresMangementSystem.Core.Models;
using HumanResouresMangementSystem.DAL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HumanResouresMangementSystem.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly PasswordHasher<User> _passwordHasher;

        public UsersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = new PasswordHasher<User>();
        }

        private async Task PopulateGroupsDropDownListAsync(object selectedGroup = null)
        {
            var groups = await _unitOfWork.Groups.GetAllAsync();
            ViewBag.Groups = new SelectList(groups ?? new List<HrGroup>(), "Id", "Name", selectedGroup);
        }

        [HasPermission("Users.View")]
        public async Task<IActionResult> Index()
        {
            var users = await _unitOfWork.Users.GetQueryable()
                .Include(x => x.Group)
                .AsNoTracking()
                .ToListAsync();

            return View(users);
        }

        [HasPermission("Users.Create")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await PopulateGroupsDropDownListAsync();
            return View();
        }

        [HasPermission("Users.Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(User user)
        {
            ModelState.Remove("PasswordHash");
            ModelState.Remove("Group");

            if (!ModelState.IsValid)
            {
                await PopulateGroupsDropDownListAsync(user.GroupId);
                return View(user);
            }

            var usernameExists = await _unitOfWork.Users.GetQueryable()
                .AnyAsync(x => x.UserName == user.UserName);

            if (usernameExists)
            {
                ModelState.AddModelError("UserName", "Username already exists");
                await PopulateGroupsDropDownListAsync(user.GroupId);
                return View(user);
            }

            var emailExists = await _unitOfWork.Users.GetQueryable()
                .AnyAsync(x => x.Email == user.Email);

            if (emailExists)
            {
                ModelState.AddModelError("Email", "Email already exists");
                await PopulateGroupsDropDownListAsync(user.GroupId);
                return View(user);
            }

            // ✅ التعديل هنا: تشفير كلمة المرور المدخلة من الفيو (Form)
            user.PasswordHash = _passwordHasher.HashPassword(user, user.Password);

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.CompleteAsync();

            if (user.Id == 0)
            {
                ModelState.AddModelError("", "User was not saved to database.");
                await PopulateGroupsDropDownListAsync(user.GroupId);
                return View(user);
            }

            return RedirectToAction(nameof(Index));
        }

        
        [HttpGet]
        [HasPermission("Users.Edit")]
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);

            if (user == null)
                return NotFound();

            await PopulateGroupsDropDownListAsync(user.GroupId);
            return View(user);
        }

        [HasPermission("Users.Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(User user)
        {
            ModelState.Remove("PasswordHash");
            ModelState.Remove("Group");

            if (!ModelState.IsValid)
            {
                await PopulateGroupsDropDownListAsync(user.GroupId);
                return View(user);
            }

            var existingUser = await _unitOfWork.Users.GetByIdAsync(user.Id);

            if (existingUser == null)
                return NotFound();

            var usernameExists = await _unitOfWork.Users.GetQueryable()
                .AnyAsync(x => x.Id != user.Id && x.UserName == user.UserName);

            if (usernameExists)
            {
                ModelState.AddModelError("UserName", "اسم المستخدم موجود بالفعل");
                await PopulateGroupsDropDownListAsync(user.GroupId);
                return View(user);
            }

            var emailExists = await _unitOfWork.Users.GetQueryable()
                .AnyAsync(x => x.Id != user.Id && x.Email == user.Email);

            if (emailExists)
            {
                ModelState.AddModelError("Email", "البريد الإلكتروني موجود بالفعل");
                await PopulateGroupsDropDownListAsync(user.GroupId);
                return View(user);
            }

            existingUser.FullName = user.FullName;
            existingUser.UserName = user.UserName;
            existingUser.Email = user.Email;
            existingUser.GroupId = user.GroupId;

            _unitOfWork.Users.Update(existingUser);
            await _unitOfWork.CompleteAsync();

            return RedirectToAction("Index", "Users");
        }

        [HasPermission("Users.Delete")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);

            if (user == null)
                return NotFound();

            _unitOfWork.Users.Delete(user);
            await _unitOfWork.CompleteAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}