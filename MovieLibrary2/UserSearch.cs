using MovieLibraryEntities.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Transactions;
using System.Security.Cryptography.X509Certificates;

namespace MovieLibrary2
{
    internal class UserSearch
    {
        public UserSearch() {
            string input;
            do 
            {
                Console.WriteLine("User Search");
                Console.WriteLine("1. List All Users");
                Console.WriteLine("2. List All Users by Occupation");
                Console.WriteLine("3. Search Users by Occupation");
                Console.WriteLine("4. Search Users by Zip Code");
                Console.WriteLine("5. Movies Rating by User");
                Console.WriteLine("0. Exit User Search");

                input = Console.ReadLine();

                try
                {
                    switch (input) {

                        case "1":
                            {
                                using (var db = new MovieContext())
                                {
                                    var listOfUsers = db.Users.Include(x => x.Occupation).OrderBy(x => x.Id);
                                    foreach (var user in listOfUsers)
                                    {
                                        Console.WriteLine($"{user.Id}) {user.Age} {user.Gender} {user.ZipCode} {user.Occupation.Name} ");

                                    }
                                }
                            } break;

                        case "2":
                            {
                                using (var db = new MovieContext())
                                {
                                    var listOfUsers = db.Users.Include(x => x.Occupation);
                                    foreach (var user in listOfUsers)
                                    {
                                        Console.WriteLine($"{user.Id}) {user.Age} {user.Gender} {user.Occupation.Name}");

                                    }
                                }
                            } break;

                            case "3":
                            {
                                Console.WriteLine("What Occupation would you like search for?");
                                var occSearch = Console.ReadLine();

                                try
                                {
                                    using (var db = new MovieContext())
                                    {
                                        var listOfUsers = db.Users.Where(x => x.Occupation.Name == occSearch);
                                        var count = 0;

                                        foreach (var user in listOfUsers)
                                        {
                                            Console.WriteLine($"{user.Id}) {user.Age} {user.Gender} {user.ZipCode}");
                                            count++;
                                        }

                                        if (count != 1)
                                        Console.WriteLine($"There were {count} users listed under {occSearch}");
                                        else
                                            Console.WriteLine($"There was {count} user listed under {occSearch}");
                                    }
                                }
                                catch (Exception)
                                {

                                    Console.WriteLine("There was an error");
                                }
                                

                            } break;

                            case "4":
                            {
                                Console.WriteLine("Enter the Zip Code you want to search for");
                                var zipSearch = Console.ReadLine();

                                try
                                {
                                    using (var db = new MovieContext())
                                    {
                                        var listOfUsers = db.Users.Include(x => x.Occupation).Where(x => x.ZipCode == zipSearch);
                                        var count = 0;

                                        foreach (var user in listOfUsers)
                                        {
                                            Console.WriteLine($"{user.Id}) {user.Age} {user.Gender} {user.Occupation.Name}");
                                            count++;
                                        }

                                        if (count != 1)
                                            Console.WriteLine($"There were {count} users listed the Zip Code {zipSearch}");
                                        else
                                            Console.WriteLine($"There was {count} user with the Zip Code {zipSearch}");
                                    }
                                }
                                catch (Exception)
                                {

                                    Console.WriteLine("There was an error");
                                }


                            }break;

                            case "5":
                            {
                                Console.WriteLine("Enter the User Id for User you want to search for");
                                var userIDSearch = Convert.ToInt32( Console.ReadLine() );

                                try
                                {
                                    using (var db = new MovieContext())
                                    {
                                        var listOfMovies = db.UserMovies.Include(x => x.Movie).Where(x => x.User.Id == userIDSearch);
                                        var count =0;
                                        long total = 0;
                                        
                                            foreach (var movie in listOfMovies)
                                            {
                                                Console.WriteLine($"Title: {movie.Movie.Title} Rating: {movie.Rating}");
                                            count++;
                                             total += movie.Rating;
                                            
                                            }
                                        if (count <= 0)
                                            Console.WriteLine($"User {userIDSearch} hasn't rated any movies");
                                        else if (count == 1)
                                        {
                                            Console.WriteLine($"User {userIDSearch} has rated {count} with a ranking of {total}");
                                        }
                                       
                                        else if (count > 1)
                                        {
                                            Console.WriteLine($"User {userIDSearch} has rated {count} movies with an average rating of {total/count}");
                                        }
                                        
                                        
                                        
                                    }

                                }
                                catch (Exception)
                                {

                                    throw;
                                }

                            }break;



                    }
                }
            catch (Exception)
                {

                    Console.WriteLine("There is an error");
                }
            } while (input != "0"); 
            
        }
    }
}
