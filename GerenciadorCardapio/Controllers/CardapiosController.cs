using Microsoft.AspNetCore.Mvc;
using GerenciadorCardapio.Services;

[ApiController]
[Route("api/cardapios")]
public class CardapiosController : ControllerBase
{
    private readonly CardapioService _service;

    public CardapiosController(CardapioService service)
    {
        _service = service;
    }

    [HttpGet("{id}")]
    public IActionResult Get(Guid id)
    {
        var cardapio = _service.GetById(id);
        if (cardapio == null) return NotFound();

        return Ok(cardapio);
    }

    [HttpGet("periodo")]
    public IActionResult GetByPeriodo([FromQuery] DateTime inicio,[FromQuery] DateTime fim)
    {
        var resultado = _service.GetByPeriodo(inicio, fim);
        return Ok(resultado);
    }

    [HttpGet("receita")]
    public IActionResult GetByReceita(
        [FromQuery] string nome)
    {
        var resultado = _service.GetByNomeReceita(nome);
        return Ok(resultado);
    }

    [HttpPost]
    public IActionResult Post(Cardapio cardapio)
    {
        _service.Create(cardapio);
        return Ok(cardapio);
    }

    [HttpPut("{id}")]
    public IActionResult Put(Guid id, Cardapio cardapio)
    {
        _service.Update(id, cardapio);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        _service.Delete(id);
        return NoContent();
    }
}

