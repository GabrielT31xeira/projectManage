using System.Web.Mvc;
using Crud.Data;

namespace projectManage.Controllers
{
    public class UserController : Controller
    {
        private AppDbContext db = new AppDbContext();
        
        // GET
        public ActionResult Index()
        {
            return View();
        }
    }
}