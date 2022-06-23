using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SOColeta.Common.Services;
using System.Net;

namespace SOColeta.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IStokService stokService;

        public ProductController(ILogger<ProductController> logger, IStokService stokService)
        {
            _logger = logger;
            this.stokService = stokService;
        }
        [AllowAnonymous]
        [HttpGet("{barcode}")]
        public async Task<IActionResult> GetProduct(string barcode, long? empresa = null)
        {
            _logger.LogDebug(barcode);
            try
            {
                var produto = await stokService.GetProduct(barcode, empresa);

                return Ok(produto);
            }
            catch (Exception e)
            {
                _logger.LogError(e.StackTrace);
                return StatusCode((int)HttpStatusCode.BadRequest, e.StackTrace);
            }
        }
    }
}
