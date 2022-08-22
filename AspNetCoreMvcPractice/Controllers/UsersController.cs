using AspNetCoreMvcPractice.Business.Services;
using AspNetCoreMvcPractice.Data.Models;
using AspNetCoreMvcPractice.ViewModels.Users;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreMvcPractice.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailService _emailService;

        public UsersController(
            UserManager<User> userManager, 
            SignInManager<User> signInManager,
            IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User() 
                { 
                    Email = model.Email, 
                    UserName = model.Email,
                    CreatedAt = DateTimeOffset.UtcNow
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        [HttpGet]
        [Route("[controller]")]
        [Route("[controller]/[action]")]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
                if (result.Succeeded)
                    return RedirectToAction("Index", "Home");

                ModelState.AddModelError("", "Invalid username and/or password");
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                    return View("ForgotPasswordConfirm");

                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var url = Url.Action("ResetPassword", "Users", 
                    new { UserId = user.Id, Code = code }, 
                    HttpContext.Request.Scheme);

                await _emailService.SendEmailAsync(model.Email, "Reset Password",
                    $"To reset your password, follow the link: <a href='{url}'>link</a>");

                return View("ForgotPasswordConfirm");
            }
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string code = null)
        {
            if (code == null)
                RedirectToAction("Error", "Home");

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return View("ResetPasswordConfirm");

            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
                return View("ResetPasswordConfirm");

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return View(model);
        }

        [HttpGet]
        public IActionResult ExternalLogin()
        {
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback));
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(OpenIdConnectDefaults.AuthenticationScheme, redirectUrl);
            return new ChallengeResult(OpenIdConnectDefaults.AuthenticationScheme, properties);
        }

        [HttpGet]
        public async Task<IActionResult> ExternalLoginCallback()
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false, false);

            if (result.Succeeded)
                return RedirectToAction("Index", "Home");

            if (info.Principal.Identity != null && !string.IsNullOrEmpty(info.Principal.Identity.Name))
            {
                var name = info.Principal.Identity.Name;
                var user = new User()
                {
                    UserName = name,
                    Email = name,
                    CreatedAt = DateTimeOffset.UtcNow
                };
                var createResult = await _userManager.CreateAsync(user);
                var loginResult = await _userManager.AddLoginAsync(user, info);

                if (createResult.Succeeded && loginResult.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }
            }

            return RedirectToAction("Login");
        }
    }
}
