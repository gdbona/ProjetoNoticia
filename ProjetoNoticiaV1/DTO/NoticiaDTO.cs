using ProjetoNoticiaV1.Models;
using System.ComponentModel.DataAnnotations;

namespace ProjetoNoticiaV1.DTO
{
    public class NoticiaDTO
    {
        public int Id { get; set; } = 0;
        
        [Required(ErrorMessage = "O campo Texto é  obrigatório", AllowEmptyStrings = false)]
        [MaxLength(250)]
        public string Texto { get; set; } = null!;
        [MaxLength(250)]
        [Required(ErrorMessage = "O campo Titulo é  obrigatório", AllowEmptyStrings = false)]
        public string Titulo { get; set; } = null!;
        
        public string tagId { get; set; } = null!;

        public List<TagDTO>? NoticiaTags { get; set; } = new List<TagDTO>();

        public List<TagDTO>? NoticiaTagsDisponivel { get; set; } = new List<TagDTO>();
    }

}
