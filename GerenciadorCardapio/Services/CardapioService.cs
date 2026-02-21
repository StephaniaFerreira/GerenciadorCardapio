using MongoDB.Driver;
using GerenciadorCardapio.Models;
using GerenciadorCardapio.Data;

namespace GerenciadorCardapio.Services;

public class CardapioService
{
    private readonly IMongoCollection<Cardapio> _collection;

    public CardapioService(IMongoContext context)
    {
        _collection = context.Cardapios;
    }

    public Cardapio? GetById(Guid id)
    {
        return _collection
            .Find(c => c.Id == id)
            .FirstOrDefault();
    }

    public List<Cardapio> GetByPeriodo(DateTime inicio, DateTime fim)
    {
        var filtro = Builders<Cardapio>.Filter.And(
            Builders<Cardapio>.Filter.Gte(c => c.DataInicio, inicio),
            Builders<Cardapio>.Filter.Lte(c => c.DataFim, fim)
        );
    
        return _collection.Find(filtro).ToList();
    }
    
    public List<Cardapio> GetByNomeReceita(string nomeReceita)
    {
        var filtro = Builders<Cardapio>.Filter.ElemMatch(
            c => c.Receitas,
            r => r.NomeReceita.ToLower().Contains(nomeReceita.ToLower())
        );
    
        return _collection.Find(filtro).ToList();
    }

    public void Create(Cardapio cardapio)
    {
        ValidarPeriodo(cardapio);
        _collection.InsertOne(cardapio);
    }

    public void Update(Guid id, Cardapio cardapio)
    {
        ValidarPeriodo(cardapio);
        _collection.ReplaceOne(c => c.Id == id, cardapio);
    }

    public void Delete(Guid id)
    {
        _collection.DeleteOne(c => c.Id == id);
    }

    private void ValidarPeriodo(Cardapio c)
    {
        if (c.DataFim <= c.DataInicio)
            throw new Exception("Período inválido");
    }
}
