using System.ComponentModel.DataAnnotations;

namespace apiVeiculos.DTOs
{
    public class CategoriaDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório")]
        [MinLength(3)]
        [MaxLength(130)]
        public string? Nome { get; set; }
    }
}
