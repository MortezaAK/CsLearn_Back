using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Models
{
    public class Keyword
    {
        [Key]
        public long Id { get; set; }
        public string KeywordText { get; set; }

        public ICollection<ArticleKeyword> ArticleKeywords { get; set; }
        public ICollection<BookKeywords> BooksKeywords { get; set; }
    }
}
