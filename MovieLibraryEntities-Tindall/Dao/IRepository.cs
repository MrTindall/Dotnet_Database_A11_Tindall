using Microsoft.EntityFrameworkCore;
using MovieLibraryEntities.Models;

namespace MovieLibraryEntities.Dao
{
    public interface IRepository
    {
        IEnumerable<Movie> GetAll();
        IEnumerable<Movie> Search(string searchString);

        Movie GetById(int id);
        Movie GetByTitle(string title);
        void AddMovie();
        void ReadById();
        void ReadByTitle();
        void ReturnAllMovies();
    }
}
