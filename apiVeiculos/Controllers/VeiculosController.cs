using apiVeiculos.DTOs;
using apiVeiculos.Entidades;
using apiVeiculos.Repositorios.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace apiVeiculos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VeiculosController : ControllerBase
    {
        private readonly IVeiculoRepository _veiculosRepository;
        private readonly IMapper _mapper;

        public VeiculosController(IVeiculoRepository veiculosRepository, IMapper mapper)
        {
            _veiculosRepository = veiculosRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<VeiculoDTO>>> ConsultarTodos()
        {
            IEnumerable<Veiculo> veiculos = await _veiculosRepository.ConsulteTodosRegistrosAsync();

            if(veiculos is null)
            {
                return NotFound("Nenhum registro de veiculos");
            }

            IEnumerable<VeiculoDTO> veiculosDto = _mapper.Map<IEnumerable<VeiculoDTO>>(veiculos);

            return Ok(veiculosDto);
        }

        [HttpGet("{id:int}", Name = "ConsultarVeiculo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<VeiculoDTO>> Consulte(int id) {
            Veiculo veiculo = await _veiculosRepository.ConsultePorIdAsync(id);

            if(veiculo is null) {
                return NotFound($"Veiculo com id {id} não foi encontrado!");
            }

            VeiculoDTO veiculoDto = _mapper.Map<VeiculoDTO>(veiculo);

            return Ok(veiculoDto);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Editar(int id, [FromBody] VeiculoDTO veiculoDTO) {
            if(id != veiculoDTO.Id)
                return BadRequest();

            if(veiculoDTO is null)
                return BadRequest();

            var veiculo = _mapper.Map<Veiculo>(veiculoDTO);

            await _veiculosRepository.AtualizeAsync(veiculo);

            return Ok(veiculoDTO); 
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult> Adicionar([FromBody] VeiculoDTO veiculoDTO) {
            if(veiculoDTO is null) {
                return BadRequest("Preencha os campos");
            }

            Veiculo veiculo = _mapper.Map<Veiculo>(veiculoDTO);

            await _veiculosRepository.AddAsync(veiculo);

            return new CreatedAtRouteResult("ConsultarVeiculo", new { id = veiculoDTO.Id }, veiculoDTO);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Excluir(int id) {
            Veiculo veiculo = await _veiculosRepository.ConsultePorIdAsync(id);

            if(veiculo is null) {
                return NotFound("Informe um id válido!");
            }

            await _veiculosRepository.RemovaAsync(id);

            return Ok();
        }

        [HttpGet]
        [Route("ConsulteVeiculosDeCategoria/{categoriaId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ConsulteVeiculosDeCategoria(int categoriaId) {
            IEnumerable<Veiculo> veiculos = await _veiculosRepository.ConsulteVeiculoPorCategoriaAsync(categoriaId);

            if(veiculos is null) {
                return NotFound($"Nenhum veículo encontrado para a categoria de {categoriaId}");
            }

            IEnumerable<VeiculoDTO> veiculoDtos = _mapper.Map<IEnumerable<VeiculoDTO>>(veiculos);

            return Ok(veiculoDtos);
        }

        [HttpGet]
        [Route("ConsultePorNome/{textoNome}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ConsultePorNome(string textoNome) {
            IEnumerable<Veiculo> veiculosEncontrados = await _veiculosRepository.ConsulteAsync(v => v.Nome.Contains(textoNome));

            if(veiculosEncontrados is null) {
                return NotFound("Nenhum registro encontrado com este nome");
            }

            IEnumerable<VeiculoDTO> veiculoDtos = _mapper.Map<IEnumerable<VeiculoDTO>>(veiculosEncontrados);

            return Ok(veiculoDtos);
        }

        [HttpGet]
        [Route("ConsulteVeiculoComCategoria/{termoPesquisa}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<VeiculoCategoriaDTO>>> ConsulteVeiculoComCategoria(string termoPesquisa) {
            IEnumerable<Veiculo> veiculosEncontrados = await _veiculosRepository.ConsulteVeiculoComCategoriaAsync(termoPesquisa);

            if(!veiculosEncontrados.Any()) {
                return NotFound($"Nenhum veiculo com nome/marca {termoPesquisa} encontrado!");
            }

            IEnumerable<VeiculoCategoriaDTO> veiculosDtos = _mapper.Map<IEnumerable<VeiculoCategoriaDTO>>(veiculosEncontrados);

            return Ok(veiculosDtos);
        }
    }
}