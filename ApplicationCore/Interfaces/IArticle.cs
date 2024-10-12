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
        Task<bool> AddArticleAsync(ArticleDTO articleDto);
    }
}
