using apiVeiculos.DTOs;
using apiVeiculos.Entidades;
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

        [HttpGet("{id:int}", Name = "ConsultarCategoria")]
        public async Task<ActionResult<CategoriaDTO>> ConsultarId(int id) {
            Categoria categoria = await _categoriasRepository.ConsultePorIdAsync(id);

            if (categoria is null)
            {
                return NotFound("Categorias não encontradas!");
            }

            CategoriaDTO categoriaDTO = _mapper.Map<CategoriaDTO>(categoria);

            return Ok(categoriaDTO);
        }

        [HttpPost]
        public async Task<ActionResult> Adicionar([FromBody] CategoriaDTO categoriaDTO) {
            if(categoriaDTO is null) {
                return BadRequest("Dados inválidos!");
            }

            Categoria categoria = _mapper.Map<Categoria>(categoriaDTO);

            await _categoriasRepository.AddAsync(categoria);

            return new CreatedAtRouteResult("ConsultarCategoria", new { id = categoriaDTO.Id }, categoriaDTO);
        }

        [HttpPut]
        public async Task<ActionResult> Editar(int id, [FromBody] CategoriaDTO categoriaDTO) {
            if(id != categoriaDTO.Id)
                return BadRequest();

            if(categoriaDTO is null)
                return BadRequest();

            var categoria = _mapper.Map<Categoria>(categoriaDTO);

            await _categoriasRepository.AtualizeAsync(categoria);

            return Ok(categoriaDTO); 
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Excluir(int id) {
            
            Categoria categoria = await _categoriasRepository.ConsultePorIdAsync(id);

            if(categoria is null) {
                return NotFound("Informe um Id válido");
            }

            await _categoriasRepository.RemovaAsync(id);

            return Ok(categoria);
        }
    }
}
