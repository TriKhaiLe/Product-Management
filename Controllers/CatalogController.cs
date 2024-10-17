using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.DataConnector;
using WebApplication1.DTOs;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CatalogController : ControllerBase
    {
        QuanLySanPhamContext _context;
        private readonly IMapper _mapper;
        public CatalogController(IMapper mapper, QuanLySanPhamContext context)
        {
			_context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetCatalogs()
		{
			List<Catalog> dsCatalog = [.. _context.Catalogs];
			return Ok(dsCatalog);
		}

        [HttpGet("{id}")]
        public ActionResult GetCatalogById(int id)
        {
            var ctl = _context.Catalogs.Find(id);
            if (ctl == null)
                return NotFound();

            return Ok(ctl);
        }

        [HttpPut]
        public ActionResult Edit(UpdateCatalogDto inputCatalog)
        {
            try
            {
                var c = _context.Catalogs.Find(inputCatalog.Id);
                if (c == null)
                    return NotFound();

                var existedCatalog = _context.Catalogs
                    .FirstOrDefault(x => x.CatalogCode == inputCatalog.CatalogCode && x.Id != inputCatalog.Id);

                if (existedCatalog != null)
                {
                    return BadRequest("This catalog code is existed");
                }

                // Cập nhật catalog
                c.CatalogCode = inputCatalog.CatalogCode;
                c.CatalogName = inputCatalog.CatalogName;
                _context.Update(c);
                _context.SaveChanges();

                return Ok(c);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message); // 500 Internal Server Error
            }
        }

        [HttpPost]
        public ActionResult AddCatalog(CreateCatalogDto cat)
        {
            try
            {
                if (cat == null)
                    return BadRequest("Info can't be empty");

                var existedCatalog = _context.Catalogs.FirstOrDefault(c => c.CatalogCode == cat.CatalogCode);
                if (existedCatalog != null)
                {
                    return BadRequest("This catalog code is already existed");
                }
                var c = new Catalog();
                c.CatalogCode = cat.CatalogCode;
                c.CatalogName = cat.CatalogName;
                _context.Add(c);
                _context.SaveChanges();
                return Ok(c);
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
                var c = _context.Catalogs.Find(id);
                if (c == null)
                {
                    return NotFound("Catalog not found");
                }

                var relatedProducts = _context.Products.Where(p => p.CatalogId == id).ToList();
                if (relatedProducts.Count != 0)
                {
                    return BadRequest("This catalog is referenced by existing products and cannot be deleted");
                }

                _context.Catalogs.Remove(c);
                _context.SaveChanges();

                return Ok("Catalog deleted successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message); // 500 Internal Server Error
            }
        }

    }
}
