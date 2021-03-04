using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebLibrary.Models;
using WebLibrary.Services;

namespace WebLibrary.Pages {
    public class MovieInfoModel : PageModel {
        private ILibrary _library;

        public Movie Movie { get; set; }
        public List<string> MovieGenres { get; set; }

        public List<MovieCollection> MovieCollections { get; set; }

        [BindProperty]
        public string CollectionName { get; set; }

        public MovieInfoModel(ILibrary library) {
            _library = library;
            MovieCollections = _library.MovieCollections;
        }

        public void OnGet(string movieId) {
            Initialize(movieId);
        }

        public FileResult OnGetByDownloadInfo(string movieId) {
            Initialize(movieId);
            return File(Movie.GetInfo(), "text/plane", $"{Movie.Title}.txt");
        }

        public IActionResult OnPostByRemove(string id) {
            _library.RemoveMovie(id);
            return RedirectToPage("Index");
        }

        public IActionResult OnPostByAddToCollection(string id) {
            Initialize(id);
            MovieCollection collection = _library.MovieCollections.FirstOrDefault(c => c.Name == CollectionName);

            if (collection.Movies.Any(m => m.Id == id))
                ModelState.AddModelError("", "Фільм вже наявний у даній колекції!");
            else {
                collection.MoviesId.Add(Movie.Id);
                _library.Update(collection);
                return RedirectToPage("MovieCollectionInfo", new { id = collection.Id });
            }

            return Page();
        }

        private void Initialize(string id) {
            Movie = _library.Movies.Find(m => m.Id == id);
        }
    }
}