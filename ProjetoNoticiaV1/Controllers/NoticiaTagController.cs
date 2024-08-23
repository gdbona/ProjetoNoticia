using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjetoNoticiaV1;
using ProjetoNoticiaV1.Models;

namespace ProjetoNoticiaV1.Controllers
{
    public class NoticiaTagController : Controller
    {
        private readonly DbNoticiaContext _context;

        public NoticiaTagController(DbNoticiaContext context)
        {
            _context = context;
        }

        // GET: NoticiaTags
        public async Task<IActionResult> Index()
        {
            var dbNoticiaContext = _context.NoticiaTags.Include(n => n.Noticia).Include(n => n.Tag);
            return View(await dbNoticiaContext.ToListAsync());
        }

        // GET: NoticiaTags/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var noticiaTag = await _context.NoticiaTags
                .Include(n => n.Noticia)
                .Include(n => n.Tag)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (noticiaTag == null)
            {
                return NotFound();
            }

            return View(noticiaTag);
        }

        // GET: NoticiaTags/Create
        public IActionResult Create()
        {
            ViewData["NoticiaId"] = new SelectList(_context.Noticia, "Id", "Id");
            ViewData["TagId"] = new SelectList(_context.Tags, "Id", "Id");
            return View();
        }

        // POST: NoticiaTags/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NoticiaId,TagId")] NoticiaTag noticiaTag)
        {
            if (ModelState.IsValid)
            {
                _context.Add(noticiaTag);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["NoticiaId"] = new SelectList(_context.Noticia, "Id", "Id", noticiaTag.NoticiaId);
            ViewData["TagId"] = new SelectList(_context.Tags, "Id", "Id", noticiaTag.TagId);
            return View(noticiaTag);
        }

        // GET: NoticiaTags/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var noticiaTag = await _context.NoticiaTags.FindAsync(id);
            if (noticiaTag == null)
            {
                return NotFound();
            }
            ViewData["NoticiaId"] = new SelectList(_context.Noticia, "Id", "Id", noticiaTag.NoticiaId);
            ViewData["TagId"] = new SelectList(_context.Tags, "Id", "Id", noticiaTag.TagId);
            return View(noticiaTag);
        }

        // POST: NoticiaTags/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NoticiaId,TagId")] NoticiaTag noticiaTag)
        {
            if (id != noticiaTag.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(noticiaTag);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NoticiaTagExists(noticiaTag.Id))
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
            ViewData["NoticiaId"] = new SelectList(_context.Noticia, "Id", "Id", noticiaTag.NoticiaId);
            ViewData["TagId"] = new SelectList(_context.Tags, "Id", "Id", noticiaTag.TagId);
            return View(noticiaTag);
        }

        // GET: NoticiaTags/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var noticiaTag = await _context.NoticiaTags
                .Include(n => n.Noticia)
                .Include(n => n.Tag)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (noticiaTag == null)
            {
                return NotFound();
            }

            return View(noticiaTag);
        }

        // POST: NoticiaTags/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var noticiaTag = await _context.NoticiaTags.FindAsync(id);
            if (noticiaTag != null)
            {
                _context.NoticiaTags.Remove(noticiaTag);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NoticiaTagExists(int id)
        {
            return _context.NoticiaTags.Any(e => e.Id == id);
        }
    }
}
