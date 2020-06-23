using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryDAL.Models
{
    public interface IReaderRepository
    {
        IEnumerable<DBUser> GetReaders();
        DBUser GetReaderById(int id);
        bool ChangeReaderStatus(int id);
    }
}
