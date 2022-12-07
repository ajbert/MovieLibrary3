using Microsoft.EntityFrameworkCore;
using MovieLibraryEntities.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace MovieLibrary2
{
    internal class SortMenu
    {

        public SortMenu()
        {

            Console.WriteLine("Do you want to search based on age, occupation, or year?");
            char input = Console.ReadLine().ToLower()[0];


            switch (input)
            {
                case 'a':
                    {
                        using (var db = new MovieContext())


                        {
                            Console.WriteLine("What age group do you want to search");
                            var searchAge = Convert.ToInt32(Console.ReadLine());

                            try
                            {
                                var rankedOcc = db.UserMovies.Include(x => x.Movie).Where(x => x.User.Age == searchAge);

                                

                                var sortedList = rankedOcc.OrderByDescending(movie => movie.Rating).ThenBy(movie => movie.Movie.Title).FirstOrDefault();
                                
                                if (sortedList != null) {
                                    Console.WriteLine($"Movie: {sortedList.Movie.Title} Rating {sortedList.Rating}");
                                }
                                else
                                {
                                    Console.WriteLine("There were no results for your search");
                                }
                                }
                            catch (Exception)
                            {

                                Console.WriteLine("There were no results for your search"); ;
                            }
                            


                        }
                    }
                    break;

                case 'o':
                    {

                        {
                            using (var db = new MovieContext())


                            {
                                Console.WriteLine("What occupation do you want to search");
                                var searchOcc = Console.ReadLine();
                                try
                                {
                                    var rankedOcc = db.UserMovies.Include(x => x.Movie).Where(x => x.User.Occupation.Name == searchOcc);

                                    var sortedList = rankedOcc.OrderByDescending(movie => movie.Rating).ThenBy(movie => movie.Movie.Title).FirstOrDefault();

                                    if (sortedList != null)
                                    {
                                        Console.WriteLine($"Movie: {sortedList.Movie.Title} Rating {sortedList.Rating}");
                                    }
                                    else
                                        Console.WriteLine("There were no results for your search");
                                }
                                catch (Exception)
                                {

                                    Console.WriteLine("There were no results for your search");
                                }


                            }

                        }
                    }
                        break;


                        case 'y':
                    {

                        {
                            using (var db = new MovieContext())


                            {
                                Console.WriteLine("What year do you want to search");
                                var searchYear = Console.ReadLine();
                                try
                                {
                                    var rankedYear = db.UserMovies.Include(x => x.Movie).Where(x => x.Movie.ReleaseDate.ToString().Contains(searchYear));
                                                                                                          
                                                                       
                                    var sortedList = rankedYear.OrderByDescending(movie => movie.Rating).ThenBy(movie => movie.Movie.Title).FirstOrDefault();

                                    if (sortedList != null)
                                    {
                                        Console.WriteLine($"Movie: {sortedList.Movie.Title} Rating {sortedList.Rating}");
                                    }
                                    else
                                        Console.WriteLine("There were no results for your search");
                                }
                                catch (Exception)
                                {

                                    Console.WriteLine("There were no results for your search");
                                }


                            }

                        }
                        
                    } break;
            }
        }
    }
}
