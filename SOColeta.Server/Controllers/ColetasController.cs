using AutoMapper;

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

    [HttpGet("{codigo}/{inventario}")]
    public async Task<IActionResult> GetColeta(string codigo, int inventario)
    {
        var coleta = await coletaService.GetColeta(codigo, inventario);

        return Ok(coleta);
    }
    [HttpPost]
    public async Task<IActionResult> AdicionarColeta(ColetaModel model)
    {
        try
        {
            var coleta = await coletaService.AddColeta(model);
            return Ok(coleta);
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
