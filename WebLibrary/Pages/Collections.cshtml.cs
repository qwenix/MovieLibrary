using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebLibrary.Services;

namespace WebLibrary.Pages
{
    public class CollectionsModel : PageModel {
        private readonly ILibrary _library;

        public List<MovieCollection> MovieCollections { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchedCollectionName { get; set; }

        [BindProperty]
        public MovieCollection Collection { get; set; }

        public CollectionsModel(ILibrary library) {
            _library = library;
        }

        public void OnGet() {
            Collection = new MovieCollection();
            MovieCollections = _library.MovieCollections.FindAll(c => 
                c.Name.Contains(SearchedCollectionName ?? string.Empty, StringComparison.CurrentCultureIgnoreCase));
        }

        public IActionResult OnPost() {
            if (ModelState.IsValid) {
                Collection.ProcessData();
                _library.Add(Collection);
                return RedirectToPage();
            }
            else {
                return Page();
            }
        }
    }
}