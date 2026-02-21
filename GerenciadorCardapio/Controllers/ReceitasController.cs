using Microsoft.AspNetCore.Mvc;
using GerenciadorCardapio.Models;
using GerenciadorCardapio.Services;

namespace GerenciadorCardapio.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReceitasController : ControllerBase
{
    private readonly ReceitaService _service;

    public ReceitasController(ReceitaService service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult Get()
    {
        var receitas = _service.GetAll();
        return Ok(receitas);
    }

    [HttpGet("{id}")]
    public IActionResult Get(Guid id)
    {
        var receita = _service.GetById(id);

        if (receita == null)
            return NotFound();

        return Ok(receita);
    }

    [HttpPost]
    public IActionResult Post([FromBody] Receita receita)
    {
        _service.Create(receita);

        return CreatedAtAction(
            nameof(Get),
            new { id = receita.Id },
            receita
        );
    }

    [HttpPut("{id}")]
    public IActionResult Put(Guid id, [FromBody] Receita receita)
    {
        var existente = _service.GetById(id);

        if (existente == null)
            return NotFound();

        _service.Update(id, receita);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        var existente = _service.GetById(id);

        if (existente == null)
            return NotFound();

        _service.Delete(id);

        return NoContent();
    }
}