using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebLibrary.Data;
using WebLibrary.Models;
using DnsClient;

namespace WebLibrary.Services {
    public class Library : ILibrary {
        private readonly IMongoCollection<Movie> _movies;
        private readonly IMongoCollection<Genre> _genres;
        private readonly IMongoCollection<MovieCollection> _movieCollections;

        /// <summary>
        /// Усі фільми, наявні у відеотеці.
        /// </summary>
        public List<Movie> Movies { get; private set; }
        /// <summary>
        /// Усі жанри, наявні у відеотеці.
        /// </summary>
        public List<Genre> Genres { get; private set; }
        /// <summary>
        /// Усі колекції, наявні у відеотеці.
        /// </summary>
        public List<MovieCollection> MovieCollections { get; private set; }

        public Library(string mongoLink) {
            var client = new MongoClient(mongoLink);
            var database = client.GetDatabase("LibraryDb");
            _movies = database.GetCollection<Movie>("Movies");
            _genres = database.GetCollection<Genre>("Genres");
            _movieCollections = database.GetCollection<MovieCollection>("Movie collections");
            Initialize();
        }

        /// <summary>
        /// Додає фільм до відеотеки.
        /// </summary>
        /// <param name="movie">Фільм, який необхідно додати</param>
        /// <returns>Доданий фільм</returns>
        public Movie Add(Movie movie) {
            AddGenresId(movie);
            _movies.InsertOne(movie);
            return movie;
        }

        /// <summary>
        /// Додає жанр до відеотеки.
        /// </summary>
        /// <param name="movie">Жанр, який необхідно додати</param>
        /// <returns>Доданий жанр</returns>
        public Genre Add(Genre genre) {
            _genres.InsertOne(genre);
            return genre;
        }

        /// <summary>
        /// Додає колекцію до відеотеки.
        /// </summary>
        /// <param name="movie">Колекція, яку необхідно додати</param>
        /// <returns>Додана колекція</returns>
        public MovieCollection Add(MovieCollection collection) {
            _movieCollections.InsertOne(collection);
            return collection;
        }

        /// <summary>
        /// Заміняє жанр з аналогічним ідентифікатором на інший.
        /// </summary>
        /// <param name="genre">Жанр, який оновляється</param>
        public void Update(Genre genre) {
            _genres.FindOneAndReplace(g => g.Id == genre.Id, genre);
        }

        /// <summary>
        /// Заміняє фільм з аналогічним ідентифікатором на інший.
        /// </summary>
        /// <param name="genre">Фільм, який оновляється</param>
        public void Update(Movie movie) {
            AddGenresId(movie);
            Movie _m = _movies.FindOneAndReplace(m => m.Id == movie.Id, movie);
            if (movie.ImageName != _m.ImageName)
                DataWriter.RemoveImage(_m.ImageName);
            if (movie.VideoName != _m.VideoName)
                DataWriter.RemoveVideo(_m.VideoName);
        }

        /// <summary>
        /// Заміняє колекцію з аналогічним ідентифікатором на іншу.
        /// </summary>
        /// <param name="genre">Колекція, якиа оновляється</param>
        public void Update(MovieCollection collection) {
            _movieCollections.ReplaceOne(c => c.Id == collection.Id, collection);
        }

        /// <summary>
        /// Видаляє фільм за вказаним ідентифікатором.
        /// </summary>
        /// <param name="id">Ідентифікатор фільму</param>
        public void RemoveMovie(string id) {
            Movie m = _movies.FindOneAndDelete(_m => _m.Id == id);
            DataWriter.RemoveImage(m.ImageName);
            DataWriter.RemoveVideo(m.VideoName);
            
            foreach (MovieCollection c in MovieCollections) {
                if (c.MoviesId.Contains(id))
                    c.MoviesId.Remove(id);
            }
        }

        /// <summary>
        /// Видаляє жанр за вказаним ідентифікатором.
        /// </summary>
        /// <param name="id">Ідентифікатор жанру</param>
        public void RemoveGenre(string id) {
            _genres.DeleteOne(g => g.Id == id);

            foreach (Movie m in Movies) {
                if (m.GenresId.Contains(id))
                    m.GenresId.Remove(id);
            }
        }

        /// <summary>
        /// Видаляє колекцію за вказаним ідентифікатором.
        /// </summary>
        /// <param name="id">Ідентифікатор колекції</param>
        public void RemoveMovieCollection(string id) {
            _movieCollections.DeleteOne(c => c.Id == id);
        }

        private void Initialize() {
            Movies = _movies.Find(_ => true).ToList();
            Genres = _genres.Find(_ => true).ToList();
            MovieCollections = _movieCollections.Find(_ => true).ToList();

            foreach (Movie m in Movies) {
                var appropGenres = Genres.Where(g => m.GenresId.Contains(g.Id));
                m.Genres = appropGenres.Select(g => g.Name).ToList();
            }
            foreach (MovieCollection c in MovieCollections) {
                c.Movies = Movies.Where(m => c.MoviesId.Contains(m.Id)).ToList();
            }
        }

        private void AddGenresId(Movie m) {
            var appropGenres = Genres.Where(g => m.Genres.Contains(g.Name));
            m.GenresId = appropGenres.Select(g => g.Id).ToList();
        }
    }
}
