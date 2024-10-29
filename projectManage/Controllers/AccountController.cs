using projectManage.Data;
using projectManage.Models;
using projectManage.Services;
using System;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace projectManage.Controllers
{
    public class AccountController : Controller
    {
        private AddDbContext db = new AddDbContext();


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
            var user = db.Usuario.FirstOrDefault(u => u.ConfirmationToken == token);
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
        [HttpGet]
        public ActionResult Register()
        {
            {
                ViewBag.Profiles = new SelectList(new[] { "Gerente", "Membro" });
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Usuario model)
        {
            var user = db.Usuario.FirstOrDefault(u => u.email == model.email);
            if (user != null)
            {
                TempData["ErrorMessage"] = "Email já em uso.";
                ViewBag.Profiles = new SelectList(new[] { "Gerente", "Membro" });
                return View(model);
            }
            if (ModelState.IsValid)
            {
                model.senha = passwordGenerator(model.senha);
                model.ConfirmationToken = GenerateToken();
                model.IsEmailConfirmed = false;
                db.Usuario.Add(model);
                db.SaveChanges();
                TempData["SuccessMessage"] = "Usuário criado com sucesso! Verifique seu e-mail para confirmar a conta.";
                SendConfirmationEmail(model);
                return RedirectToAction("Login");
            }
            TempData["ErrorMessage"] = "Ocorreu um erro. Tente novamente.";
            ViewBag.Profiles = new SelectList(new[] { "Gerente", "Membro" });
            return View(model);
        }

        [HttpGet]
        public ActionResult Login()
        {
            {
                return View();
            }
        }

        public void SendConfirmationEmail(Usuario user)
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
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                string hashedPassword = passwordGenerator(model.senha);
                var user = db.Usuario.FirstOrDefault(u => u.email == model.email && u.senha == hashedPassword);
                if (user != null)
                {
                    if (!user.IsEmailConfirmed)
                    {
                        ModelState.AddModelError("", "E-mail não confirmado. Verifique seu e-mail.");
                        return View(model);
                    }

                    // Configurar o cookie de autenticação
                    FormsAuthentication.SetAuthCookie(user.email, false);
                    var authTicket = new FormsAuthenticationTicket(
                        1, user.email, DateTime.Now, DateTime.Now.AddMinutes(30), false, user.profile);
                    string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                    var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                    HttpContext.Response.Cookies.Add(authCookie);

                    if (user.profile == "Gerente")
                    {
                        return RedirectToAction("Index", "Gerente");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Membro");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Nome de usuário ou senha inválidos.");
                }
            }

            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Debug.WriteLine($"Erro: {error.ErrorMessage}");
            }

            return View(model);
        }

        // Ação de Logout
        [HttpGet]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("Login", "Account");

        }
    }
}