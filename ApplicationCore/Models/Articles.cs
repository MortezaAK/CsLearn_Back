using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Models
{
    public class Articles
    {
        [Key]
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int ViewCount { get; set; }
        public DateTime RegDate { get; set; }
        public string posterImage { get; set; }
        public int isDelete { get; set; }
        public int LikeCount { get;set; }
       
        public ICollection<ArticlesPermission> ArticlesPermissions { get; set; }
        public ICollection<ArticleKeyword> ArticleKeywords { get; set; }
    }
}
