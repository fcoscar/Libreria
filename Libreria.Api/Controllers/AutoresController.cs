using Libreria.Applications.DTOs;
using Libreria.Applications.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Libreria.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AutoresController : ControllerBase
{
    private readonly IAutoresService _autorService;
    private readonly ILogger<AutoresController> _logger;

    public AutoresController(IAutoresService autorService, ILogger<AutoresController> logger)
    {
        _autorService = autorService;
        _logger = logger;
    }
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<AutorDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<AutorDto>>> GetAutorPorId(int id)
    {
        try
        {
            var autor = await _autorService.GetAutorPorIdAsync(id);
            if (autor == null)
            {
                return NotFound(ApiResponse<object>.ErrorResponse(
                    $"Autor con ID {id} no encontrado"));
            }
            return Ok(ApiResponse<AutorDto>.SuccessResponse(autor));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener autor con ID {AutorId}", id);
            return StatusCode(500, ApiResponse<object>.ErrorResponse(
                "Error al procesar la solicitud"));
        }
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<List<AutorDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<List<AutorDto>>>> GetTodosAutores()
    {
        try
        {
            var autores = await _autorService.GetTodosAutoresAsync();
            return Ok(ApiResponse<List<AutorDto>>.SuccessResponse(autores));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener autores");
            return StatusCode(500, ApiResponse<object>.ErrorResponse(
                "Error al procesar la solicitud"));
        }
    }
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<AutorDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<AutorDto>>> CrearAutor([FromBody] CrearAutorDto dto)
    {
        try
        {
            var autor = await _autorService.CrearAutorAsync(dto);
            _logger.LogInformation("Autor creado con ID {AutorId}", autor.Id);
            
            return CreatedAtAction(
                nameof(GetAutorPorId), 
                new { id = autor.Id }, 
                ApiResponse<AutorDto>.SuccessResponse(autor, "Autor creado exitosamente"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear autor");
            return StatusCode(500, ApiResponse<object>.ErrorResponse(
                "Error al procesar la solicitud"));
        }
    }
}