using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using ProjetoNoticiaV1.DTO;
using ProjetoNoticiaV1.Interface;
using ProjetoNoticiaV1.Models;
using System.Linq;

namespace ProjetoNoticiaV1.Service
{
    public class NoticiaService : INoticia
    {
        private readonly DbNoticiaContext _context;

        public NoticiaService(DbNoticiaContext context)
        {
            _context = context;
        }

        public async Task salvarNoticiaAsync(NoticiaDTO noticiaDTO)
        {
            try
            {
                Noticia noticia = new Noticia();
                noticia.Id = noticiaDTO.Id;
                noticia.Texto = noticiaDTO.Texto;
                noticia.Titulo = noticiaDTO.Titulo;
                noticia.UsuarioId = 0;

                await _context.Database.BeginTransactionAsync();

                if (noticia.Id == 0)
                    _context.Add(noticia);
                else
                    _context.Update(noticia);

                await _context.SaveChangesAsync();

                int NoticiaId = noticia.Id;

                if (noticiaDTO.NoticiaTags?.Count() > 0)
                {
                    foreach (var tagsNoticia in noticiaDTO.NoticiaTags)
                    {
                        NoticiaTag noticiaTag = new NoticiaTag();
                        noticiaTag.NoticiaId = NoticiaId;
                        noticiaTag.TagId = tagsNoticia.Id;
                        noticia.NoticiaTags.Add(noticiaTag);
                    }
                }
                await AdicionaListaTagNoticiaAsync(noticia.NoticiaTags, NoticiaId);
                await _context.Database.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await _context.Database.RollbackTransactionAsync();
                throw new Exception(ex.Message);
            }
        }
        public async Task AdicionaListaTagNoticiaAsync(ICollection<NoticiaTag> noticiaTag, int NoticiaId)
        {
            try
            {
                var noticiaUpdate = noticiaTag.ToList();

                var tagsCadastradas = _context.NoticiaTags.Where(s => s.NoticiaId == NoticiaId).ToList();

                //seleciona tags para remover
                var tagsARemover = tagsCadastradas.Where(s => !noticiaUpdate.Contains(s)).ToList();

                if (tagsARemover.Any())
                {
                    _context.RemoveRange(tagsARemover);
                    _context.SaveChanges();
                }

                var tagAAdicionar = noticiaUpdate.Where(s => !tagsCadastradas.Contains(s)).Select(s => new NoticiaTag { NoticiaId = s.NoticiaId, TagId = s.TagId }).ToList();

                if (tagAAdicionar.Any())
                {
                    _context.AddRange(tagAAdicionar);
                    _context.SaveChanges();
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<NoticiaDTO> GetNoticiaById(int id)
        {
            NoticiaDTO noticiaDTO = null;
            Noticia noticia = null;

            try
            {
                noticia = await _context.Noticia.Include(n => n.NoticiaTags).FirstOrDefaultAsync(s => s.Id == id);

                if (noticia != null)
                {
                    noticiaDTO ??= new NoticiaDTO();
                    noticiaDTO.Id = id;
                    noticiaDTO.Texto = noticia.Texto;
                    noticiaDTO.Titulo = noticia.Titulo;

                    if (noticia.NoticiaTags.Count > 0)
                    {
                        var tags = noticia.NoticiaTags.Select(s => s.TagId).ToList();

                        foreach (var item in _context.Tags.Where(w => tags.Contains(w.Id)))
                        {
                            TagDTO tagDTO = new TagDTO();
                            tagDTO.Id = item.Id;
                            tagDTO.Descricao = item.Descricao;

                            noticiaDTO.NoticiaTags.Add(tagDTO);
                        }
                    }
                }
                return noticiaDTO;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<NoticiaDTO>> GetNoticia()
        {
            List<NoticiaDTO> noticiaDTO = new List<NoticiaDTO>();
            List<Noticia> noticia = null;

            try
            {
                noticia = await _context.Noticia.Include(n => n.NoticiaTags).ToListAsync();

                if (noticia != null)
                {
                    foreach (var noticias in noticia)
                    {

                        NoticiaDTO noticiaDB = new NoticiaDTO();
                        noticiaDB.Id = noticias.Id;
                        noticiaDB.Texto = noticias.Texto;
                        noticiaDB.Titulo = noticias.Titulo;

                        if (noticias.NoticiaTags.Count > 0)
                        {
                            var tags = noticias.NoticiaTags.Select(s => s.TagId).ToList();

                            foreach (var item in _context.Tags.Where(w => tags.Contains(w.Id)))
                            {
                                TagDTO tagDTO = new TagDTO();
                                tagDTO.Id = item.Id;
                                tagDTO.Descricao = item.Descricao;

                                noticiaDB.NoticiaTags.Add(tagDTO);
                            }
                        }
                        noticiaDTO.Add(noticiaDB);
                    }

                }
                return noticiaDTO;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task DeleteNoticiaById(int id)
        {
            try
            { 
                var tagsCadastradas = _context.NoticiaTags.Where(s => s.NoticiaId == id).ToList();
 
                if (tagsCadastradas.Any())
                {
                    _context.RemoveRange(tagsCadastradas);
                    _context.SaveChanges();
                }

                var noticiaCadastrada = _context.Noticia.Where(s => s.Id == id).ToList();

                if (noticiaCadastrada.Any())
                {
                    _context.RemoveRange(noticiaCadastrada);
                    _context.SaveChanges();
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
