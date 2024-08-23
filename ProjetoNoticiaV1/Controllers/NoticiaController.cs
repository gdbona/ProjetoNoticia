using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using ProjetoNoticiaV1;
using ProjetoNoticiaV1.DTO;
using ProjetoNoticiaV1.Interface;
using ProjetoNoticiaV1.Models;

namespace ProjetoNoticiaV1.Controllers
{
    public class NoticiaController : Controller
    {
        private readonly DbNoticiaContext _context;
        private readonly INoticia _Noticia;

        public NoticiaController(DbNoticiaContext context, INoticia noticia)
        {
            _context = context;
            _Noticia = noticia;
        }

        // GET: Noticia
        public async Task<IActionResult> Index()
        {
            var dbNoticiaContext = _context.Noticia.Include(n => n.Usuario);
            return View(await dbNoticiaContext.ToListAsync());
        }

        public async Task<IActionResult> getNoticias()
        {
            return PartialView(await _Noticia.GetNoticia());
        }

        // GET: Noticia/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var noticia = await _context.Noticia
                .Include(n => n.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (noticia == null)
            {
                return NotFound();
            }

            return View(noticia);
        }

        // GET: Noticia/Create
        public IActionResult Create()
        {
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Id");

            //var model = new NoticiaDTO(); 

            return PartialView();
        }

        // POST: Noticia/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NoticiaDTO noticia)
        {
            ModelState.Remove("NoticiaTags");
            ModelState.Remove("NoticiaTagsDisponivel");
            ModelState.Remove("tagId");

            if (ModelState.IsValid)
            {
                await _Noticia.salvarNoticiaAsync(noticia);
                return RedirectToAction(nameof(Index));
            }
            //ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Id", noticia.UsuarioId);
            return View(noticia);
        }

        // GET: Noticia/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var noticia = await _Noticia.GetNoticiaById(Convert.ToInt32(id));
            if (noticia == null)
            {
                return NotFound();
            }
            //ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Id", noticia.UsuarioId);
            return View(noticia);
        }

        // POST: Noticia/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, NoticiaDTO noticia)
        {
            if (id != noticia.Id)
            {
                return NotFound();
            }

            ModelState.Remove("tagId");

            if (ModelState.IsValid)
            {
                try
                {
                    await _Noticia.salvarNoticiaAsync(noticia);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NoticiaExists(noticia.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            return View(noticia);
        }

        // GET: Noticia/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var noticia = await _context.Noticia
                .Include(n => n.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (noticia == null)
            {
                return NotFound();
            }

            return View(noticia);
        }

        // POST: Noticia/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var noticia = await _context.Noticia.FindAsync(id);
            if (noticia != null)
            {
                _Noticia.DeleteNoticiaById(id);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool NoticiaExists(int id)
        {
            return _context.Noticia.Any(e => e.Id == id);
        }

        public async Task<JsonResult> ListaTags()
        {
            return Json(await _context.Tags.ToArrayAsync());
        }

        [HttpPost]
        public async Task<IActionResult> _TagsDisponiveis(NoticiaDTO model)
        {
            List<TagDTO> tagDto = new List<TagDTO>();
            foreach (var item in await _context.Tags.ToListAsync())
            {
                tagDto.Add(new TagDTO() { Id = item.Id, Descricao = item.Descricao });
            }
            model.NoticiaTagsDisponivel.Clear();

            var tagDisponivel = tagDto.Where(ms => !model.NoticiaTags.Any(p => p.Id == ms.Id)).ToList();

            if (tagDisponivel != null && tagDisponivel.Count > 0)
                model.NoticiaTagsDisponivel.AddRange(tagDisponivel);

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> AdicionaOuRemoveTagNoticia([Bind("NoticiaTags,NoticiaTagsDisponivel,tagId")] NoticiaDTO model)
        {
            try
            {
                if (!String.IsNullOrEmpty(model.tagId))
                    if (model.NoticiaTags.FirstOrDefault(s => s.Id == Convert.ToInt32(model.tagId)) != null)
                        model.NoticiaTags.RemoveAll(s => s.Id == Convert.ToInt32(model.tagId));
                    else
                        model.NoticiaTags.Add(model.NoticiaTagsDisponivel.FirstOrDefault(s => s.Id == Convert.ToInt32(model.tagId)));

                return PartialView("_TagsAdicionadas", model);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IActionResult> pageEdit(string id)
        {
            if (!String.IsNullOrEmpty(id))
                return View(new NoticiaDTO() { Id = Convert.ToInt32(id) });
            else
                return View(new NoticiaDTO());
        }
    }
}
