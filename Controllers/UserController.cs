using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication4.Model;

namespace WebApplication4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        //[Route("api/[controller]")]
      //  [ApiController]

        private readonly ProductContext _dbContex;
        private object _dbContext;

        public UserController(ProductContext dbContext)
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

        [HttpGet("{productname}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProduct(string productname)
        {
            if (_dbContex.Products == null)
            {
                return NotFound();
            }
            var product = await _dbContex.Products.FirstOrDefaultAsync(p => p.ProductName == productname);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

    }
}
