using Moq;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using GerenciadorCardapio.Services;
using GerenciadorCardapio.Models;
using GerenciadorCardapio.Data;

namespace GerenciadorCardapioUnitTest.Tests;

public class ReceitaServiceTests
{
    //representa a colletion
    private readonly Mock<IMongoCollection<Receita>> _mockCollection;
    private readonly Mock<IMongoContext> _mockContext;
    private readonly ReceitaService _service;

    public ReceitaServiceTests()
    {
        _mockCollection = new Mock<IMongoCollection<Receita>>();

        //criar um mock do MongoCOntext
        //quando service chamar _context.Receita retorna o objeto Mock
        _mockContext = new Mock<IMongoContext>();
        _mockContext.Setup(c => c.Receitas)
                    .Returns(_mockCollection.Object);

        _service = new ReceitaService(_mockContext.Object);
    }

    [Fact]
    public void GetAll_DeveRetornarListaDeReceitas()
    {
        // Arrange
        var receitas = new List<Receita>
        {
            new Receita { Id = Guid.NewGuid(), Nome = "Bolo" },
            new Receita { Id = Guid.NewGuid(), Nome = "Torta" }
        };
        //retorno do Find
        var mockCursor = new Mock<IAsyncCursor<Receita>>();

        mockCursor.SetupSequence(_ => _.MoveNext(It.IsAny<CancellationToken>()))
            .Returns(true)
            .Returns(false);

        mockCursor.Setup(_ => _.Current)
            .Returns(receitas);
        
        _mockCollection.Setup(c => c.FindSync(
                            It.IsAny<FilterDefinition<Receita>>(),
                            It.IsAny<FindOptions<Receita, Receita>>(),
                            It.IsAny<CancellationToken>()))
                        .Returns(mockCursor.Object);

        // Act
        var resultado = _service.GetAll();

        // Assert
        Assert.Equal(2, resultado.Count);
    }

    [Fact]
    public void GetById_DeveRetornarReceitaQuandoExistir()
    {
        // Arrange
        var id = Guid.NewGuid();
        var receita = new Receita { Id = id, Nome = "Pizza" };

        var mockCursor = new Mock<IAsyncCursor<Receita>>();

        //Faz duas consultas ao cursor, na primeira retorna - existe um lote disponivel
        // Na segunda - acabou
        mockCursor.SetupSequence(_ => _.MoveNext(It.IsAny<CancellationToken>()))
            .Returns(true)
            .Returns(false);

        mockCursor.Setup(_ => _.Current)
            .Returns(new List<Receita> { receita });

        _mockCollection
            .Setup(c => c.FindSync(
                It.IsAny<FilterDefinition<Receita>>(),
                It.IsAny<FindOptions<Receita, Receita>>(),
                It.IsAny<CancellationToken>()))
            .Returns(mockCursor.Object);

        // Act
        var resultado = _service.GetById(id);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal("Pizza", resultado.Nome);
    }

    [Fact]
    public void Create_DeveInserirReceita()
    {
        // Arrange
        var receita = new Receita { Id = Guid.NewGuid(), Nome = "Lasanha" };

        // Act
        _service.Create(receita);

        // Assert
        _mockCollection.Verify(c =>
            c.InsertOne(receita,
                        null,
                        default),
            Times.Once);
    }

    [Fact]
    public void Update_DeveSubstituirReceita()
    {
        // Arrange
        var id = Guid.NewGuid();
        var receita = new Receita { Id = id, Nome = "Atualizada" };

        // Act
        _service.Update(id, receita);

        // Assert
        _mockCollection.Verify(c =>
                            c.ReplaceOne(
                                It.IsAny<FilterDefinition<Receita>>(),
                                receita,
                                It.IsAny<ReplaceOptions>(),
                                It.IsAny<CancellationToken>()),
                            Times.Once);
    }

    [Fact]
    public void Delete_DeveRemoverReceita()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        _service.Delete(id);

        // Assert
        _mockCollection.Verify(c =>
            c.DeleteOne(
                It.IsAny<FilterDefinition<Receita>>(),
                default),
            Times.Once);
    }
}


#region COMENTÁRIOS
//.setup - configura o comportamento do MOCK. Coloque dentro a função que terá o comportamento alterado
//.returns - especifica qual mock deverá retornar, quando a função alterada for chamada
// os métodos retornando um cursor (ponteiro de leitura)
// cursor.Current - retorna o lote atual de documentos
//SetupSequence - chamadas consecutivas ao cursor
/* O MOQ só consegue mockar 
    - Interfaces -  pode criar uma implementação falsa
    - Métodos virtuais -  pode sobresecrever o método
    - Métodos de classes abstratas
*/
//.Verify - verifica se o método foi chamado
#endregion