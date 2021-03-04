using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WebLibrary.Models;
using WebLibrary.Pages;

namespace WebLibrary.Services {
    public interface ILibrary {
        List<Movie> Movies { get; }
        List<Genre> Genres { get; }
        List<MovieCollection> MovieCollections { get; }
        
        Movie Add(Movie movie);
        Genre Add(Genre genre);
        MovieCollection Add(MovieCollection genre);

        void Update(Genre genre);
        void Update(Movie movie);
        void Update(MovieCollection movie);

        void RemoveMovie(string id);
        void RemoveGenre(string id);
        void RemoveMovieCollection(string id);
    }
}
