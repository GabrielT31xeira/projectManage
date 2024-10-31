using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using projectManage.Data;
using projectManage.Models;

namespace projectManage.Controllers
{
    public class EquipesController : Controller
    {
        private AddDbContext db = new AddDbContext();

        // POST: Equipes/EntrarNaEquipe
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EntrarNaEquipe(int id)
        {
            var usuarioAutenticado = db.Usuario.FirstOrDefault(u => u.email == User.Identity.Name);
            var userId = usuarioAutenticado.Id;

            var equipe = db.Equipes.Include(e => e.Usuarios).FirstOrDefault(e => e.Id == id);
            var usuario = db.Usuario.Find(userId);

            if (equipe != null && usuario != null && !equipe.Usuarios.Contains(usuario))
            {
                equipe.Usuarios.Add(usuario);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        // POST: Equipes/SairDaEquipe
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SairDaEquipe(int id)
        {
            var usuarioAutenticado = db.Usuario.FirstOrDefault(u => u.email == User.Identity.Name);
            var userId = usuarioAutenticado.Id;
            var equipe = db.Equipes.Include(e => e.Usuarios).FirstOrDefault(e => e.Id == id);
            var usuario = db.Usuario.Find(userId);

            if (equipe != null && usuario != null)
            {
                equipe.Usuarios.Remove(usuario);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        // GET: Equipes
        public ActionResult Index()
        {
            var usuarioAutenticado = db.Usuario.FirstOrDefault(u => u.email == User.Identity.Name);
            var userId = usuarioAutenticado.Id;

            var minhasEquipes = db.Equipes
                              .Include(e => e.Usuarios)
                              .Where(e => e.Usuarios.Any(u => u.Id == userId))
                              .ToList();

            var outrasEquipes = db.Equipes
                                   .Include(e => e.Usuarios)
                                   .Where(e => !e.Usuarios.Any(u => u.Id == userId))
                                   .ToList();

            var model = new EquipeViewModel
            {
                MinhasEquipes = minhasEquipes,
                OutrasEquipes = outrasEquipes
            };
            return View(model);
        }

        // GET: Equipes/Details/5
        public ActionResult Details(int? id)
        {
            var equipe = db.Equipes.Include(e => e.Usuarios).FirstOrDefault(e => e.Id == id);
            if (equipe == null)
            {
                return HttpNotFound();
            }
            return View(equipe);
        }

        // GET: Equipes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Equipes/Create
        // Para proteger-se contra ataques de excesso de postagem, ative as propriedades específicas às quais deseja se associar. 
        // Para obter mais detalhes, confira https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Equipe model)
        {
            if (ModelState.IsValid)
            {
                db.Equipes.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // GET: Equipes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Equipe equipe = db.Equipes.Find(id);
            if (equipe == null)
            {
                return HttpNotFound();
            }
            return View(equipe);
        }

        // POST: Equipes/Edit/5
        // Para proteger-se contra ataques de excesso de postagem, ative as propriedades específicas às quais deseja se associar. 
        // Para obter mais detalhes, confira https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nome")] Equipe equipe)
        {
            if (ModelState.IsValid)
            {
                db.Entry(equipe).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(equipe);
        }

        // GET: Equipes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Equipe equipe = db.Equipes.Find(id);
            if (equipe == null)
            {
                return HttpNotFound();
            }
            return View(equipe);
        }

        // POST: Equipes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Equipe equipe = db.Equipes.Find(id);
            db.Equipes.Remove(equipe);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoverUsuario(int equipeId, int usuarioId)
        {
            var equipe = db.Equipes.Include(e => e.Usuarios).FirstOrDefault(e => e.Id == equipeId);
            var usuario = db.Usuario.Find(usuarioId);

            if (equipe != null && usuario != null)
            {
                equipe.Usuarios.Remove(usuario);
                db.SaveChanges();
            }

            return RedirectToAction("Details", new { id = equipeId });
        }
    }
}
