﻿using AutoMapper;

using Microsoft.AppCenter.Crashes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using SOColeta.Common.Exceptions;
using SOColeta.Data;
using SOColeta.Models;

using SOCore.Utils;

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using Coleta = SOColeta.Models.Coleta;
using Inventario = SOColeta.Models.Inventario;

namespace SOColeta.Services
{
    public class StockAPIService : IStockService
    {
        private readonly HttpClient httpClient;
        private readonly ILogger<StockAPIService> logger;
        private readonly AppDbContext dbContext;
        private readonly IMapper mapper;

        public StockAPIService(HttpClient httpClient, ILogger<StockAPIService> logger, 
            AppDbContext dbContext, IMapper mapper)
        {
            this.httpClient = httpClient;
            this.logger = logger;
            this.dbContext = dbContext;
            this.mapper = mapper;
        }
        public async Task AddColeta(Coleta coleta)
        {
            string message = "";
            string path = "/api/coletas";
            string json = JsonSerializer.Serialize(coleta);
            try
            {
                StringContent httpContent = new(json, Encoding.Default, "application/json");
                logger.LogDebug("------------------------------------------------------------------");
                logger.LogDebug($"Enviando requisição para salvar a coleta.");
                logger.LogDebug($"Path: {path}");
                logger.LogDebug($"Data: {json}");
                logger.LogDebug("------------------------------------------------------------------");
                HttpResponseMessage response = await httpClient.PostAsync(path, httpContent);
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        break;
                    case HttpStatusCode.NotFound:
                        message = $"Rota não encontrada: {path}";
                        logger.LogError(message);
                        throw new StockAPIException(message);
                    default:
                        message = $"O servidor retornou um status não mapeado: {path}";
                        logger.LogError(message);
                        throw new StockAPIException(message);
                }
            }
            catch (HttpRequestException hre)
            {
                Crashes.TrackError(hre);
                logger.LogError($"Não conseguimos contatar a API em tempo hábil.");
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
                logger.LogDebug("------------------------------------------------------------------");
                logger.LogDebug($"Ocorreu um erro fatal ao salvar os dados na API.");
                logger.LogDebug($"Path: {path}");
                logger.LogDebug($"Data: {json}");
                logger.LogDebug("------------------------------------------------------------------");
                throw new StockAPIException(e.Message, e);
            }
        }

        public async Task AddProduto(Produto produto)
        {
            string message = "";
            string path = "/api/produtos";
            string json = JsonSerializer.Serialize(produto);
            try
            {
                StringContent httpContent = new(json, Encoding.Default, "application/json");
                logger.LogDebug("------------------------------------------------------------------");
                logger.LogDebug($"Enviando requisição para salvar o produto.");
                logger.LogDebug($"Path: {path}");
                logger.LogDebug($"Data: {json}");
                logger.LogDebug("------------------------------------------------------------------");
                HttpResponseMessage response = await httpClient.PostAsync(path, httpContent);
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        break;
                    case HttpStatusCode.NotFound:
                        message = $"Rota não encontrada: {path}";
                        logger.LogError(message);
                        throw new StockAPIException(message);
                    default:
                        message = $"O servidor retornou um status não mapeado: {path}";
                        logger.LogError(message);
                        throw new StockAPIException(message);
                }
            }
            catch (HttpRequestException hre)
            {
                Crashes.TrackError(hre);
                logger.LogError($"Não conseguimos contatar a API em tempo hábil.");
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
                logger.LogDebug("------------------------------------------------------------------");
                logger.LogDebug($"Ocorreu um erro fatal ao salvar os dados na API.");
                logger.LogDebug($"Path: {path}");
                logger.LogDebug($"Data: {json}");
                logger.LogDebug("------------------------------------------------------------------");
                throw new StockAPIException(e.Message, e);
            }
        }

        public Task<Inventario> CreateInventario()
        {            
            Common.Models.Inventario inventario = new()
            {
                DataCriacao = DateTime.Now,
                Device = SOHelper.Serial
            };
            return CreateInventario(inventario);
        }
        public async Task<Inventario> CreateInventario(Common.Models.Inventario inventario)
        {
            string message = "";
            string path = "/api/inventarios";
            string json = JsonSerializer.Serialize(inventario);
            try
            {
                StringContent httpContent = new(json, Encoding.Default, "application/json");
                logger.LogDebug("------------------------------------------------------------------");
                logger.LogDebug($"Enviando requisição para salvar o produto.");
                logger.LogDebug($"Path: {path}");
                logger.LogDebug($"Data: {json}");
                logger.LogDebug("------------------------------------------------------------------");
                HttpResponseMessage response = await httpClient.PostAsync(path, httpContent);
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        string responseText = await response.Content.ReadAsStringAsync();
                        return JsonSerializer.Deserialize<Inventario>(responseText);
                    case HttpStatusCode.NotFound:
                        message = $"Rota não encontrada: {path}";
                        logger.LogError(message);
                        throw new StockAPIException(message);
                    default:
                        message = $"O servidor retornou um status não mapeado: {path}";
                        logger.LogError(message);
                        throw new StockAPIException(message);
                }
            }
            catch (HttpRequestException hre)
            {
                Crashes.TrackError(hre);
                logger.LogError($"Não conseguimos contatar a API em tempo hábil.");
                throw new StockAPIException("Não foi possível conectar ao servidor, verifique se o mesmo está ligado ou contate o suporte!");
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
                logger.LogDebug("------------------------------------------------------------------");
                logger.LogDebug($"Ocorreu um erro fatal ao salvar os dados na API.");
                logger.LogDebug($"Path: {path}");
                logger.LogDebug($"Data: {json}");
                logger.LogDebug("------------------------------------------------------------------");
                throw new StockAPIException(e.Message, e);
            }
        }

        public async Task ExportInventario(Inventario inventario)
        {
            string message = "";
            string path = $"/api/inventarios/{inventario.Id}";
            string json = JsonSerializer.Serialize(inventario);
            try
            {
                StringContent httpContent = new(json, Encoding.Default, "application/json");
                logger.LogDebug("------------------------------------------------------------------");
                logger.LogDebug($"Enviando requisição para salvar o produto.");
                logger.LogDebug($"Path: {path}");
                logger.LogDebug($"Data: {json}");
                logger.LogDebug("------------------------------------------------------------------");
                HttpResponseMessage response = await httpClient.PostAsync(path, httpContent);
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        break;
                    case HttpStatusCode.NotFound:
                        message = $"Rota não encontrada: {path}";
                        logger.LogError(message);
                        throw new StockAPIException(message);
                    default:
                        message = $"O servidor retornou um status não mapeado: {path}";
                        logger.LogError(message);
                        throw new StockAPIException(message);
                }
            }
            catch (HttpRequestException hre)
            {
                Crashes.TrackError(hre);
                logger.LogError($"Não conseguimos contatar a API em tempo hábil.");
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
                logger.LogDebug("------------------------------------------------------------------");
                logger.LogDebug($"Ocorreu um erro fatal ao salvar os dados na API.");
                logger.LogDebug($"Path: {path}");
                logger.LogDebug($"Data: {json}");
                logger.LogDebug("------------------------------------------------------------------");
                throw new StockAPIException(e.Message, e);
            }
        }

        public async Task FinishInventario()
        {
            string message = "";
            string path = "/api/inventarios/finish";
            try
            {
                logger.LogDebug("------------------------------------------------------------------");
                logger.LogDebug($"Enviando requisição para salvar o produto.");
                logger.LogDebug($"Path: {path}");
                logger.LogDebug("------------------------------------------------------------------");
                HttpResponseMessage response = await httpClient.PostAsync(path, new StringContent(""));
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        break;
                    case HttpStatusCode.NotFound:
                        message = $"Rota não encontrada: {path}";
                        logger.LogError(message);
                        throw new StockAPIException(message);
                    default:
                        message = $"O servidor retornou um status não mapeado: {path}";
                        logger.LogError(message);
                        throw new StockAPIException(message);
                }
            }
            catch (HttpRequestException hre)
            {
                Crashes.TrackError(hre);
                logger.LogError($"Não conseguimos contatar a API em tempo hábil.");
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
                logger.LogDebug("------------------------------------------------------------------");
                logger.LogDebug($"Ocorreu um erro fatal ao salvar os dados na API.");
                logger.LogDebug($"Path: {path}");
                logger.LogDebug("------------------------------------------------------------------");
                throw new StockAPIException(e.Message, e);
            }
        }

        public async IAsyncEnumerable<Coleta> GetColetasAsync(string id = null)
        {
            List<Coleta> coletas = new();
            string message = "";
            string path = $"/api/coletas?inventario={id}";
            try
            {
                logger.LogDebug("------------------------------------------------------------------");
                logger.LogDebug($"Buscando contagem de inventarios em aberto.");
                logger.LogDebug($"Path: {path}");
                logger.LogDebug("------------------------------------------------------------------");
                HttpResponseMessage response = await httpClient.GetAsync(path);
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        string responseText = await response.Content.ReadAsStringAsync();
                        coletas = JsonSerializer.Deserialize<List<Coleta>>(responseText);
                        break;
                    case HttpStatusCode.NotFound:
                        message = $"Rota não encontrada: {path}";
                        logger.LogError(message);
                        throw new StockAPIException(message);
                    default:
                        message = $"O servidor retornou um status não mapeado: {path}";
                        logger.LogError(message);
                        throw new StockAPIException(message);
                }
            }
            catch (HttpRequestException hre)
            {
                Crashes.TrackError(hre);
                logger.LogError($"Não conseguimos contatar a API em tempo hábil.");
                throw new StockAPIException("Não foi possível conectar ao servidor, verifique se o mesmo está ligado ou contate o suporte!");
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
                logger.LogDebug("------------------------------------------------------------------");
                logger.LogDebug($"Ocorreu um erro fatal ao salvar os dados na API.");
                logger.LogDebug($"Path: {path}");
                logger.LogDebug("------------------------------------------------------------------");
                throw new StockAPIException(e.Message, e);
            }
            foreach (Coleta coleta in coletas)
                yield return coleta;
        }

        public async IAsyncEnumerable<Inventario> GetFinishedInventarios()
        {
            List<Inventario> inventarios = new();
            string message = "";
            string path = $"/api/inventarios?finished={true}";
            try
            {
                logger.LogDebug("------------------------------------------------------------------");
                logger.LogDebug($"Buscando inventários finalizados.");
                logger.LogDebug($"Path: {path}");
                logger.LogDebug("------------------------------------------------------------------");
                HttpResponseMessage response = await httpClient.GetAsync(path);
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        string responseText = await response.Content.ReadAsStringAsync();
                        inventarios = JsonSerializer.Deserialize<List<Inventario>>(responseText);
                        break;
                    case HttpStatusCode.NotFound:
                        message = $"Rota não encontrada: {path}";
                        logger.LogError(message);
                        throw new StockAPIException(message);
                    default:
                        message = $"O servidor retornou um status não mapeado: {path}";
                        logger.LogError(message);
                        throw new StockAPIException(message);
                }
            }
            catch (HttpRequestException hre)
            {
                Crashes.TrackError(hre);
                logger.LogError($"Não conseguimos contatar a API em tempo hábil.");
                throw new StockAPIException("Não foi possível conectar ao servidor, verifique se o mesmo está ligado ou contate o suporte!");
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
                logger.LogDebug("------------------------------------------------------------------");
                logger.LogDebug($"Ocorreu um erro fatal ao salvar os dados na API.");
                logger.LogDebug($"Path: {path}");
                logger.LogDebug("------------------------------------------------------------------");
                throw new StockAPIException(e.Message, e);
            }
            foreach (Inventario inventario in inventarios)
                yield return inventario;
        }

        public async Task<Inventario> GetOpenedInventario()
        {
            string message = "";
            string path = $"/api/inventarios/open?device={SOHelper.Serial}";
            try
            {
                logger.LogDebug("------------------------------------------------------------------");
                logger.LogDebug($"Buscando inventário em aberto.");
                logger.LogDebug($"Path: {path}");
                logger.LogDebug("------------------------------------------------------------------");
                HttpResponseMessage response = await httpClient.GetAsync(path);
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        string responseText = await response.Content.ReadAsStringAsync();
                        return JsonSerializer.Deserialize<Inventario>(responseText);
                    case HttpStatusCode.NotFound:
                        message = $"Rota não encontrada: {path}";
                        logger.LogError(message);
                        throw new StockAPIException(message);
                    default:
                        message = $"O servidor retornou um status não mapeado: {path}";
                        logger.LogError(message);
                        throw new StockAPIException(message);
                }
            }
            catch (HttpRequestException hre)
            {
                Crashes.TrackError(hre);
                logger.LogError($"Não conseguimos contatar a API em tempo hábil.");
                throw new StockAPIException("Não foi possível conectar ao servidor, verifique se o mesmo está ligado ou contate o suporte!");
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
                logger.LogDebug("------------------------------------------------------------------");
                logger.LogDebug($"Ocorreu um erro fatal ao salvar os dados na API.");
                logger.LogDebug($"Path: {path}");
                logger.LogDebug("------------------------------------------------------------------");
                throw new StockAPIException(e.Message, e);
            }
        }

        public async Task<Produto> GetProduto(string barcode)
        {
            string message = "";
            string path = $"/api/produtos?barcode={barcode}";
            try
            {
                logger.LogDebug("------------------------------------------------------------------");
                logger.LogDebug($"Buscando produto.");
                logger.LogDebug($"Path: {path}");
                logger.LogDebug("------------------------------------------------------------------");
                HttpResponseMessage response = await httpClient.GetAsync(path);
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        string responseText = await response.Content.ReadAsStringAsync();
                        return JsonSerializer.Deserialize<Produto>(responseText);
                    case HttpStatusCode.NotFound:
                        message = $"Rota não encontrada: {path}";
                        logger.LogError(message);
                        throw new StockAPIException(message);
                    default:
                        message = $"O servidor retornou um status não mapeado: {path}";
                        logger.LogError(message);
                        throw new StockAPIException(message);
                }
            }
            catch (HttpRequestException hre)
            {
                Crashes.TrackError(hre);
                logger.LogError($"Não conseguimos contatar a API em tempo hábil.");
                throw new StockAPIException("Não foi possível conectar ao servidor, verifique se o mesmo está ligado ou contate o suporte!");
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
                logger.LogDebug("------------------------------------------------------------------");
                logger.LogDebug($"Ocorreu um erro fatal ao salvar os dados na API.");
                logger.LogDebug($"Path: {path}");
                logger.LogDebug("------------------------------------------------------------------");
                throw new StockAPIException(e.Message, e);
            }
        }

        public async Task<bool> InventarioHasColeta()
        {
            string message = "";
            string path = $"/api/inventarios/valid?device={SOHelper.Serial}";
            try
            {
                logger.LogDebug("------------------------------------------------------------------");
                logger.LogDebug($"Buscando produto.");
                logger.LogDebug($"Path: {path}");
                logger.LogDebug("------------------------------------------------------------------");
                HttpResponseMessage response = await httpClient.GetAsync(path);
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        string responseText = await response.Content.ReadAsStringAsync();
                        return JsonSerializer.Deserialize<bool>(responseText);
                    case HttpStatusCode.NotFound:
                        message = $"Rota não encontrada: {path}";
                        logger.LogError(message);
                        throw new StockAPIException(message);
                    default:
                        message = $"O servidor retornou um status não mapeado: {path}";
                        logger.LogError(message);
                        throw new StockAPIException(message);
                }
            }
            catch (HttpRequestException hre)
            {
                Crashes.TrackError(hre);
                logger.LogError($"Não conseguimos contatar a API em tempo hábil.");
                throw new StockAPIException("Não foi possível conectar ao servidor, verifique se o mesmo está ligado ou contate o suporte!");
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
                logger.LogDebug("------------------------------------------------------------------");
                logger.LogDebug($"Ocorreu um erro fatal ao salvar os dados na API.");
                logger.LogDebug($"Path: {path}");
                logger.LogDebug("------------------------------------------------------------------");
                throw new StockAPIException(e.Message, e);
            }
        }

        public async Task RemoveColeta(Coleta coleta)
        {
            string message = "";
            string path = $"/api/coletas/{coleta.Id}";
            string json = JsonSerializer.Serialize(coleta);
            try
            {
                StringContent httpContent = new(json, Encoding.Default, "application/json");
                logger.LogDebug("------------------------------------------------------------------");
                logger.LogDebug($"Enviando requisição para salvar o produto.");
                logger.LogDebug($"Path: {path}");
                logger.LogDebug($"Data: {json}");
                logger.LogDebug("------------------------------------------------------------------");
                HttpResponseMessage response = await httpClient.DeleteAsync(path);
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        break;
                    case HttpStatusCode.NotFound:
                        message = $"Rota não encontrada: {path}";
                        logger.LogError(message);
                        throw new StockAPIException(message);
                    default:
                        message = $"O servidor retornou um status não mapeado: {path}";
                        logger.LogError(message);
                        throw new StockAPIException(message);
                }
            }
            catch (HttpRequestException hre)
            {
                Crashes.TrackError(hre);
                logger.LogError($"Não conseguimos contatar a API em tempo hábil.");
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
                logger.LogDebug("------------------------------------------------------------------");
                logger.LogDebug($"Ocorreu um erro fatal ao salvar os dados na API.");
                logger.LogDebug($"Path: {path}");
                logger.LogDebug($"Data: {json}");
                logger.LogDebug("------------------------------------------------------------------");
                throw new StockAPIException(e.Message, e);
            }
        }

        public async Task SyncData()
        {
            var inventarios = await dbContext.Inventarios
                .ToArrayAsync();
            var coletas = await dbContext.Coletas.ToArrayAsync();

            try
            {
                foreach (var inventario in inventarios)
                {
                    await CreateInventario(mapper.Map<Common.Models.Inventario>(inventario));
                    dbContext.Inventarios.Remove(inventario);
                }
            }
            catch (Exception)
            {

            }

            try
            {
                foreach (var coleta in coletas)
                {
                    await AddColeta(coleta);
                    dbContext.Coletas.Remove(coleta);
                }
            }
            catch (Exception)
            {

            }

            try
            {
                await dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {

            }
        }
    }
}
