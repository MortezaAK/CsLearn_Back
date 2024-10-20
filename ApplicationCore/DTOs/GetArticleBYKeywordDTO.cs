using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.DTOs
{
    public class GetArticleBYKeywordDTO
    {
        public IEnumerable<ArticlesDTO> GetAllArticle { get; set; }
        public IEnumerable<KeywordDTO> GetAllKeyWords { get; set; }
    }
}
