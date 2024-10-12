using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Models
{
    public class ArticleKeyword
    {
        public long id { get;set; }
        public long ArticleId { get; set; }
        public Articles Article { get; set; }

        public long KeywordId { get; set; }
        public Keyword Keyword { get; set; }
    }
}
