using HumanResouresMangementSystem.Core.Dto;
using HumanResouresMangementSystem.Core.Entites;
using HumanResouresMangementSystem.DAL.Interfaces;
using HumanResouresMangementSystem.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


namespace HumanResouresMangementSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly PasswordHasher<User> _passwordHasher;

        public AccountController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = new PasswordHasher<User>();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // 1. جلب المستخدم مع الجروب وجدول الربط GroupPermissions والـ Permission نفسه
            var user = await _unitOfWork.Users.GetQueryable()
                .Include(x => x.Group)
                    .ThenInclude(g => g.GroupPermissions)
                        .ThenInclude(gp => gp.Permission) // تضمين الـ Permission لقراءة ControllerName و ActionName
                .AsNoTracking()
                .FirstOrDefaultAsync(x =>
                    x.UserName == model.UserNameOrEmail ||
                    x.Email == model.UserNameOrEmail);

            if (user == null)
            {
                ModelState.AddModelError(
                    "UserNameOrEmail",
                    "من فضلك ادخل اسم مستخدم صالح");

                return View(model);
            }

            // بدلاً من: _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, model.Password);
            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, model.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError(
                    "Password",
                    "من فضلك ادخل كلمة مرور صالحة");

                return View(model);
            }

            // 2. إنشاء الـ Claims الأساسية
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty)
            };

            // 3. إضافة الـ Role والصلاحيات المرتبطة بـ Controller & Action
            if (user.Group != null)
            {
                claims.Add(new Claim(ClaimTypes.Role, user.Group.Name));

                if (user.Group.GroupPermissions != null)
                {
                    foreach (var gp in user.Group.GroupPermissions)
                    {
                        if (gp.Permission != null)
                        {
                            // تخزين الصلاحية بصيغة Controller.Action (مثال: Users.Create أو Users.Index)
                            var permissionClaim = $"{gp.Permission.ControllerName}.{gp.Permission.ActionName}";
                            claims.Add(new Claim("Permission", permissionClaim));
                        }
                    }
                }
            }

            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme
            );

            var principal = new ClaimsPrincipal(identity);

            // 4. تسجبل الدخول في الـ Cookie
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal
            );

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme
            );

            return RedirectToAction("Login", "Account");
        }
    }
}