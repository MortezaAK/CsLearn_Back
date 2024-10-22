using ApplicationCore.DTOs;
using ApplicationCore.Interfaces.DataAccess;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Dictionary<Type, object> PrivateRepository;
        private readonly ApplicationContext db;
        private readonly IHttpContextAccessor _accessor;
        private readonly IMapper _mapper;
        public UnitOfWork(ApplicationContext context, IMapper mapper, IHttpContextAccessor accessor)
        {
            db = context;
            _mapper = mapper;
            _accessor = accessor;
            PrivateRepository = new Dictionary<Type, object>();
        }
        //کوئری براساس نمایش مقاله ها براساس CategoryID
        public async Task<IEnumerable<GetArticleBYCategoryDTO>> GetArticleBYCategoryID(long id)
        {
            var result = (from article in db.Articles
                          join acper in db.ArticlesPermissions on article.Id equals acper.ArticlesId
                          join cat in db.Categories on acper.CategoriesId equals cat.Id
                          where cat.Id == id
                          select new GetArticleBYCategoryDTO
                          {
                              Title = article.Title,
                              Description = article.Description,
                              LikeCount = article.LikeCount,
                              ViewCount = article.ViewCount,
                              posterImage = article.posterImage,
                              RegDate = article.RegDate,
                          }).ToList();
            return result;
        }
        //کوئری براساس KeywordID نمایش مقاله ها
        public async Task<IEnumerable<GetArticleBYKeywordDTO>> GetArticleBYKeywordID(long id)
        {
            var result = (from article in db.Articles
                          join Ak in db.ArticleKeywords on article.Id equals Ak.ArticlesId
                          join Key in db.Keywords on Ak.KeywordsId equals Key.Id
                          where Key.Id == id
                          select new GetArticleBYKeywordDTO
                          {
                              Title = article.Title,
                              Description = article.Description,
                              LikeCount = article.LikeCount,
                              ViewCount = article.ViewCount,
                              posterImage = article.posterImage,
                              RegDate = article.RegDate,
                          }).ToList();
            return result;
        }

        //public async ValueTask DisposeAsync()
        //{
        //    //if (db != null)
        //    //{
        //    await db.DisposeAsync();
        //    //}
        //}

        public IRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            object repository;

            PrivateRepository.TryGetValue(typeof(TEntity), out repository);
            if (repository == null)
            {
                repository = new Repository<TEntity>(db, _mapper, _accessor);
                PrivateRepository.Add(typeof(TEntity), repository);
            }

            return (Repository<TEntity>)repository;
        }

        public async Task<bool> Save()
        {
            return await db.SaveChangesAsync() > 0;
        }
    }
}
