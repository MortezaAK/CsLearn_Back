using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.DTOs
{
    public class ArticleByDTO
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime RegDate { get; set; }
        public string PosterImage { get; set; }
        public List<CategoryInOtherTableDTO> CategoryIds { get; set; }
        public List<KeywordDTO> KeywordIds { get; set; }
        public int IsDelete { get; set; }
        public int LikeCount { get; set; }
        public int ViewCount { get; set; }
    }
}
