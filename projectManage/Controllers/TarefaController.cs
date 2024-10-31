using projectManage.Data;
using projectManage.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace projectManage.Controllers
{
    public class TarefaController : Controller
    {
        private AddDbContext db = new AddDbContext();

        [HttpGet]
        public ActionResult DetailsTarefa(int tarefaId)
        {
            var tarefa = db.Tarefas.Include(t => t.Comentarios).FirstOrDefault(t => t.Id == tarefaId);

            if (tarefa == null)
            {
                return HttpNotFound();
            }

            return View(tarefa);
        }

        // Ação para Criar Arquivo
        [HttpGet]
        public ActionResult CriarArquivo(int tarefaId)
        {
            var tarefa = db.Tarefas.Find(tarefaId);
            if (tarefa == null)
            {
                return HttpNotFound();
            }

            var model = new Arquivo { Tarefas = new List<Tarefa> { tarefa } };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CriarArquivo(Arquivo model, int tarefaId)
        {
            if (ModelState.IsValid)
            {
                var tarefa = db.Tarefas.Include(t => t.Arquivos).FirstOrDefault(t => t.Id == tarefaId);
                if (tarefa == null)
                {
                    return HttpNotFound();
                }

                tarefa.Arquivos.Add(model);
                db.SaveChanges();
                return RedirectToAction("DetailsTarefa", "Projetos", new { tarefaId = tarefaId });
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult CriarComentario(int tarefaId)
        {
            var tarefa = db.Tarefas.Find(tarefaId);
            if (tarefa == null)
            {
                return HttpNotFound();
            }

            var model = new Comentario { TarefaId = tarefaId, Data = DateTime.Now };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CriarComentario(Comentario model, int tarefaId)
        {
            if (ModelState.IsValid)
            {
                db.Comentarios.Add(model);
                db.SaveChanges();
                return RedirectToAction("DetailsTarefa", "Projetos", new { tarefaId = tarefaId });
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoverComentario(int tarefaId, int comentarioId)
        {
            var comentario = db.Comentarios.Find(comentarioId);

            if (comentario != null)
            {
                db.Comentarios.Remove(comentario);
                db.SaveChanges();
            }

            return RedirectToAction("DetailsTarefa", "Projetos", new { tarefaId = tarefaId });
        }
    }
}