using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Models
{
    public class ArticlesPermission
    {
        public long Id { get; set; }
        public long ArticlesId { get; set; }
        public Articles Article { get; set; }

        public long CategoriesId { get; set; }
        public Categories Category { get; set; }
    }
}
