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

            return View(c);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Catalog inputCatalog)
        {
            try
            {
                var c = context.Catalogs.Find(inputCatalog.Id);
                if (c == null)
                    return NotFound();

                var existedCatalog = context.Catalogs
                    .FirstOrDefault(x => x.CatalogCode == inputCatalog.CatalogCode && x.Id != inputCatalog.Id);

                if (existedCatalog != null)
                {
                    ModelState.AddModelError("CatalogCode", "CatalogCode already exists.");
                    return View(inputCatalog);
                }

                // Cập nhật catalog
                c.CatalogCode = inputCatalog.CatalogCode;
                c.CatalogName = inputCatalog.CatalogName;
                context.Update(c);
                context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(inputCatalog);
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
                if (c == null)
                {
                    TempData["ErrorMessage"] = "Catalog not found.";
                    return RedirectToAction(nameof(Index));
                }

                var relatedProducts = context.Products.Where(p => p.CatalogId == id).ToList();
                if (relatedProducts.Any())
                {
                    TempData["ErrorMessage"] = "This catalog is referenced by existing products and cannot be deleted.";
                    return RedirectToAction(nameof(Index));
                }

                context.Catalogs.Remove(c);
                context.SaveChanges();

                TempData["SuccessMessage"] = "Catalog deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while deleting the catalog: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

    }
}
