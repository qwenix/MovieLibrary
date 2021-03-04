using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebLibrary.Models;
using WebLibrary.Services;

namespace WebLibrary.Pages {
    public class IndexModel : PageModel {
        private ILibrary _library;

        [BindProperty(SupportsGet = true)]
        public SearchedMovie SearchedMovie { get; set; }

        public List<Movie> Movies { set; get; }
        public List<Genre> Genres { get; set; }

        public IndexModel(ILibrary library) {
            _library = library;
        }

        public void OnGet() {
            Movies = _library.Movies.FindAll(m => SearchedMovie.GetPredicates().All(p => p(m)));
            Genres = _library.Genres;
        }

        public IActionResult OnPostByRemove(string id) {
            _library.RemoveMovie(id);
            return RedirectToPage();
        }
    }
}
