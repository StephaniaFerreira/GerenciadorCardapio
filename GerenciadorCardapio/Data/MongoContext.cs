using MongoDB.Driver;
using GerenciadorCardapio.Configurations;
using GerenciadorCardapio.Models;

namespace GerenciadorCardapio.Data;

public class MongoContext : IMongoContext
{
    private readonly IMongoDatabase _database;

    public MongoContext(MongoSettings settings)
    {
        var client = new MongoClient(settings.ConnectionString);
        _database = client.GetDatabase(settings.DatabaseName);
        CreateIndexes();
    }

    public IMongoCollection<Receita> Receitas =>
        _database.GetCollection<Receita>("receitas");

    public IMongoCollection<Cardapio> Cardapios =>
        _database.GetCollection<Cardapio>("cardapios");

    private void CreateIndexes()
    {
        var collection = _database.GetCollection<Cardapio>("cardapios");
        var indexKeysReceita = Builders<Cardapio>.IndexKeys
            .Ascending("Receitas.NomeReceita"); // caminho exato no documento Mongo

        var indexOptionsReceita = new CreateIndexOptions
        {
            Name = "idx_cardapio_nome_receita"
        };

        collection.Indexes.CreateOne(new CreateIndexModel<Cardapio>(indexKeysReceita, indexOptionsReceita));

        var indexKeysPeriodo = Builders<Cardapio>.IndexKeys
            .Ascending(c => c.DataInicio)
            .Ascending(c => c.DataFim);

        var indexOptionsPeriodo = new CreateIndexOptions
        {
            Name = "idx_cardapio_periodo"
        };

        collection.Indexes.CreateOne(new CreateIndexModel<Cardapio>(indexKeysPeriodo, indexOptionsPeriodo));
    }

}
