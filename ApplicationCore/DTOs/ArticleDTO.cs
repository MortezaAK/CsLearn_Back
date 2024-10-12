using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.DTOs
{
    public class ArticleDTO
    {
      
            public string Title { get; set; }
            public string Description { get; set; }
            public DateTime RegDate { get; set; }
            public string PosterImage { get; set; }
            public List<long> CategoryIds { get; set; }
            public List<long> KeywordIds { get; set; }
        

    }
}
