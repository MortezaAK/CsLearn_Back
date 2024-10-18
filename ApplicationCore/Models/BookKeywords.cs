using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Models
{
    public class BookKeywords
    {
        public long id { get; set; }
        public long BookId { get; set; }
        public Books Books { get; set; }

        public long KeywordId { get; set; }
        public Keyword Keyword { get; set; }
    }
}
