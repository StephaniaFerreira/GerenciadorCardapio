using System.ComponentModel.DataAnnotations;
//Bson - Binary JSON
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using GerenciadorCardapio.Models;
using GerenciadorCardapio.Enum;

namespace GerenciadorCardapio.Models;

public class Receita
{
    [BsonId]
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public string Nome { get; set; }

    public string Descricao { get; set; }

    public int TempoPreparo { get; set; }

    public List<Ingrediente> Ingredientes { get; set; } = new();

    public List<string> ModoPreparo { get; set; } = new();

    public CategoriaReceita Categoria { get; set; }

    public Dificuldade Dificuldade { get; set; }

    [Range(0, 5)]
    public double Avaliacao { get; set; }
}