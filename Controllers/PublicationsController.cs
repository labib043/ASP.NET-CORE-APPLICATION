using System.Linq;
using System.Threading.Tasks;
using crudcore.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
//using Syncfusion.XForms.Android.Core;

///*/*1*/*/264252Core.Model;

namespace crudcore.Controller
{
    public class PublicationsController : Microsoft.AspNetCore.Mvc.Controller
    {
        public ApplicationDbContext applicationDb;
        public PublicationsController(ApplicationDbContext applicationDbContext)
        {
            applicationDb = applicationDbContext;
        }
        public IActionResult Index()
        {
            PViewModel pViewModel = new PViewModel();
            pViewModel.PublicationIDVM = applicationDb.Publications.AsEnumerable();
            return View(pViewModel.PublicationIDVM);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Publication publication)
        {
            if (ModelState.IsValid)
            {
                //  applicationDb.Publications.Add(publication); 
                applicationDb.Publications.Add(publication);

                await applicationDb.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(publication);

        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var publication = await applicationDb.Publications.FindAsync(id);
            if (publication == null)
            {
                return NotFound();
            }
            return View(publication);

        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Publication publication)
        {
            if (id != publication.PublicationID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    applicationDb.Update(publication);
                    await applicationDb.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PublicationExists(publication.PublicationID))
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
            return View(publication);
        }

        public ActionResult Delete(int? id)

        {

            try

            {

                var firstEntity = applicationDb.Publications.Where(c => c.PublicationID == id).FirstOrDefault();

                applicationDb.Publications.Remove(firstEntity);

                applicationDb.SaveChanges();

            }

            finally

            {



            }

            return RedirectToAction("Index");

        }


        private bool PublicationExists(int id)
        {
            return applicationDb.Publications.Any(p => p.PublicationID == id);
        }
    }
}
