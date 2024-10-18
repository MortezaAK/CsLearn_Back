using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Models
{
    public class BooksPermissions
    {
        public long Id { get; set; }
        public long BookId { get; set; }
        public Books Books { get; set; }

        public long CategoriesId { get; set; }
        public Categories Category { get; set; }
    }
}
