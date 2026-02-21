using Moq;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading;
using GerenciadorCardapio.Services;
using GerenciadorCardapio.Models;
using GerenciadorCardapio.Data;

namespace GerenciadorCardapioUnitTest.Tests;

public class CardapioServiceTests
{
    private readonly Mock<IMongoCollection<Cardapio>> _mockCollection;
    private readonly Mock<IMongoContext> _mockContext;
    private readonly CardapioService _service;

    public CardapioServiceTests()
    {
        _mockCollection = new Mock<IMongoCollection<Cardapio>>();
        _mockContext = new Mock<IMongoContext>();

        _mockContext
            .Setup(c => c.Cardapios)
            .Returns(_mockCollection.Object);

        _service = new CardapioService(_mockContext.Object);
    }

    // ==============================
    // GET BY ID
    // ==============================

    [Fact]
    public void GetById_DeveRetornarCardapio()
    {
        var id = Guid.NewGuid();

        var cardapio = new Cardapio
        {
            Id = id,
            DataInicio = DateTime.Today,
            DataFim = DateTime.Today.AddDays(5)
        };

        var mockCursor = new Mock<IAsyncCursor<Cardapio>>();

        mockCursor
            .SetupSequence(c => c.MoveNext(It.IsAny<CancellationToken>()))
            .Returns(true)
            .Returns(false);

        mockCursor
            .Setup(c => c.Current)
            .Returns(new List<Cardapio> { cardapio });

        _mockCollection
            .Setup(c => c.FindSync(
                It.IsAny<FilterDefinition<Cardapio>>(),
                It.IsAny<FindOptions<Cardapio, Cardapio>>(),
                It.IsAny<CancellationToken>()))
            .Returns(mockCursor.Object);

        var resultado = _service.GetById(id);

        Assert.NotNull(resultado);
        Assert.Equal(id, resultado.Id);
    }

    // ==============================
    // GET BY PERIODO
    // ==============================

    [Fact]
    public void GetByPeriodo_DeveRetornarLista()
    {
        var lista = new List<Cardapio>
        {
            new Cardapio
            {
                Id = Guid.NewGuid(),
                DataInicio = DateTime.Today,
                DataFim = DateTime.Today.AddDays(2)
            }
        };

        var mockCursor = new Mock<IAsyncCursor<Cardapio>>();

        mockCursor
            .SetupSequence(c => c.MoveNext(It.IsAny<CancellationToken>()))
            .Returns(true)
            .Returns(false);

        mockCursor
            .Setup(c => c.Current)
            .Returns(lista);

        _mockCollection
            .Setup(c => c.FindSync(
                It.IsAny<FilterDefinition<Cardapio>>(),
                It.IsAny<FindOptions<Cardapio, Cardapio>>(),
                It.IsAny<CancellationToken>()))
            .Returns(mockCursor.Object);

        var resultado = _service.GetByPeriodo(DateTime.Today, DateTime.Today.AddDays(5));

        Assert.Single(resultado);
    }

    // ==============================
    // CREATE
    // ==============================

    [Fact]
    public void Create_DeveInserirCardapio()
    {
        var cardapio = new Cardapio
        {
            Id = Guid.NewGuid(),
            DataInicio = DateTime.Today,
            DataFim = DateTime.Today.AddDays(5)
        };

        _service.Create(cardapio);

        _mockCollection.Verify(c =>
            c.InsertOne(
                cardapio,
                It.IsAny<InsertOneOptions>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    // ==============================
    // UPDATE
    // ==============================

    [Fact]
    public void Update_DeveSubstituirCardapio()
    {
        var id = Guid.NewGuid();

        var cardapio = new Cardapio
        {
            Id = id,
            DataInicio = DateTime.Today,
            DataFim = DateTime.Today.AddDays(5)
        };

        _service.Update(id, cardapio);

        _mockCollection.Verify(c =>
            c.ReplaceOne(
                It.IsAny<FilterDefinition<Cardapio>>(),
                cardapio,
                It.IsAny<ReplaceOptions>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    // ==============================
    // DELETE
    // ==============================

    [Fact]
    public void Delete_DeveRemoverCardapio()
    {
        var id = Guid.NewGuid();

        _service.Delete(id);

        _mockCollection.Verify(c =>
            c.DeleteOne(
                It.IsAny<FilterDefinition<Cardapio>>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    // ==============================
    // VALIDAR PERIODO INVALIDO
    // ==============================

    [Fact]
    public void Create_DeveLancarExcecao_QuandoPeriodoInvalido()
    {
        var cardapio = new Cardapio
        {
            Id = Guid.NewGuid(),
            DataInicio = DateTime.Today,
            DataFim = DateTime.Today.AddDays(-1)
        };

        Assert.Throws<Exception>(() => _service.Create(cardapio));
    }
}