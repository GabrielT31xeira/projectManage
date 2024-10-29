using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace projectManage.Controllers
{
    [Authorize(Roles = "Membro")]
    public class MembroController : Controller
    {
        // GET: Membro
        public ActionResult Index()
        {
            return View();
        }
    }
}