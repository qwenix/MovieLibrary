using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebLibrary.Models;
using WebLibrary.Services;

namespace WebLibrary.Pages
{
    public class MovieCollectionInfoModel : PageModel {
        private readonly ILibrary _library;

        [BindProperty]
        [Required(ErrorMessage = "Назва обов'язкова!")]
        [StringLength(200, ErrorMessage = "Недопустима довжина назви!")]
        [RegularExpression(StringRevision.INPUT_REG_EXP, ErrorMessage = "Недопустима назва!")]
        public string NewCollectionName { get; set; }

        public MovieCollection MovieCollection { get; set; }

        public MovieCollectionInfoModel(ILibrary library) {
            _library = library;
        }

        public void OnGet(string id) {
            Initialize(id);
        }

        public FileResult OnGetByDownloadInfo(string id) {
            Initialize(id);
            return File(MovieCollection.GetInfo(), "text/plane", $"{MovieCollection.Name}.txt");
        }

        public IActionResult OnPostByRemoveMovie(string collectionId, string movieId) {
            Initialize(collectionId);
            MovieCollection.MoviesId.Remove(movieId);
            _library.Update(MovieCollection);
            return RedirectToPage(new { id = MovieCollection.Id });
        }

        public IActionResult OnPostByRemoveCollection(string id) {
            _library.RemoveMovieCollection(id);
            return RedirectToPage("Collections");
        }

        public IActionResult OnPostByEdit(string collectionId) {
            Initialize(collectionId);
            if (ModelState.IsValid) {
                MovieCollection.Name = NewCollectionName;
                _library.Update(MovieCollection);
                return RedirectToPage(new { id = MovieCollection.Id });
            }
            else
                return Page();
        }
        
        private void Initialize(string id) {
            MovieCollection = _library.MovieCollections.SingleOrDefault(c => c.Id == id);
        }
    }
}