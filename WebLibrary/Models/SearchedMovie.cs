using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebLibrary.Models {
    public class SearchedMovie : IValidatableObject {
        public string Title { get; set; }
        public List<string> Genres { get; set; }
        public List<string> Directors { get; set; }
        public List<string> Actors { get; set; }
        public List<string> Studios { get; set; }
        public int? YearFrom { get; set; }
        public int? YearTo { get; set; }

        public IEnumerable<Predicate<Movie>> GetPredicates() {
            RemoveBlanked();

            if (Title != null)
                yield return m => m.Title.Contains(Title, StringComparison.CurrentCultureIgnoreCase);
            if (YearFrom != null)
                yield return m => m.Year >= YearFrom;
            if (YearTo != null)
                yield return m => m.Year <= YearTo;
            if (Genres.Count > 0)
                yield return m => m.Genres.Intersect(Genres).Count() == Genres.Count();
            if (Directors.Count > 0)
                yield return m => Directors.All(d => m.Directors.Any(_d => 
                    _d.Contains(d, StringComparison.CurrentCultureIgnoreCase)));
            if (Actors.Count > 0)
                yield return m => Actors.All(a => m.Actors.Any(_a => 
                    _a.Contains(a, StringComparison.CurrentCultureIgnoreCase)));
            if (Studios.Count > 0)
                yield return m => Studios.All(s => m.Studios.Any(_s => 
                    _s.Contains(s, StringComparison.CurrentCultureIgnoreCase)));
        }

        public SearchedMovie() {
            Genres = new List<string>();
            Directors = new List<string>();
            Actors = new List<string>();
            Studios = new List<string>();
        }

        private void RemoveBlanked() {
            Directors = Directors.Where(d => d != null).ToList();
            Genres = Genres.Where(g => g != null).ToList();
            Actors = Actors.Where(a => a != null).ToList();
            Studios = Studios.Where(s => s != null).ToList();
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) {
            if (YearFrom != null && YearTo != null && YearFrom > YearTo)
                yield return new ValidationResult("Перевірте правильність діапазону років...");
        }
    }
}
