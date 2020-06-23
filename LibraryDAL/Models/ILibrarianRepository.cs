using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryDAL.Models
{
    public interface ILibrarianRepository
    {
        IEnumerable<DBUser> GetLibrarians();
        DBUser GetLibrarianById(int id);
        bool AddNewLibrarian(DBUser librarian, string password);
        bool DeleteLibrarian(int id);
    }
}
