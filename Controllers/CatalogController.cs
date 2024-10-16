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

        public ActionResult Edit(int id)
        {
            var c = context.Catalogs.Find(id);
            if (c == null)
            {
                return NotFound();
            }

            // Lấy danh sách các catalog từ database và truyền vào ViewBag
            //ViewBag.CatalogId = new SelectList(context.Catalogs, "Id", "Name", product.CatalogId);

            return View(c);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Catalog catalog)
        {
            try
            {
                var c = context.Catalogs.Find(catalog.Id);
                c.CatalogCode = catalog.CatalogCode;
                c.CatalogName = catalog.CatalogName;
                context.Update(c);
                context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Catalog cat)
        {
            try
            {
                var existedCatalog = context.Catalogs.FirstOrDefault(c => c.CatalogCode == cat.CatalogCode);
                if (existedCatalog != null)
                {
                    ModelState.AddModelError("CatalogCode", "CatalogCode already exists.");
                    return View(cat);
                }
                var c = new Catalog();
                c.CatalogCode = cat.CatalogCode;
                c.CatalogName = cat.CatalogName;
                context.Add(c);
                context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            try
            {
                var c = context.Catalogs.Find(id);
                context.Catalogs.Remove(c);
                context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

    }
}
