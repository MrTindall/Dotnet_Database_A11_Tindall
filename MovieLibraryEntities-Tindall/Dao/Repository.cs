﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MovieLibraryEntities.Context;
using MovieLibraryEntities.Models;

namespace MovieLibraryEntities.Dao
{
    public class Repository : IRepository, IDisposable
    {
        private readonly IDbContextFactory<MovieContext> _contextFactory;
        private readonly MovieContext _context;

        public Repository(IDbContextFactory<MovieContext> contextFactory)
        {
            _contextFactory = contextFactory;
            _context = _contextFactory.CreateDbContext();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public IEnumerable<Movie> GetAll()
        {
            return _context.Movies.ToList();
        }

        public IEnumerable<Movie> Search(string searchString)
        {
            var allMovies = _context.Movies;
            var listOfMovies = allMovies.ToList();
            var temp = listOfMovies.Where(x => x.Title.Contains(searchString, StringComparison.CurrentCultureIgnoreCase));

            return temp;
        }

        public Movie GetById(int id)
        {
            return _context.Movies.FirstOrDefault(x => x.Id == id);
        }

        public Movie GetByTitle(string title)
        {
            return _context.Movies.FirstOrDefault(x => x.Title.ToLower() == title);
        }

        public void AddMovie()
        {
            Console.Write("Enter the movie title: ");
            var movieTitle = Console.ReadLine();

            Console.Write("Enter the movie release year: ");
            var movieReleaseYearInput = Console.ReadLine();

            var newMovie = new Movie()
            {
                Title = movieTitle
            };

            if (int.TryParse(movieReleaseYearInput, out int movieReleaseYear))
            {
                newMovie.ReleaseDate = new DateTime(movieReleaseYear, 1, 1); 
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid year.");
            }


            using (var repository = new MovieContext())
            {
                repository.Movies.Add(newMovie);
                repository.SaveChanges();
            }

            Console.WriteLine("Successfully added the movie");
        }

        public void ReadById()
        {
            Console.Write($"Enter an Id: ");
            var idValue = Console.ReadLine();

            var movie = GetById(Convert.ToInt32(idValue));
            if (movie != null)
            {
                Console.WriteLine($"\nYour movie is {movie.Title}\n");
            }
            else
            {
                Console.WriteLine("\nThat is not in the database\n");
            }
        }

        public void ReadByTitle()
        {
            Console.Write($"Enter a movie Title: ");
            var titleValue = Console.ReadLine().ToLower();

            var movie = Search(titleValue);
            if (movie != null)
            {
                foreach (var item in movie)
                {
                    Console.WriteLine($"\nTitle: {item.Title}, Release Date: {item.ReleaseDate.Year}\n");
                }
            }
            else
            {
                Console.WriteLine($"That movie is not in the database\n");
            }
        }

        public void ReturnAllMovies()
        {
            var movies = GetAll();

            Console.WriteLine();
            foreach (var movie in movies)
            {
                Console.WriteLine($"Title: {movie.Title}, Release Date: {movie.ReleaseDate.Year}");
            }
            Console.WriteLine();
        }

        public void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            optionsBuilder
            .UseLazyLoadingProxies()
            .UseSqlServer(configuration.GetConnectionString("MovieContext"));
        }

        public void ModifyMovie()
        {
            Console.WriteLine("Enter Movie to Modify");
            var userInput = Console.ReadLine();
            var movie = Search(userInput);
            Console.WriteLine();
            if (movie != null)
            {           
                var titleToMod = GetByTitle(userInput);
                if(titleToMod != null)
                {
                    Console.WriteLine($"Enter a new Title to replace {titleToMod.Title}");
                    var newTitle = Console.ReadLine();
                    if (newTitle != null)
                    {
                        Console.WriteLine($"{titleToMod.Title} is now {newTitle}\n");
                        titleToMod.Title = newTitle;
                        _context.SaveChanges();
                    }
                    else
                    {
                        Console.WriteLine("Title Does not Exist\n");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid Input\n");
                }
            }
            else
            {
                Console.WriteLine("Invalid Input\n");
            }

        }

        public void DeleteMovie()
        {
            Console.WriteLine("Enter Movie to Delete");
            var userInput = Console.ReadLine();
            var movie = Search(userInput);
            Console.WriteLine();
            if (movie != null)
            {
                var titleToDelete = GetByTitle(userInput);

                if (titleToDelete != null)
                {
                    Console.WriteLine($"{titleToDelete.Title} is now deleted\n");
                    _context.Remove(titleToDelete); 
                    _context.SaveChanges();
                }
                else
                {
                    Console.WriteLine("Title Does not Exist\n");
                }

            }
            else
            {
                Console.WriteLine("Invalid Input\n");
            }
        }
    }
}
