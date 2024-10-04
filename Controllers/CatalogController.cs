using Microsoft.AspNetCore.Mvc;
using WebApplication1.DataConnector;

namespace WebApplication1.Controllers
{
	public class CatalogController : Controller
	{
        QuanLySanPhamContext context;
        public CatalogController()
        {
			context = new QuanLySanPhamContext();

        }
        public IActionResult Index()
		{
			List<Catalog> dsCatalog = context.Catalogs.ToList();
			return View(dsCatalog);
		}

        public ActionResult Details(int id)
        {
            var ctl = context.Catalogs.Find(id);

            return View(ctl);
        }

    }
}
