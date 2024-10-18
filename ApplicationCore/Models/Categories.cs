using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Models
{
    public class Categories
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
        public int GroupId { get;set; }
        public int isDelete { get; set; }


        public ICollection<ArticlesPermission> ArticlesPermissions { get; set; }
        public ICollection<BooksPermissions> BooksPermissions { get; set; }
    }
}

//public int GroupId { get;set; }
//1 => aricle
//2 => Book
//3 => BugBank
//3 => news