using Libreria.Applications.DTOs;
using Libreria.Applications.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Libreria.Api.Controllers;


[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class LibrosController : ControllerBase
{
    private readonly ILogger<LibrosController> _logger;
    private readonly ILibroService _libroService;

    public LibrosController(ILibroService libroService, ILogger<LibrosController> logger)
    {
        _libroService = libroService;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<List<GetLibroDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<List<GetLibroDto>>>> GetTodosLibros()
    {
        try
        {
            var libros = await _libroService.GetTodosLibrosAsync();
            return Ok(ApiResponse<List<GetLibroDto>>.SuccessResponse(libros));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener todos los libros");
            return StatusCode(500, ApiResponse<object>.ErrorResponse(
                "Error al procesar la solicitud"));
        }
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<GetLibroDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<GetLibroDto>>> GetLibroPorId(int id)
    {
        try
        {
            var libro = await _libroService.GetLibroPorIdAsync(id);
            if (libro == null)
            {
                return NotFound(ApiResponse<object>.ErrorResponse(
                    $"Libro con ID {id} no encontrado"));
            }
            return Ok(ApiResponse<GetLibroDto>.SuccessResponse(libro));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener libro con ID {LibroId}", id);
            return StatusCode(500, ApiResponse<object>.ErrorResponse(
                "Error al procesar la solicitud"));
        }
    }
    
    [HttpGet("antes-de-2000")]
    [ProducesResponseType(typeof(ApiResponse<List<GetLibroDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<List<GetLibroDto>>>> GetLibrosAntesDe2000()
    {
        try
        {
            _logger.LogInformation("Obteniendo libros publicados antes del año 2000");
            var libros = await _libroService.GetLibrosAntesDe2000Async();
            return Ok(ApiResponse<List<GetLibroDto>>.SuccessResponse(
                libros, 
                $"Se encontraron {libros.Count} libros"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener libros antes de 2000");
            return StatusCode(500, ApiResponse<object>.ErrorResponse(
                "Error al procesar la solicitud", 
                new List<string> { ex.Message }));
        }
    }
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse<GetLibroDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<GetLibroDto>>> CrearLibro([FromBody] PostLibroDto dto)
    {
        try
        {
            var libro = await _libroService.CrearLibroAsync(dto);
            _logger.LogInformation("Libro creado con ID {LibroId}", libro.Id);
            
            return CreatedAtAction(
                nameof(GetLibroPorId), 
                new { id = libro.Id }, 
                ApiResponse<GetLibroDto>.SuccessResponse(libro, "Libro creado exitosamente"));
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Error de validación al crear libro");
            return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear libro");
            return StatusCode(500, ApiResponse<object>.ErrorResponse(
                "Error al procesar la solicitud"));
        }
    }
    
    [HttpGet("busqueda-avanzada")]
    [ProducesResponseType(typeof(ApiResponse<List<GetLibroDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<List<GetLibroDto>>>> BusquedaAvanzada([FromQuery] BusquedaAvanzadaDto filtros)
    {
        try
        {
            var resultado = await _libroService.BusquedaAvanzadaAsync(filtros);
            return Ok(ApiResponse<List<GetLibroDto>>.SuccessResponse(resultado));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en búsqueda de libros por filtros");
            return StatusCode(500, ApiResponse<object>.ErrorResponse(
                "Error al procesar la solicitud"));
        }
    }
    
    [HttpGet("populares")]
    [ProducesResponseType(typeof(ApiResponse<List<GetLibroDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<List<GetLibroDto>>>> LibrosPopulares([FromQuery] BusquedaAvanzadaDto filtros)
    {
        try
        {
            var resultado = await _libroService.BusquedaAvanzadaAsync(filtros);
            return Ok(ApiResponse<List<GetLibroDto>>.SuccessResponse(resultado));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en búsqueda de libros por filtros");
            return StatusCode(500, ApiResponse<object>.ErrorResponse(
                "Error al procesar la solicitud"));
        }
    }
}