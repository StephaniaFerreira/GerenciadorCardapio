using MongoDB.Driver;
using GerenciadorCardapio.Models;
using GerenciadorCardapio.Data;

namespace GerenciadorCardapio.Services;

public class ReceitaService
{
    private readonly IMongoContext _context;

    public ReceitaService(IMongoContext context)
    {
        _context = context;
    }

    //.Find retorna um objeto do Tipo IFindFluent
    public List<Receita> GetAll()
    {
        return _context.Receitas
            .Find(_ => true)
            .ToList();
    }

    public Receita? GetById(Guid id)
    {
        return _context.Receitas
            .Find(r => r.Id == id)
            .FirstOrDefault();
    }

    public void Create(Receita receita)
    {
        _context.Receitas.InsertOne(receita);
    }

    public void Update(Guid id, Receita receita)
    {
        _context.Receitas.ReplaceOne(r => r.Id == id, receita);
    }

    public void Delete(Guid id)
    {
        _context.Receitas.DeleteOne(r => r.Id == id);
    }
}
