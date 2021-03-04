using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebLibrary.Data;
using WebLibrary.Models;
using WebLibrary.Services;

namespace WebLibrary.Pages {
    public class MovieEditorModel : PageModel {
        private ILibrary _library;

        public List<Genre> Genres { get; set; }
        public string Header { get; set; }

        [BindProperty]
        public Movie Movie { get; set; }

        public MovieEditorModel(ILibrary library) {
            _library = library;
        }

        public IActionResult OnPost(IFormFile image, IFormFile video) {
            Movie.ProcessData();
            CheckForMatches();

            if (ModelState.IsValid) {
                if (image != null)
                    Movie.ImageName = DataWriter.AddImage(image) ?? Movie.ImageName;
                if (video != null)
                    Movie.VideoName = DataWriter.AddVideo(video) ?? Movie.VideoName;

                if (Movie.Id == null)
                    _library.Add(Movie);
                else
                    _library.Update(Movie);

                return RedirectToPage("Index");
            }
            else {
                InitializeProperties();
                return Page();
            }
        }

        public void OnGet() {
            Movie = new Movie();
            InitializeProperties();
        }

        public void OnGetByEdit(string movieId) {
            Movie = _library.Movies.Find(m => m.Id == movieId);
            InitializeProperties();
        }

        private void InitializeProperties() {
            Genres = _library.Genres;
            Header = Movie.Id == null? "Додати стрічку" : "Редагувати стрічку";
        }

        private void CheckForMatches() {
            foreach (Movie m in _library.Movies) {
                if (m.Equals(Movie))
                    ModelState.AddModelError("", "Фільм вже наявний у відеотеці!");
            }
        }
    }
}