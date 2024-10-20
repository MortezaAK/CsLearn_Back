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
        Task<IEnumerable<Articles>> GetAllArticle();
        Task<bool> AddArticleAsync(ArticleDTO articleDto);
        Task<bool> UpdateArticleAsync(ArticleDTO articlesDTO);
        Task<Articles> GetArticleById(long id);
        Task<bool> DeleteArticle(long articleId);
        Task<IEnumerable<GetArticleBYCategoryDTO>> GetArticleBYCategoryID(long id);
        Task<IEnumerable<GetArticleBYKeywordDTO>> GetArticleBYKeywordID(long id);
    }
}
