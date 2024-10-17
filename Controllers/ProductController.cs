using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DataConnector;
using WebApplication1.DTOs;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
	{
        QuanLySanPhamContext _context;
        private readonly IMapper _mapper;

        public ProductController(IMapper mapper)
        {
            
			_context = new QuanLySanPhamContext();
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetProducts()
		{
			List<Product> dsSp = [.. _context.Products];
			return Ok(dsSp);
		}

        [HttpGet("{id}")]
        public ActionResult GetProductById(int id)
		{
            var product = _context.Products.Find(id);
            if (product == null)
                return NotFound();

            return Ok(product);
		}

        [HttpPost]
        public ActionResult Create(CreateNewProductDto inputProduct)
        {
            try
            {
                if (inputProduct == null)
                    return BadRequest("Info can't be empty");

                var existedProduct = _context.Products.FirstOrDefault(x => x.ProductCode == inputProduct.ProductCode);
                if (existedProduct != null)
                {
                    return BadRequest("This product code is already existed");
                }

                // Kiểm tra xem Catalog có tồn tại không dựa vào CatalogCode
                var catalog = _context.Catalogs.FirstOrDefault(c => c.CatalogCode == inputProduct.CatalogCode);
                if (catalog == null)
                    return NotFound($"Catalog with code {inputProduct.CatalogCode} not found.");

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
                _context.Products.Add(product);
                _context.SaveChanges();

                UpdateProductDto returnObject = _mapper.Map<UpdateProductDto>(product);
                returnObject.CatalogCode = inputProduct.CatalogCode;
                return Ok(returnObject);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message); // 500 Internal Server Error
            }
        }

        // POST: ProductController/Edit/5
        [HttpPut]
		public ActionResult Edit(UpdateProductDto inputProduct)
		{
			try
			{
                var p = _context.Products.Find(inputProduct.Id);
                if (p == null)
                    return NotFound();

                var catalog = _context.Catalogs
                    .FirstOrDefault(x => x.CatalogCode == inputProduct.CatalogCode);
                if (catalog == null)
                    return NotFound($"Catalog with code {inputProduct.CatalogCode} not found.");

                var existedProduct = _context.Products
                    .FirstOrDefault(x => x.ProductCode == inputProduct.ProductCode && x.Id != inputProduct.Id);
                if (existedProduct != null)
                {
                    return BadRequest("This product code is already existed");
                }

                //p.CatalogId = catalog.Id;
                //p.UnitPrice = inputProduct.UnitPrice;
                //p.Picture = inputProduct.Picture;
                //p.ProductName = inputProduct.ProductName;
                //p.ProductCode = inputProduct.ProductCode;

                _mapper.Map(inputProduct, p);
                p.CatalogId = catalog.Id;

                _context.Update(p);
                _context.SaveChanges();

                return Ok(inputProduct);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message); // 500 Internal Server Error
            }
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
		{
            try
            {
                var p = _context.Products.Find(id);
                if (p == null) return NotFound("Product not found");


                _context.Products.Remove(p);
                _context.SaveChanges();

                return Ok("Product deleted successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message); // 500 Internal Server Error
            }
        }
	}
}
