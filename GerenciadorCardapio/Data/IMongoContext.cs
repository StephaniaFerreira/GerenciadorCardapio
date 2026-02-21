using MongoDB.Driver;
using GerenciadorCardapio.Models;

namespace GerenciadorCardapio.Data;

public interface IMongoContext
{
    IMongoCollection<Receita> Receitas { get; }
    IMongoCollection<Cardapio> Cardapios { get; }
}