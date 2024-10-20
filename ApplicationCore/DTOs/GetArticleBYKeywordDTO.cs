using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.DTOs
{
    public class GetArticleBYKeywordDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime RegDate { get; set; }
        public string posterImage { get; set; }
        public int ViewCount { get; set; }
        public int LikeCount { get; set; }
    }
}
