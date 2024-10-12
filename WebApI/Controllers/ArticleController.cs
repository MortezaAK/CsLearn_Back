using ApplicationCore.DTOs;
using ApplicationCore.Interfaces.Services;
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

    }
}
