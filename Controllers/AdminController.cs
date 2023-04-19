using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication4.Model;

namespace WebApplication4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly ProductContext _dbContex;
        private object _dbContext;

        public AdminController(ProductContext dbContext)
        {
            _dbContex = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProduct()
        {
            if (_dbContex.Products == null)
            {
                return NotFound();
            }
            return await _dbContex.Products.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProduct(int id)
        {
            if (_dbContex.Products == null)
            {
                return NotFound();
            }
            var product = await _dbContex.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]
        public async  Task<ActionResult<Product>> PostProduct(Product product)
        {
            _dbContex.Products.Add(product);
            await _dbContex.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProduct), new {id=product.Id},product);
        }

        [HttpPut]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if(id!=product.Id)
            {
                return BadRequest();
            }
            _dbContex.Entry(product).State = EntityState.Modified;

            try
            {
                await _dbContex.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) 
            {
                if (!ProductAvailable(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
                
            
            }
            return Ok();
        }

        private bool ProductAvailable(int id)
        {
            return (_dbContex.Products?.Any(p => p.Id == id)).GetValueOrDefault();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBrand(int id)
        {
            if (_dbContex.Products == null)
            {
                return NotFound();
            }
            var product =await _dbContex.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
             _dbContex.Products.Remove(product);
            await _dbContex.SaveChangesAsync();
            return Ok();

        }


    }
}
