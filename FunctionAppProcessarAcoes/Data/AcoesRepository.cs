using System;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using FunctionAppConsultaAcoesRedis.Models;

namespace FunctionAppConsultaAcoesRedis.Data
{
    public class AcoesRepository
    {
        private readonly ConnectionMultiplexer _conexaoRedis;
        private readonly string _prefixoChaveRedis;

        public AcoesRepository(IConfiguration configuration)
        {
            _conexaoRedis = ConnectionMultiplexer.Connect(
                configuration["Redis:Connection"]);
            _prefixoChaveRedis = configuration["Redis:PrefixoChave"];
        }
        public Acao Get(string codigo)
        {
            string strDadosAcao =
                _conexaoRedis.GetDatabase().StringGet(
                    $"{_prefixoChaveRedis}-{codigo}");
            if (!String.IsNullOrWhiteSpace(strDadosAcao))
                return JsonSerializer.Deserialize<Acao>(
                    strDadosAcao,
                    new JsonSerializerOptions()
                    {
                        PropertyNameCaseInsensitive = true
                    });
            else
                return null;
        }        
    }
}