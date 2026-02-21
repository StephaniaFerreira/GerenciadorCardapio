using Microsoft.Extensions.Configuration;
using Xunit;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using GerenciadorCardapio.Services;
using GerenciadorCardapio.Data;
using GerenciadorCardapio.Models;
using GerenciadorCardapio.Configurations;

public class CardapioIntegrationTests : IDisposable
{
    private readonly CardapioService _service;
    private readonly IMongoCollection<Cardapio> _collection;

    public CardapioIntegrationTests()
    {
        var solutionsRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../"));
        var configuration = new ConfigurationBuilder()
            .SetBasePath(solutionsRoot)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        // Lê a seção MongoSettings
        var settings = configuration.GetSection("MongoSettings").Get<MongoSettings>();

        var context = new MongoContext(settings);
        _service = new CardapioService(context);

        _collection = context.Cardapios;
        _collection.DeleteMany(_ => true);
    }

    [Fact]
    public void Create_E_GetById_DeveFuncionar()
    {
        var cardapio = new Cardapio
        {
            Id = Guid.NewGuid(),
            DataInicio = DateTime.Today,
            DataFim = DateTime.Today.AddDays(5),
            Receitas = new List<ReceitaRef>
            {
                new ReceitaRef { ReceitaId = Guid.NewGuid(), NomeReceita = "Lasanha" }
            }
        };

        _service.Create(cardapio);

        var resultado = _service.GetById(cardapio.Id);

        Assert.NotNull(resultado);
        Assert.Single(resultado.Receitas);
    }

    [Fact]
    public void GetByPeriodo_DeveRetornarResultado()
    {
        var cardapio = new Cardapio
        {
            Id = Guid.NewGuid(),
            DataInicio = DateTime.Today,
            DataFim = DateTime.Today.AddDays(3)
        };

        _service.Create(cardapio);

        var resultado = _service.GetByPeriodo(
            DateTime.Today,
            DateTime.Today.AddDays(10));

        Assert.Single(resultado);
    }

    [Fact]
    public void GetByNomeReceita_DeveRetornarCardapio()
    {
        var receita = new ReceitaRef { ReceitaId = Guid.NewGuid(), NomeReceita = "Pizza" };
        var cardapio = new Cardapio
        {
            Id = Guid.NewGuid(),
            DataInicio = DateTime.Today,
            DataFim = DateTime.Today.AddDays(5),
            Receitas = new List<ReceitaRef> { receita }
        };

        _service.Create(cardapio);

        var resultado = _service.GetByNomeReceita("pizza");

        Assert.Single(resultado);
        Assert.Equal(cardapio.Id, resultado.First().Id);
    }

     [Fact]
    public void Update_DeveAlterarCardapio()
    {
        var cardapio = new Cardapio
        {
            Id = Guid.NewGuid(),
            DataInicio = DateTime.Today,
            DataFim = DateTime.Today.AddDays(5)
        };

        _service.Create(cardapio);

        // Atualiza
        cardapio.DataFim = DateTime.Today.AddDays(10);
        _service.Update(cardapio.Id, cardapio);

        var resultado = _service.GetById(cardapio.Id);

        Assert.NotNull(resultado);
        Assert.Equal(cardapio.DataFim.Date, resultado.DataFim.ToLocalTime().Date);
    }

    [Fact]
    public void Delete_DeveRemoverCardapio()
    {
        var cardapio = new Cardapio
        {
            Id = Guid.NewGuid(),
            DataInicio = DateTime.Today,
            DataFim = DateTime.Today.AddDays(5)
        };

        _service.Create(cardapio);

        _service.Delete(cardapio.Id);

        var resultado = _service.GetById(cardapio.Id);

        Assert.Null(resultado);
    }

    public void Dispose()
    {
        _collection.DeleteMany(_ => true);
    }
}