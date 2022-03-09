using crudcore.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace crudcore.Controller
{
    public class BooksController : Microsoft.AspNetCore.Mvc.Controller
    {
        public ApplicationDbContext _applicationDb;
        IWebHostEnvironment _webHostEnvironment;

        public BooksController(ApplicationDbContext applicationDbContext, IWebHostEnvironment webHostEnvironment)
        {
            _applicationDb = applicationDbContext;
            _webHostEnvironment = webHostEnvironment;
        }


        public async Task<IActionResult> Index()
        {
            return View(_applicationDb.Books.AsEnumerable());
        }





        public IActionResult Create()
        {
            ViewData["PublicationID_FK"] = new SelectList(_applicationDb.Publications, "PublicationID", "PublicationName");
            return View();
        }


        [HttpPost]

        public async Task<IActionResult> Create(Book book)
        {
            if (ModelState.IsValid)
            {
                string uploads = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                if (book.Image.Length > 0)
                {
                    string filePath = Path.Combine(uploads, book.Image.FileName);
                    using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await book.Image.CopyToAsync(fileStream);
                        book.ImagePath = book.Image.FileName;
                    }
                }

                _applicationDb.Add(book);
                await _applicationDb.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PublicationID_FK"] = new SelectList(_applicationDb.Publications, "PublicationID", "PublicationName", book.PublicationID_FK);
            return View(book);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _applicationDb.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            ViewData["PublicationID_FK"] = new SelectList(_applicationDb.Publications, "PublicationID", "PublicationName", book.PublicationID_FK);
            return View(book);
        }


        [HttpPost]

        public async Task<IActionResult> Edit(int id, Book book)
        {
            if (id != book.BookID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                string uploads = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                if (book.Image.Length > 0)
                {
                    string filePath = Path.Combine(uploads, book.Image.FileName);
                    using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await book.Image.CopyToAsync(fileStream);
                        book.ImagePath = book.Image.FileName;
                    }
                }

                try
                {
                    _applicationDb.Update(book);
                    await _applicationDb.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.BookID))
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
            ViewData["PublicationID_FK"] = new SelectList(_applicationDb.Publications, "PublicationID", "PublicationIDName", book.PublicationID_FK);
            return View(book);
        }

        public ActionResult Delete(int? id)

        {

            try

            {

                var firstEntity = _applicationDb.Books.Where(c => c.BookID == id).FirstOrDefault();

                _applicationDb.Books.Remove(firstEntity);

                _applicationDb.SaveChanges();

            }

            finally

            {



            }

            return RedirectToAction("Index");

        }


        private bool BookExists(int id)
        {
            return _applicationDb.Books.Any(b => b.BookID == id);
        }
    }
}