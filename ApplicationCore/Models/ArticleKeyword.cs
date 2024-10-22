using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Models
{
    public class ArticleKeyword
    {
        
        public long ID { get;set; }
        public long ArticlesId { get; set; }
        public Articles Article { get; set; }

        public long KeywordsId { get; set; }
        public Keyword Keyword { get; set; }
    }
}
