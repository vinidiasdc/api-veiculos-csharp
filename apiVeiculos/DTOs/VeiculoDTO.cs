using apiVeiculos.Entidades;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace apiVeiculos.DTOs
{
    public class VeiculoDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório")]
        [MinLength(3)]
        [MaxLength(130)]
        public string? Nome { get; set; }

        [Required(ErrorMessage = "Marca é obrigatório")]
        [MinLength(3)]
        [MaxLength(130)]
        public string? Marca { get; set; }

        [Required(ErrorMessage = "Ano é obrigatório")]
        public int Ano { get; set; }

        [Required(ErrorMessage = "Velocidade é obrigatório")]
        [Column(TypeName = "decimal(10,2)")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        [DisplayName("Km/h")]
        public double Velocidade { get; set; }

        [DisplayName("Id da Categoria")]
        public int CategoriaId { get; set; }

        [JsonIgnore] //ignora a serialização dos dados de categoria na conversão do JSON
        public Categoria? Categoria { get; set; }
    }
}
