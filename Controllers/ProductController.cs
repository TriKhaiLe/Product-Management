using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DataConnector;

namespace WebApplication1.Controllers
{
	public class ProductController : Controller
	{
		[BindProperty]
        public int? CatalogId { get; set; }


        [BindProperty]
        public string? ProductCode { get; set; }

        [BindProperty]
        public string? ProductName { get; set; }

        [BindProperty]
        public string? Picture { get; set; }

        [BindProperty]
        public double? UnitPrice { get; set; }

        QuanLySanPhamContext context;
        public ProductController()
        {
            
			context = new QuanLySanPhamContext();
        }

        // GET: ProductController
        public ActionResult Index()
		{
			List<Product> dsSp = context.Products.Include(p => p.Catalog).ToList();
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
        public ActionResult Create(IFormCollection form)
        {
            try
            {
                // Lấy CatalogCode từ form
                string catalogCode = form["CatalogCode"];

                // Tra cứu CatalogId dựa trên CatalogCode
                var catalog = context.Catalogs.FirstOrDefault(c => c.CatalogCode == catalogCode);

                if (catalog == null)
                {
                    // Nếu không tìm thấy Catalog với mã CatalogCode, trả về lỗi
                    ModelState.AddModelError("CatalogCode", "Catalog code không tồn tại.");
                    return View();
                }

                // Tạo sản phẩm mới với CatalogId đã tìm thấy
                var product = new Product
                {
                    UnitPrice = Convert.ToDouble(form["UnitPrice"]),
                    Picture = form["Picture"],
                    ProductName = form["ProductName"],
                    ProductCode = form["ProductCode"],
                    CatalogId = catalog.Id // Gán CatalogId sau khi tra cứu
                };

                // Thêm sản phẩm vào database
                context.Add(product);
                context.SaveChanges();
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
            var product = context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            // Lấy danh sách các catalog từ database và truyền vào ViewBag
            //ViewBag.CatalogId = new SelectList(context.Catalogs, "Id", "Name", product.CatalogId);

            return View(product);
        }

        // POST: ProductController/Edit/5
        [HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(int id, IFormCollection collection)
		{
			try
			{
                var p = context.Products.Find(id);
                p.UnitPrice = UnitPrice;
                p.Picture = Picture;
                p.ProductName = ProductName;
                p.ProductCode = ProductCode;
                p.CatalogId = CatalogId;
                context.Update(p);
                context.SaveChanges();
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
            try
            {
                var p = context.Products.Find(id);
                context.Products.Remove(p);
                context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        // POST: ProductController/Delete/5
        [HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(int id, IFormCollection collection)
		{
			try
			{
                context.Products.Remove(context.Products.Find(id));
				return RedirectToAction(nameof(Index));
			}
			catch (Exception ex)
			{
				return View(ex.Message);
			}
		}
	}
}
