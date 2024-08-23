using ProjetoNoticiaV1.Models;
using System.ComponentModel.DataAnnotations;

namespace ProjetoNoticiaV1.DTO
{
    public class UsuarioDTO
    {
        public int? Id { get; set; }
        [Required]
        [Display(Name = "Nome")]
        public string Nome { get; set; } = null!;
        [Required]
        [Display(Name = "Senha")]
        public string Senha { get; set; } = null!;
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = null!;
         
    }
}
