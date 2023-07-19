using apiVeiculos.DTOs;
using apiVeiculos.Entidades;
using apiVeiculos.Repositorios;
using apiVeiculos.Repositorios.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace apiVeiculos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly ICategoriaRepository _categoriasRepository;
        private readonly IMapper _mapper;

        public CategoriasController(ICategoriaRepository categoriasRepository, IMapper mapper)
        {
            _categoriasRepository = categoriasRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> ConsulteCategorias()
        {
            IEnumerable<Categoria> categorias = await _categoriasRepository.ConsulteTodosRegistrosAsync();

            if (categorias is null)
            {
                return NotFound("Categorias não encontradas!");
            }

            IEnumerable<CategoriaDTO> categoriasDTO = _mapper.Map<IEnumerable<CategoriaDTO>>(categorias);

            return Ok(categoriasDTO);
        }
    }
}
