using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryDAL.Models
{
    public interface IDBRepository
    {
        DBUser GetUserById(int id);

        IEnumerable<DBUser> GetUsers();
    }
}
