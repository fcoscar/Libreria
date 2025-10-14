using Libreria.Applications.DTOs;
using Libreria.Applications.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Libreria.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class PrestamosController : ControllerBase
{
    private readonly ILogger<PrestamosController> _logger;
    private readonly IPrestamoService _prestamoService;

    public PrestamosController(IPrestamoService prestamoService, ILogger<PrestamosController> logger)
    {
        _prestamoService = prestamoService;
        _logger = logger;
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<GetPrestamoDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<GetPrestamoDto>>> GetPrestamoPorId(int id)
    {
        try
        {
            var prestamo = await _prestamoService.GetPrestamoByIdAsync(id);
            if (prestamo == null)
            {
                return NotFound(ApiResponse<object>.ErrorResponse(
                    $"Préstamo con ID {id} no encontrado"));
            }
            return Ok(ApiResponse<GetPrestamoDto>.SuccessResponse(prestamo));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener préstamo con ID {PrestamoId}", id);
            return StatusCode(500, ApiResponse<object>.ErrorResponse(
                "Error al procesar la solicitud"));
        }
    }
    
    [HttpGet("no-devueltos")]
    [ProducesResponseType(typeof(ApiResponse<List<GetPrestamoDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<List<GetPrestamoDto>>>> GetPrestamosNoDevueltos()
    {
        try
        {
            _logger.LogInformation("Obteniendo prestamos no devueltos");
            var prestamos = await _prestamoService.GetPrestamosNoDevueltosAsync();
            return Ok(ApiResponse<List<GetPrestamoDto>>.SuccessResponse(
                prestamos, 
                $"Se encontraron {prestamos.Count} prestamos no devueltos"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener prestamos no devueltos");
            return StatusCode(500, ApiResponse<object>.ErrorResponse(
                "Error al procesar la solicitud"));
        }
    }
    
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> EliminarPrestamo(int id)
    {
        try
        {
            var eliminado = await _prestamoService.EliminarPrestamoAsync(id);
            if (!eliminado)
            {
                return NotFound(ApiResponse<object>.ErrorResponse(
                    $"Préstamo con ID {id} no encontrado"));
            }
            
            _logger.LogInformation("Préstamo eliminado con ID {PrestamoId}", id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar préstamo con ID {PrestamoId}", id);
            return StatusCode(500, ApiResponse<object>.ErrorResponse(
                "Error al procesar la solicitud"));
        }
    }
    
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> ActualizarPrestamo(
        int id, 
        [FromBody] ActualizarFechaDevDto dto)
    {
        try
        {
            var actualizado = await _prestamoService.ActualizarFechaDevPrestamoAsync(id, dto);
            if (!actualizado)
            {
                return NotFound(ApiResponse<object>.ErrorResponse(
                    $"Préstamo con ID {id} no encontrado"));
            }
            
            _logger.LogInformation("Préstamo actualizado con ID {PrestamoId}", id);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar préstamo con ID {PrestamoId}", id);
            return StatusCode(500, ApiResponse<object>.ErrorResponse(
                "Error al procesar la solicitud"));
        }
    }
    
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse<PostPrestamoDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<PostPrestamoDto>>> CrearPrestamo(
        [FromBody] PostPrestamoDto dto)
    {
        try
        {
            var prestamo = await _prestamoService.CrearPrestamoAsync(dto);
            _logger.LogInformation("Préstamo creado con ID {PrestamoId}", prestamo.Id);
            
            return CreatedAtAction(
                nameof(GetPrestamoPorId), 
                new { id = prestamo.Id }, 
                ApiResponse<GetPrestamoDto>.SuccessResponse(
                    prestamo, "Préstamo creado exitosamente"));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear préstamo");
            return StatusCode(500, ApiResponse<object>.ErrorResponse(
                "Error al procesar la solicitud"));
        }
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<List<GetPrestamoDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<List<GetPrestamoDto>>>> GetTodosPrestamos()
    {
        try
        {
            var prestamos = await _prestamoService.GetTodosPrestamosAsync();
            return Ok(ApiResponse<List<GetPrestamoDto>>.SuccessResponse(prestamos));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener todos los préstamos");
            return StatusCode(500, ApiResponse<object>.ErrorResponse(
                "Error al procesar la solicitud"));
        }
    }
}