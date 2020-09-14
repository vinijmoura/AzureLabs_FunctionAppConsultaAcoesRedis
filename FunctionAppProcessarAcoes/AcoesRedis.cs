using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using FunctionAppConsultaAcoesRedis.Models;
using FunctionAppConsultaAcoesRedis.Data;

namespace FunctionAppConsultaAcoesRedis
{
    public class AcoesRedis
    {
        private AcoesRepository _repository;

        public AcoesRedis(AcoesRepository repository)
        {
            _repository = repository;
        }

        [FunctionName("AcoesRedis")]
        public IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            string codigo = req.Query["codigo"];
            if (String.IsNullOrWhiteSpace(codigo))
            {
                log.LogError(
                    $"AcoesRedis HTTP trigger V2 - Codigo de Acao nao informado");
                return new BadRequestObjectResult(new
                {
                    Sucesso = false,
                    Mensagem = "Código de Ação não informado"
                });
            }

            log.LogInformation($"AcoesRedis HTTP trigger V2 - codigo da Acao: {codigo}");
            Acao acao = null;
            if (!String.IsNullOrWhiteSpace(codigo))
                acao = _repository.Get(codigo.ToUpper());

            if (acao != null)
            {
                log.LogInformation(
                    $"AcoesRedis HTTP trigger V2 - Acao: {codigo} | Valor atual: {acao.Valor} | Ultima atualizacao: {acao.Data}");
                return new OkObjectResult(acao);
            }
            else
            {
                log.LogError(
                    $"AcoesRedis HTTP trigger V2 - Codigo de Acao nao encontrado: {codigo}");
                return new NotFoundObjectResult(new
                {
                    Sucesso = false,
                    Mensagem = $"Código de Ação não encontrado: {codigo}"
                });
            }
        }
    }
}
