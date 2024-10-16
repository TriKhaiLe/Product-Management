using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DataConnector;
using WebApplication1.DTOs;

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
        public ActionResult Create(CreateNewProductDto inputProduct)
        {
            try
            {
                if (inputProduct == null)
                {
                    ModelState.AddModelError("", "Invalid product data.");
                    return View(inputProduct);
                }

                var existedProduct = context.Products.FirstOrDefault(x => x.ProductCode == inputProduct.ProductCode);
                if (existedProduct != null)
                {
                    ModelState.AddModelError("ProductCode", "ProductCode already exists.");
                    return View(inputProduct);
                }

                // Kiểm tra xem Catalog có tồn tại không dựa vào CatalogCode
                var catalog = context.Catalogs.FirstOrDefault(c => c.CatalogCode == inputProduct.CatalogCode);
                if (catalog == null)
                {
                    ModelState.AddModelError("", $"Catalog with code {inputProduct.CatalogCode} not found.");
                    return View(inputProduct);
                }

                // Tạo một sản phẩm mới từ DTO
                var product = new Product
                {
                    CatalogId = catalog.Id,  // Liên kết với Catalog
                    ProductCode = inputProduct.ProductCode,
                    ProductName = inputProduct.ProductName,
                    Picture = inputProduct.Picture,
                    UnitPrice = inputProduct.UnitPrice
                };

                // Thêm sản phẩm mới vào database
                context.Products.Add(product);
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
