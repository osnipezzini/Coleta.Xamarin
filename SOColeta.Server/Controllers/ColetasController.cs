using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using SOColeta.Common.DataModels;
using SOColeta.Common.Services;

namespace SOColeta.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ColetasController : ControllerBase
{
    private readonly IColetaService coletaService;
    private readonly ILogger<ColetasController> logger;

    public ColetasController(IColetaService coletaService, ILogger<ColetasController> logger)
    {
        this.coletaService = coletaService;
        this.logger = logger;
    }
    [AllowAnonymous]
    [HttpGet("{inventario}")]
    public async Task<IActionResult> GetColeta(Guid? inventario)
    {
        try
        {
            var coletas = await coletaService.GetColeta(inventario);

            return Ok(coletas);
        }
        catch (Exception e)
        {
            logger.LogDebug(e.StackTrace);
            logger.LogError(e.Message);
            return Ok(e.Message);
        }
    }
    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> AdicionarColeta(ColetaModel model)
    {
        try
        {
            await coletaService.AddColeta(model);
            return Ok();
        }
        catch (AutoMapperMappingException amme)
        {
            logger.LogDebug(amme.StackTrace);
            logger.LogError("Erro ao mapear propriedades: " + amme.Message);
            return BadRequest(amme.Message);
        }
        catch (Exception e)
        {
            logger.LogDebug(e.StackTrace);
            logger.LogError(e.Message);
            return Ok(e.Message);
        }
    }
}
