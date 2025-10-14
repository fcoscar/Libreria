using Libreria.Api.Controllers;
using Libreria.Applications.DTOs;
using Libreria.Applications.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Microsoft.AspNetCore.Mvc.Core;

namespace Libreria.Test;

public class LibrosControllerTests
{
    private readonly Mock<ILibroService> _mockLibroService;
    private readonly Mock<ILogger<LibrosController>> _mockLogger;
    private readonly LibrosController _controller;
    public LibrosControllerTests()
    {
        _mockLibroService = new Mock<ILibroService>();
        _mockLogger = new Mock<ILogger<LibrosController>>();
        _controller = new LibrosController(_mockLibroService.Object, _mockLogger.Object);
    }
    [Fact]
    public async Task GetLibrosAntesDe2000_ReturnsOkWithLibros()
    {
        var librosEsperados = new List<GetLibroDto>
        {
            new GetLibroDto { Id = 1, Titulo = "Cien Años de Soledad", AnoPublicacion = 1967 },
            new GetLibroDto { Id = 2, Titulo = "Ficciones", AnoPublicacion = 1944 }
        };

        _mockLibroService
            .Setup(s => s.GetLibrosAntesDe2000Async())
            .ReturnsAsync(librosEsperados);

        var result = await _controller.GetLibrosAntesDe2000();

        var okResult = Assert.IsType<ActionResult<ApiResponse<List<GetLibroDto>>>>(result);
        var okObjectResult = Assert.IsType<OkObjectResult>(okResult.Result);
        var apiResponse = Assert.IsType<ApiResponse<List<GetLibroDto>>>(okObjectResult.Value);
        
        Assert.True(apiResponse.Success);
        Assert.NotNull(apiResponse.Data);
        Assert.Equal(2, apiResponse.Data.Count);
        Assert.Equal("Cien Años de Soledad", apiResponse.Data[0].Titulo);
    }

    [Fact]
    public async Task GetLibrosAntesDe2000_ReturnsEmptyList_WhenNoBooks()
    {
        _mockLibroService
            .Setup(s => s.GetLibrosAntesDe2000Async())
            .ReturnsAsync(new List<GetLibroDto>());

        var result = await _controller.GetLibrosAntesDe2000();

        var okResult = Assert.IsType<ActionResult<ApiResponse<List<GetLibroDto>>>>(result);
        var okObjectResult = Assert.IsType<OkObjectResult>(okResult.Result);
        var apiResponse = Assert.IsType<ApiResponse<List<GetLibroDto>>>(okObjectResult.Value);
        
        Assert.True(apiResponse.Success);
        Assert.NotNull(apiResponse.Data);
        Assert.Empty(apiResponse.Data);
    }

    [Fact]
    public async Task GetLibrosAntesDe2000_ReturnsInternalServerError_WhenExceptionThrown()
    {
        _mockLibroService
            .Setup(s => s.GetLibrosAntesDe2000Async())
            .ThrowsAsync(new Exception("Error de base de datos"));

        var result = await _controller.GetLibrosAntesDe2000();

        var statusCodeResult = Assert.IsType<ActionResult<ApiResponse<List<GetLibroDto>>>>(result);
        var objectResult = Assert.IsType<ObjectResult>(statusCodeResult.Result);
        Assert.Equal(500, objectResult.StatusCode);
    }

    [Fact]
    public async Task CrearLibro_ReturnsCreated_WhenValidData()
    {
        var crearDto = new PostLibroDto
        {
            Titulo = "Nuevo Libro",
            AutorId = 1,
            AnoPublicacion = 2020,
            Genero = "Ficción"
        };

        var libroCreado = new GetLibroDto()
        {
            Id = 10,
            Titulo = "Nuevo Libro",
            AnoPublicacion = 2020,
        };

        _mockLibroService
            .Setup(s => s.CrearLibroAsync(It.IsAny<PostLibroDto>()))
            .ReturnsAsync(libroCreado);

        var result = await _controller.CrearLibro(crearDto);

        var createdResult = Assert.IsType<ActionResult<ApiResponse<GetLibroDto>>>(result);
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(createdResult.Result);
        var apiResponse = Assert.IsType<ApiResponse<GetLibroDto>>(createdAtActionResult.Value);
        
        Assert.True(apiResponse.Success);
        Assert.NotNull(apiResponse.Data);
        Assert.Equal(10, apiResponse.Data.Id);
        Assert.Equal("Nuevo Libro", apiResponse.Data.Titulo);
    }

    [Fact]
    public async Task CrearLibro_ReturnsBadRequest_WhenAutorNotExists()
    {
        var crearDto = new PostLibroDto()
        {
            Titulo = "Nuevo Libro",
            AutorId = 999,
            AnoPublicacion = 2020,
            Genero = "Ficción"
        };

        _mockLibroService
            .Setup(s => s.CrearLibroAsync(It.IsAny<PostLibroDto>()))
            .ThrowsAsync(new InvalidOperationException("El autor con ID 999 no existe"));

        var result = await _controller.CrearLibro(crearDto);

        var actionResult = Assert.IsType<ActionResult<ApiResponse<object>>>(result);
        var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result); 
        var apiResponse = Assert.IsType<ApiResponse<object>>(badRequestObjectResult.Value);
        
        Assert.False(apiResponse.Success);
        Assert.Contains("no existe", apiResponse.Message);
    }
}