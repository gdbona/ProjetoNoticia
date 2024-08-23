using ProjetoNoticiaV1.DTO;
using ProjetoNoticiaV1.Models;

namespace ProjetoNoticiaV1.Interface
{
    public interface INoticia
    {
        Task salvarNoticiaAsync(NoticiaDTO noticia); 

        Task<NoticiaDTO> GetNoticiaById(int id);

        Task<List<NoticiaDTO>> GetNoticia();

        Task DeleteNoticiaById(int id);
    }
}
