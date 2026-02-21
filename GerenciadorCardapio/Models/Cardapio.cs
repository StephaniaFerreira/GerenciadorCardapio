using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Cardapio
{
    [BsonId]
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid Id { get; set; } = Guid.NewGuid();

    public TipoPeriodo Periodo { get; set; }

    public DateTime DataInicio { get; set; }

    public DateTime DataFim { get; set; }

    public List<ReceitaRef> Receitas { get; set; } = new();
}


public class ReceitaRef
{
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid ReceitaId { get; set; }

    public string NomeReceita { get; set; } = string.Empty;
}