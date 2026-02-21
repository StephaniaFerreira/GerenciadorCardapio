using Microsoft.Extensions.Configuration;
using Xunit;
using MongoDB.Driver;
using System;
using System.Linq;
using GerenciadorCardapio.Services;
using GerenciadorCardapio.Data;
using GerenciadorCardapio.Models;
using GerenciadorCardapio.Configurations;

namespace GerenciadorCardapio.IntegrationTests;

public class ReceitaIntegrationTests : IDisposable
{
    private readonly ReceitaService _service;
    private readonly IMongoCollection<Receita> _collection;

    public ReceitaIntegrationTests()
    {
        var solutionsRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../"));
        var configuration = new ConfigurationBuilder()
            .SetBasePath(solutionsRoot)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        // Lê a seção MongoSettings
        var settings = configuration.GetSection("MongoSettings").Get<MongoSettings>();

        var context = new MongoContext(settings);
        _service = new ReceitaService(context);

        _collection = context.Receitas;

        // limpa antes de cada teste
        _collection.DeleteMany(_ => true);
    }

    [Fact]
    public void Create_E_GetById_DeveFuncionar()
    {
        var receita = new Receita
        {
            Id = Guid.NewGuid(),
            Nome = "Bolo Teste"
        };

        _service.Create(receita);

        var resultado = _service.GetById(receita.Id);

        Assert.NotNull(resultado);
        Assert.Equal("Bolo Teste", resultado.Nome);
    }

    [Fact]
    public void GetAll_DeveRetornarTodasReceitas()
    {
        var receita1 = new Receita
        {
            Id = Guid.NewGuid(),
            Nome = "Bolo"
        };

        var receita2 = new Receita
        {
            Id = Guid.NewGuid(),
            Nome = "Torta"
        };

        _service.Create(receita1);
        _service.Create(receita2);

        var resultado = _service.GetAll();

        Assert.Equal(2, resultado.Count);
        Assert.Contains(resultado, r => r.Nome == "Bolo");
        Assert.Contains(resultado, r => r.Nome == "Torta");
    }

    [Fact]
    public void Update_DeveAlterarReceitaNoBanco()
    {
        var receita = new Receita
        {
            Id = Guid.NewGuid(),
            Nome = "Original"
        };

        _service.Create(receita);

        receita.Nome = "Atualizada";

        _service.Update(receita.Id, receita);

        var resultado = _service.GetById(receita.Id);

        Assert.NotNull(resultado);
        Assert.Equal("Atualizada", resultado.Nome);
    }

    [Fact]
    public void Delete_DeveRemoverDoBanco()
    {
        var receita = new Receita
        {
            Id = Guid.NewGuid(),
            Nome = "Para Deletar"
        };

        _service.Create(receita);

        _service.Delete(receita.Id);

        var resultado = _service.GetById(receita.Id);

        Assert.Null(resultado);
    }

    public void Dispose()
    {
        _collection.DeleteMany(_ => true);
    }
}


#region COMENTÁRIOS
/* IDsposable - liberar os recusos não gerenciados pelo garbage collector 
    - conexão com o banco de dados
    - arquivos abertos
    - streams
    - conexões de rede
    - Handles dos sistema operacional
    - cursores do Mongo
*/
//Implementa a interface e sobreescreve o método DIspose para fechar/excluir tudo que foi feito.

#endregion