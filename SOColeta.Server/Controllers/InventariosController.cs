﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using SOColeta.Common.DataModels;
using SOColeta.Common.Models;
using SOColeta.Common.Services;

using System.Net;

namespace SOColeta.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventariosController : ControllerBase
    {
        private readonly ILogger<InventariosController> _logger;
        private readonly IStokService stokService;

        public InventariosController(ILogger<InventariosController> logger, IStokService stokService)
        {
            _logger = logger;
            this.stokService = stokService;
        }
        [AllowAnonymous]
        [HttpGet("{serial}")]
        public async Task<IActionResult> GetInventario(string serial)
        {
            _logger.LogDebug(serial);
            try
            {
                var inventario = await stokService.GetInventario(serial);
                
                return Ok(inventario);
            }
            catch (Exception e)
            {
                _logger.LogError(e.StackTrace);
                return StatusCode((int)HttpStatusCode.BadRequest, e.StackTrace);
            }
        }
        [AllowAnonymous]
        [HttpPost("{inventarioGuid}/")]
        public async Task<IActionResult> LancarInventario(Guid? inventarioGuid,
            [FromQuery] long? pessoa)
        {
            try
            {
                await stokService.LancarInventario(inventarioGuid, pessoa);
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                _logger.LogDebug(e.StackTrace);
                return StatusCode((int)HttpStatusCode.BadRequest, e.StackTrace);
            }
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> BuscarInventarios([FromQuery] string inserted)
        {
            try
            {
                var inventarios = await stokService.BuscarInventarios(inserted);
                return Ok(inventarios);
            }
            catch (Exception e)
            {
                _logger.LogError(e.StackTrace);
                return Ok(e.Message);
            }
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> RegistrarInventario(Inventario inventario)
        {
            try
            {
                var model = await stokService.RegistrarInventario(inventario);
                return Ok(model);
            }
            catch (Exception e)
            {
                _logger.LogError(e.StackTrace);
                return Ok(e.Message);
            }
        }
        [AllowAnonymous]
        [HttpPost("salvar")]
        public async Task<IActionResult> FinalizarInventario()
        {
            try
            {
                var inventario = await stokService.FinalizarInventario();
                if (inventario is null)
                    return NotFound("Inventário não existe!");
                return Ok(inventario);
            }
            catch (Exception e)
            {
                _logger.LogError(e.StackTrace);
                return Ok(e.Message);
            }
        }
    }
}
