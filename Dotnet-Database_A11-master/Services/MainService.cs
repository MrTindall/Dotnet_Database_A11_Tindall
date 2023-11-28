using MovieLibraryEntities.Context;
using MovieLibraryEntities.Dao;
using MovieLibraryEntities.Models;
using System;
using System.Collections.Generic;


namespace ApplicationTemplate.Services;

/// <summary>
///     You would need to inject your interfaces here to execute the methods in Invoke()
///     See the commented out code as an example
/// </summary>
public class MainService : IMainService
{

    //private readonly IContext _context;
    private readonly IRepository _context;
    public MainService(IRepository repository)
    {
        _context = repository;
    }
    public void Invoke()
    {
        while (true)
        {
            Console.Write($"Search for a movie:\n1: Id\n2: Title\n3: List Movies\n4: Add Movie\n5: Modify Movie\n6: Delete Movie\n7: Exit\nEnter: ");
            var idOrTitleAnswer = Console.ReadLine();

            if( idOrTitleAnswer == "1" ) 
            {
                _context.ReadById();
            }

            else if (idOrTitleAnswer == "2")
            {
                _context.ReadByTitle();
            }

            else if (idOrTitleAnswer == "3")
            {
                _context.ReturnAllMovies();
            }

            else if (idOrTitleAnswer == "4")
            {
                _context.AddMovie();
            }
            else if (idOrTitleAnswer == "5")
            {
                _context.ModifyMovie();
            }
            else if (idOrTitleAnswer == "6")
            {
                _context.DeleteMovie();
            }
            else if (idOrTitleAnswer == "7")
            {
                Console.WriteLine("\nExiting the program...");
                break;
            }
            else
            {
                Console.WriteLine($"That is not a valid input\n");
            }      
        }
       
    }
}