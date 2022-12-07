using MovieLibraryEntities.Context;
using MovieLibraryEntities.Models;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;
using System.Transactions;
using System.Linq;
using MovieLibrary2;
using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using System.Security.Cryptography.X509Certificates;

namespace MovieLibrary2;

public class Program
{

    private static void Main(string[] args)
    {
        var factory = LoggerFactory.Create(x => x.AddConsole());
        var logger = factory.CreateLogger<Program>();
        string input;


        do
        {
            Console.WriteLine("1. Search Movie");
            Console.WriteLine("2. Add Movie");
            Console.WriteLine("3. Update Movie");
            Console.WriteLine("4. Delete Movie");
            Console.WriteLine("5. List all movies");
            Console.WriteLine("6. Search by year");
            Console.WriteLine("7. User Menu");
            Console.WriteLine("8. Get Top Rated Movie   ");
            Console.WriteLine("0. Exit");
            input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    // search for movie
                    Console.WriteLine("Enter search criteria");
                    var movieSearch = Console.ReadLine();

                    try
                    {
                        using (var db = new MovieContext())
                        {
                            var searchResults = db.Movies.Include(m => m.MovieGenres).ThenInclude(m => m.Genre).Where(m => m.Title.Contains(movieSearch));
                            // var searchResults = listOfMovies.Where(m => m.Title.Contains(movieSearch, StringComparison.OrdinalIgnoreCase));
                            var count = 0;
                            foreach (var movie in searchResults)
                            {
                                Console.WriteLine($"{movie.Id}) {movie.Title} {movie.ReleaseDate}");

                                Console.Write("Genre(s): ");
                                foreach (var g in movie.MovieGenres)
                                {
                                    Console.Write($" {g.Genre.Name} ");
                                }
                                Console.WriteLine();
                                count++;
                            }
                            if (count == 1)
                                Console.WriteLine($"{count} movie containing {movieSearch} was found");

                            else
                                Console.WriteLine($"{count} movies containing {movieSearch} were found");
                        }
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("There was an error with your search!");
                    }
                    break;

                case "2":
                    {
                        Console.WriteLine("Enter title of movie");
                        var title = Console.ReadLine();

                        Console.WriteLine("Enter year movie was released(Year-Month-Day)");
                        var date = Convert.ToDateTime(Console.ReadLine());

                        using (var db = new MovieContext())
                        {
                            var addMovie = new Movie()
                            {
                                Title = title,
                                ReleaseDate = date
                            };
                            db.Movies.Add(addMovie);
                            db.SaveChanges();

                            var newMovie = db.Movies.FirstOrDefault(x => x.Title == title);
                            Console.WriteLine($"{newMovie.Id}) {newMovie.Title} ({newMovie.ReleaseDate})");
                        }
                    }
                    break;


                case "3":
                    {
                        Console.WriteLine("Enter movie ID to update");
                        var movieId = Convert.ToInt32(Console.ReadLine());

                        try
                        {
                            using (var db = new MovieContext())
                            {
                                var movieToUpdate = db.Movies.FirstOrDefault(x => x.Id == movieId);
                                Console.WriteLine($"{movieToUpdate.Id}) {movieToUpdate.Title} {movieToUpdate.ReleaseDate}");

                                Console.WriteLine("Would you like to update the title or the date?");
                                var input2 = Console.ReadLine();

                                if (input2.StartsWith("t"))
                                {
                                    Console.WriteLine("Please enter new title");
                                    var newTitle = Console.ReadLine();

                                    movieToUpdate.Title = newTitle;

                                    db.Movies.Update(movieToUpdate);
                                    db.SaveChanges();
                                    Console.WriteLine($"{movieToUpdate.Id}) {movieToUpdate.Title} was updated");
                                }
                                else if (input2.StartsWith("d"))
                                {
                                    Console.WriteLine("Please enter the updated release date (Year-Month-Day)");
                                    var newDate = Convert.ToDateTime(Console.ReadLine());

                                    movieToUpdate.ReleaseDate = newDate;
                                    db.Movies.Update(movieToUpdate);
                                    db.SaveChanges();
                                    Console.WriteLine($"{movieToUpdate.Id}) {movieToUpdate.Title} {movieToUpdate.ReleaseDate} was updated");
                                }
                                else
                                    Console.WriteLine($"{movieToUpdate.Id}) {movieToUpdate.Title} wasn't updated");
                            }


                        }
                        catch (Exception)
                        {
                            Console.WriteLine("There was an error with your search");
                        }
                    }
                    break;

                case "4":
                    {
                        Console.WriteLine("Enter movie ID to delete");
                        var movieId = Convert.ToInt32(Console.ReadLine());

                        try
                        {
                            using (var db = new MovieContext())
                            {

                                var movieToDelete = db.Movies.FirstOrDefault(x => x.Id == movieId);
                                Console.WriteLine($"{movieToDelete.Id}) {movieToDelete.Title}");

                                Console.WriteLine("Do you want to delete this record? y/n ");
                                var confirm = Console.ReadLine();

                                if (confirm != "y")
                                {
                                    Console.WriteLine("The movie was not deleted");
                                    break;
                                }
                                else
                                    db.Movies.Remove(movieToDelete);
                                db.SaveChanges();
                                Console.WriteLine("The movie was deleted");
                            }

                        }
                        catch (Exception)
                        {
                            Console.WriteLine("There was an error!");
                        }
                    }
                    break;

                case "5":
                    {
                        using (var db = new MovieContext())
                        {
                            var listOfMovies = db.Movies;
                            foreach (var movie in listOfMovies)
                            {
                                Console.WriteLine($"{movie.Id}) {movie.Title} {movie.ReleaseDate}");

                            }
                        }
                    }
                    break;

                case "6":
                    // search for movie
                    Console.WriteLine("Enter year");
                    movieSearch = Console.ReadLine();

                    try
                    {
                        using (var db = new MovieContext())
                        {
                            var listOfMovies = db.Movies.ToList();
                            var searchResults = listOfMovies.Where(m => m.ReleaseDate.ToString().Contains(movieSearch));
                            var count = 0;
                            foreach (var movie in searchResults)
                            {
                                Console.WriteLine($"{movie.Id}) {movie.Title} {movie.ReleaseDate}");
                                count++;
                            }
                            if (count == 1)
                                Console.WriteLine($"{count} movie from {movieSearch} was found");

                            else
                                Console.WriteLine($"{count} movies from {movieSearch} were found");
                        }
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("There was an error with your search!");
                    }
                    break;

                case "7":
                    {
                        var userSearch = new UserSearch();
                    } break;

                case "8": {
                    
                     var sortMenu = new SortMenu();
                    } break;                  

            }   

        } while (input != "0");
    }
}