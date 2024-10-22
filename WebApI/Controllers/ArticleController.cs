using ApplicationCore.DTOs;
using ApplicationCore.Interfaces.Services;
using ApplicationCore.Models;
using AutoMapper;
using Infrastructure.DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleController : BaseController
    {
        public ArticleController(IServiceContainer _serviceContainer) : base(_serviceContainer)
        {
        }

        [HttpGet]
        public async Task<IActionResult> GetAllArticle()
        {
            var result = await serviceContainer.Article.GetAllArticle();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddArticle(ArticleDTO dTO)
        {
            var result = await serviceContainer.Article.AddArticleAsync(dTO);

            if (result)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> EditArticle(long id, [FromBody] ArticleDTO articles)
        {
            var articleId = await serviceContainer.Article.GetArticleById(id);
          
            if (articleId == null)
                return NotFound();
            var result = await serviceContainer.Article.UpdateArticleAsync(articles, id);
            return Ok(result);
        }
        [HttpGet("id")]
        public async Task<IActionResult> GetArticleById(long id)
        {
            var articleId = await serviceContainer.Article.GetArticleById(id);
            if (articleId == null)
                return NotFound();
            return Ok(articleId);
        }

        [HttpDelete("id")]
        public async Task<IActionResult> DeleteArticle(long id)
        {
            if (id == null)
                return NotFound();
            var result = await serviceContainer.Article.DeleteArticle(id);
            return Ok(result);

        }
        [HttpGet("GetArticleBYCategoryID/id")]
        public async Task<IActionResult> GetArticleBYCategoryID(long id)
        {
            var result = await serviceContainer.Article.GetArticleBYCategoryID(id);
            if (result == null)
                return NotFound();
            return Ok(result);
        }
        [HttpGet("GetArticleBYKeywordID/id")]
        public async Task<IActionResult> GetArticleBYKeywordID(long id)
        {
            var result = await serviceContainer.Article.GetArticleBYKeywordID(id);
            if (result == null)
                return NotFound();
            return Ok(result);
        }
    }
}
