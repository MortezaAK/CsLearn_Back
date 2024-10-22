using ApplicationCore.DTOs;
using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IArticle
    {
        Task<IEnumerable<ArticleByDTO>> GetAllArticle();
        Task<bool> AddArticleAsync(ArticleDTO articleDto);
        Task<bool> UpdateArticleAsync(ArticleDTO articlesDTO, long articleID);
        Task<Articles> GetArticleById(long id);
        Task<bool> DeleteArticle(long articleId);
        Task<IEnumerable<GetArticleBYCategoryDTO>> GetArticleBYCategoryID(long id);
        Task<IEnumerable<GetArticleBYKeywordDTO>> GetArticleBYKeywordID(long id);
    }
}
