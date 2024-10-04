using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.DataConnector;

namespace WebApplication1.Controllers
{
	public class ProductController : Controller
	{
		QuanLySanPhamContext context;
        public ProductController()
        {
            
			context = new QuanLySanPhamContext();
        }
        // GET: ProductController
        public ActionResult Index()
		{
			List<Product> dsSp = context.Products.ToList();
			return View(dsSp);
		}

		// GET: ProductController/Details/5
		public ActionResult Details(int id)
		{
            var product = context.Products.Find(id);

            return View(product);
		}

		// GET: ProductController/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: ProductController/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(IFormCollection collection)
		{
			try
			{
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}

		// GET: ProductController/Edit/5
		public ActionResult Edit(int id)
		{
			return View();
		}

		// POST: ProductController/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(int id, IFormCollection collection)
		{
			try
			{
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}

		// GET: ProductController/Delete/5
		public ActionResult Delete(int id)
		{
			return View();
		}

		// POST: ProductController/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(int id, IFormCollection collection)
		{
			try
			{
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}
	}
}
