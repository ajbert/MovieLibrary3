using MovieLibraryEntities.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Transactions;
using System.Security.Cryptography.X509Certificates;
using MovieLibraryEntities.Models;

namespace MovieLibrary2
{
    internal class UserSearch
    {
        public UserSearch() {
            string input;
            do 
            {
                Console.WriteLine("User Menu");
                Console.WriteLine("1. List All Users");
                Console.WriteLine("2. List All Users by Occupation");
                Console.WriteLine("3. Search Users by Occupation");
                Console.WriteLine("4. Search Users by Zip Code");
                Console.WriteLine("5. Movies Rating by User");
                Console.WriteLine("6. Add User");
                Console.WriteLine("7. Delete User");
                Console.WriteLine("0. Exit User Search");

                input = Console.ReadLine();

                try
                {
                    switch (input)
                    {

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
                            }
                            break;

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
                            }
                            break;

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


                            }
                            break;

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


                            }
                            break;

                        case "5":
                            {
                                Console.WriteLine("Enter the User Id for User you want to search for");
                                var userIDSearch = Convert.ToInt32(Console.ReadLine());

                                try
                                {
                                    using (var db = new MovieContext())
                                    {
                                        var listOfMovies = db.UserMovies.Include(x => x.Movie).Where(x => x.User.Id == userIDSearch);
                                        var count = 0;
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
                                            Console.WriteLine($"User {userIDSearch} has rated {count} movie with a ranking of {total}");
                                        }

                                        else if (count > 1)
                                        {
                                            Console.WriteLine($"User {userIDSearch} has rated {count} movies with an average rating of {total / count}");
                                        }



                                    }

                                }
                                catch (Exception)
                                {

                                    throw;
                                }

                            }
                            break;

                        case "6":

                            {
                                Console.WriteLine("Add a new user \n");
                                Console.WriteLine("Enter user's age");
                                var age = Convert.ToInt32(Console.ReadLine());
                                Console.WriteLine("Enter user's gender");
                                var gender = Convert.ToString(Console.ReadLine()[0]).ToUpper();
                                Console.WriteLine("Enter user's zip code");
                                var zip = Console.ReadLine();
                                Console.WriteLine("Enter user's occupation");
                                var occ = Console.ReadLine();

                                using (var db = new MovieContext())



                                    try
                                    {
                                        if (db.Occupations.FirstOrDefault(x => x.Name == occ) == null)
                                        {
                                            var newOcc = new Occupation()
                                            {
                                                Name = occ
                                            };
                                            db.Occupations.Add(newOcc);
                                            db.SaveChanges();
                                        }


                                        var occID = db.Occupations.FirstOrDefault(x => x.Name == occ);

                                        var newUser = new User()
                                        {
                                            Age = age,
                                            Gender = gender,
                                            ZipCode = zip,
                                            Occupation = occID

                                        };
                                        db.Users.Add(newUser);
                                        db.SaveChanges();

                                        var newEntry = db.Users.AsEnumerable().Last();
                                        Console.WriteLine($"{newEntry.Id} {newEntry.Age} {newEntry.Gender} {newEntry.ZipCode} {newEntry.Occupation.Name}");

                                    }
                                    catch (Exception)
                                    {

                                        Console.WriteLine("There was an error"); 
                                    }

                            }
                            break;

                        case "7": 
                                {
                                    Console.WriteLine("Enter User ID of User to be deleted");
                                    var userID = Convert.ToInt32(Console.ReadLine());

                                    using (var db = new MovieContext())
                                    {
                                        var userToDelete = db.Users.Include(x => x.Occupation).FirstOrDefault(x => x.Id == userID);
                                        Console.WriteLine($"{userToDelete.Id}) {userToDelete.Age} {userToDelete.Gender} {userToDelete.ZipCode} {userToDelete.Occupation.Name}");

                                        Console.WriteLine("Do you want to delete this user? y/n ");
                                        var confirm = Console.ReadLine();

                                        if (confirm.ToLower() != "y")
                                        {
                                            Console.WriteLine("The user was not deleted");
                                            break;
                                        }
                                        else
                                            db.Users.Remove(userToDelete);
                                        db.SaveChanges();
                                        Console.WriteLine("The user was deleted");

                                    }
                                }
                                break;

                        case "8":
                            {
                                Console.WriteLine("Enter Movie ID");
                                var movieId = Convert.ToInt32(Console.ReadLine());
                                Console.WriteLine("Enter your User ID");
                                var userId = Convert.ToInt32(Console.ReadLine());

                                using (var db = new MovieContext())
                                {
                                  var rateMovie = db.Movies.Include(x => x.UserMovies).ThenInclude(x => x.User).FirstOrDefault(x =>x.Id == movieId);
                                    Console.WriteLine($"What do you rate {rateMovie.Title}? (1-5)");
                                    var rating = Convert.ToInt32(Console.ReadLine());

                                    Console.WriteLine($"You want rate {rateMovie.Title} {rating}? y/n?");
                                    var confirm = Console.ReadLine();

                                    if (confirm.ToLower() != "y")
                                    {
                                        Console.WriteLine($"The {rateMovie.Title} was not rated");
                                        break;
                                    }
                                    else
                                    {
                                        var newUserMovie = new UserMovie()
                                        {
                                            Rating = rating,
                                            RatedAt = DateTime.Now,
                                            User = db.Users.FirstOrDefault(x => x.Id == userId),
                                            Movie = db.Movies.FirstOrDefault(x => x.Id == movieId)
                                        };
                                        db.UserMovies.Add(newUserMovie);
                                        db.SaveChanges();
                                        var newEntry = db.UserMovies.AsEnumerable().Last();
                                        Console.WriteLine($"{newEntry.Id} {newEntry.Movie.Title} {newEntry.Rating} {newEntry.User.Id}");
                                    }


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
