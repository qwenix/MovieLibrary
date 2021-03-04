using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebLibrary.Models;
using WebLibrary.Services;

namespace WebLibrary.Pages
{
    public class GenresModel : PageModel {
        private ILibrary _library;

        [BindProperty]
        public Genre Genre { get; set; }
        public List<Genre> Genres { set; get; }

        public GenresModel(ILibrary library) {
            _library = library;
        }

        public void OnGet() {
            Initialize();
        }

        public IActionResult OnPostByRemove(string id) {
            _library.RemoveGenre(id);
            return RedirectToPage();
        }

        public IActionResult OnPostBySave() {
            Genre.Name = StringRevision.RemoveUnnecessarySpaces(Genre.Name);
            CheckForMatches();

            if (ModelState.IsValid) {
                if (Genre.Id == null)
                    _library.Add(Genre);
                else
                    _library.Update(Genre);

                return RedirectToPage();
            }
            else {
                Initialize();
                return Page();
            }
        }

        private void Initialize() {
            Genres = _library.Genres.FindAll(_ => true);
        }

        private void CheckForMatches() {
            foreach (Genre g in _library.Genres) {
                if (g.Equals(Genre))
                    ModelState.AddModelError("", "Такий жанр вже наявний у відеотеці!");
            }
        }
    }
}