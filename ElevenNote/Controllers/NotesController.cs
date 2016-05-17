using ElevenNote.Models;
using ElevenNote.Services;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElevenNote.Controllers
{
    [Authorize]

    public class NotesController : Controller
    {
        private readonly Lazy<NoteService> _svc;

        public NotesController()
        {
            _svc =
                new Lazy<NoteService>(
                        () =>
                        {
                            var userId = Guid.Parse(User.Identity.GetUserId());
                            return new NoteService(userId);
                        }
                        );
        }
        
        // GET: Notes
        public ActionResult Index()
        {
            var notes = _svc.Value.GetNotes();

            return View(notes);
        }

        public ActionResult Create()
        {
            var vm = new NoteCreateViewModel();

            // Let's get all importance levels that we need for a DropDownList
            var importanceLevels = GetAllImportance();

            // Create a list of SelectListItems so these can be rendered on the page
            vm.ImportanceLevels = GetSelectListItems(importanceLevels);

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NoteCreateViewModel vm)
        {
            // Get all importance levels again
            var importanceLevels = GetAllImportance();

            // Set these importance levels on the model. We need to do this because
            // only the selected value from the DropDownList is posted back, not the whole
            // list of importance levels.
            vm.ImportanceLevels = GetSelectListItems(importanceLevels);

            if (!ModelState.IsValid) return View(vm);

            if (!_svc.Value.CreateNote(vm))
            {
                ModelState.AddModelError("", "Unable to create note");
                return View(vm);
            }

            return RedirectToAction("Index");
        }

        // Just return a list of impotance levels - in a real-world application this would call
        // into data access layer to retrieve importance levels from a database.
        private IEnumerable<string> GetAllImportance()
        {
            return new List<string>
            {
                "Urgent",
                "Normal",
                "Not Urgent",
            };
        }

        // This is one of the most important parts in the whole example.
        // This function takes a list of strings and returns a list of SelectListItem objects.
        // These objects are going to be used later in the .html template to render the
        // DropDownList.
        private IEnumerable<SelectListItem> GetSelectListItems(IEnumerable<string> elements)
        {
            // Create an empty list to hold result of the operation
            var selectList = new List<SelectListItem>();

            // For each string in the 'elements' variable, create a new SelectListItem object
            // that has both its Value and Text properties set to a particular value.
            // This will result in MVC rendering each item as:
            //     <option value="State Name">State Name</option>
            foreach (var element in elements)
            {
                selectList.Add(new SelectListItem
                {
                    Value = element,
                    Text = element
                });
            }

            return selectList;
        }

        public ActionResult Details(int id)
        {
            var vm = _svc.Value.GetNoteById(id);

            return View(vm);
        }

        public ActionResult Edit(int id)
        {
            var vm = _svc.Value.GetNoteById(id);

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Edit(NoteDetailViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            if (!_svc.Value.UpdateNote(vm))
            {
                ModelState.AddModelError("", "Unable to update note");
                return View(vm);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        [ActionName("Delete")]
        public ActionResult DeleteGet(int id)
        {
            var vm = _svc.Value.GetNoteById(id);

            return View(vm);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePost(int id)
        {
            _svc.Value.DeleteNote(id);

            return RedirectToAction("Index");
        }
    }
}