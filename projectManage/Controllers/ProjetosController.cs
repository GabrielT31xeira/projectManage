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
    public class ProjetosController : Controller
    {
        private AddDbContext db = new AddDbContext();

        // GET: Projetos
        public ActionResult Index()
        {
            return View(db.Projetos.ToList());
        }

        // GET: Projetos/Details/5
        public ActionResult Details(int id)
        {
            var projeto = db.Projetos.Include(p => p.Tarefas).FirstOrDefault(p => p.Id == id);
            if (projeto == null)
            {
                return HttpNotFound();
            }
            return View(projeto);
        }


        // GET: Projetos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Projetos/Create
        // Para proteger-se contra ataques de excesso de postagem, ative as propriedades específicas às quais deseja se associar. 
        // Para obter mais detalhes, confira https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Projeto projeto)
        {
            if (ModelState.IsValid)
            {
                // Obter o usuário autenticado
                var usuarioAutenticado = db.Usuario.FirstOrDefault(u => u.email == User.Identity.Name);
                if (usuarioAutenticado != null)
                {
                    projeto.DonoId = usuarioAutenticado.Id;
                    db.Projetos.Add(projeto);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "Usuário autenticado não encontrado.");
                return RedirectToAction("Index");
            }

            return View(projeto);
        }

        // GET: Projetos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Projeto projeto = db.Projetos.Find(id);
            if (projeto == null)
            {
                return HttpNotFound();
            }
            return View(projeto);
        }

        // POST: Projetos/Edit/5
        // Para proteger-se contra ataques de excesso de postagem, ative as propriedades específicas às quais deseja se associar. 
        // Para obter mais detalhes, confira https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nome,Descricao,DataInicio,DataFim")] Projeto projeto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(projeto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(projeto);
        }

        // GET: Projetos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Projeto projeto = db.Projetos.Find(id);
            if (projeto == null)
            {
                return HttpNotFound();
            }
            return View(projeto);
        }

        // POST: Projetos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Projeto projeto = db.Projetos.Find(id);
            db.Projetos.Remove(projeto);
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

        public ActionResult RemoveUsuario(int projetoId, int usuarioId)
        {
            var projeto = db.Projetos.Include(p => p.Usuarios).FirstOrDefault(p => p.Id == projetoId);
            var usuario = db.Usuario.Find(usuarioId);

            if (projeto != null && usuario != null && projeto.DonoId != usuarioId) // Não permitir remover o dono do projeto
            {
                projeto.Usuarios.Remove(usuario);
                db.SaveChanges();
            }

            return RedirectToAction("Details", new { id = projetoId });
        }

        public ActionResult CriarTarefa(int projetoId)
        {
            var projeto = db.Projetos.Find(projetoId);
            if (projeto == null)
            {
                return HttpNotFound();
            }

            var model = new Tarefa { ProjetoId = projetoId };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CriarTarefa(Tarefa model)
        {
            if (ModelState.IsValid)
            {
                db.Tarefas.Add(model);
                db.SaveChanges();
                return RedirectToAction("Details", new { id = model.ProjetoId });
            }

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Gerente")]
        [ValidateAntiForgeryToken]
        public ActionResult RemoverTarefa(int tarefaId)
        {
            var tarefa = db.Tarefas.Find(tarefaId);
            if (tarefa != null)
            {
                db.Tarefas.Remove(tarefa);
                db.SaveChanges();
            }

            return RedirectToAction("Details", new { id = tarefa.ProjetoId });
        }

        [HttpGet]
        public ActionResult DetailsTarefa(int tarefaId)
        {
            var tarefa = db.Tarefas.Find(tarefaId);
            if (tarefa == null)
            {
                return HttpNotFound();
            }

            return View(tarefa);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoverArquivo(int tarefaId, int arquivoId)
        {
            var tarefa = db.Tarefas.Include(t => t.Arquivos).FirstOrDefault(t => t.Id == tarefaId);
            var arquivo = db.Arquivos.Find(arquivoId);

            if (tarefa != null && arquivo != null)
            {
                tarefa.Arquivos.Remove(arquivo);
                db.SaveChanges();
            }

            return RedirectToAction("DetailsTarefa", new { tarefaId = tarefaId });
        }

    }
}
