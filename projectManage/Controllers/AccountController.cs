using projectManage.Data;
using projectManage.Models;
using projectManage.Services;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;

namespace projectManage.Controllers
{
    public class AccountController : Controller
    {
        private AppDbContext db = new AppDbContext();


        public static string passwordGenerator(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private string GenerateToken()
        {
            using (var cryptoProvider = new RNGCryptoServiceProvider())
            {
                byte[] tokenBuffer = new byte[50];
                cryptoProvider.GetBytes(tokenBuffer);
                return Convert.ToBase64String(tokenBuffer);
            }
        }

        public ActionResult ConfirmEmail(string token)
        {
            var user = db.Users.FirstOrDefault(u => u.ConfirmationToken == token);
            if (user != null)
            {
                user.IsEmailConfirmed = true;
                user.ConfirmationToken = null;
                db.SaveChanges();
                TempData["SuccessMessage"] = "E-mail confirmado com sucesso!";
                return RedirectToAction("Login");
            }
            TempData["ErrorMessage"] = "Token de confirmação inválido.";
            return RedirectToAction("Register");
        }

        // GET: Account
        public ActionResult Register()
        {
            {
                ViewBag.Profiles = new SelectList(new[] { "Gerente", "Membro"});
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User model)
        {
            if (ModelState.IsValid)
            {
                model.senha = passwordGenerator(model.senha);
                model.ConfirmationToken = GenerateToken();
                model.IsEmailConfirmed = false;
                db.Users.Add(model);
                db.SaveChanges();
                TempData["SuccessMessage"] = "Usuário criado com sucesso! Verifique seu e-mail para confirmar a conta.";
                SendConfirmationEmail(model);
                return RedirectToAction("Login");
            }
            TempData["ErrorMessage"] = "Ocorreu um erro. Tente novamente.";
            ViewBag.Profiles = new SelectList(new[] { "Gerente", "Membro" });
            return View(model);
        }

        public ActionResult Login()
        {
            {
                return View();
            }
        }

        public void SendConfirmationEmail(User user)
        {
            var confirmationLink = Url.Action("ConfirmEmail", "Account", new { token = user.ConfirmationToken }, protocol: Request.Url.Scheme);
            string subject = "Confirmação de E-mail";
            string body = $"Clique no link para confirmar sua conta: \"{confirmationLink}\"";

            // Enviar e-mail (use sua própria implementação ou um serviço de e-mail)
            var emailService = new EmailService("smtp.mailtrap.io", 2525, "b7c5b41b79785b", "b2754a6ec07cf5");
            emailService.SendEmail(user.email, subject, body);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User model)
        {
            if (ModelState.IsValid)
            {
                string hashedPassword = passwordGenerator(model.senha);
                var user = db.Users.FirstOrDefault(u => u.email == model.email && u.senha == hashedPassword);
                if (user != null)
                {
                    if (!user.IsEmailConfirmed)
                    {
                        ModelState.AddModelError("", "E-mail não confirmado. Verifique seu e-mail.");
                        return View(model);
                    }
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Nome de usuário ou senha inválidos.");
                }
            }
            return View(model);
        }
    }
}