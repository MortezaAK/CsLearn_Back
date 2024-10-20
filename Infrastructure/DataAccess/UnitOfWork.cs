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
            var cat = (from ap in db.ArticlesPermissions
                       join a in db.Articles on ap.ArticlesId equals a.Id
                       join c in db.Categories on ap.CategoriesId equals c.Id
                       where c.Id == id
                       select new CategoryDTO
                       {
                           GroupId = c.GroupId,
                           isDelete = c.isDelete,
                           Name = c.Name,
                       }).ToList();
            var ar = (from ap in db.ArticlesPermissions
                          join a in db.Articles on ap.ArticlesId equals a.Id
                          join c in db.Categories on ap.CategoriesId equals c.Id
                          where c.Id == id
                      select new ArticlesDTO
                      {
                          Description = a.Description,
                          posterImage = a.posterImage,
                          RegDate = a.RegDate,
                          Title = a.Title,
                      }).ToList();
            var result = (from ap in db.ArticlesPermissions
                          join a in db.Articles on ap.ArticlesId equals a.Id
                          join c in db.Categories on ap.CategoriesId equals c.Id
                          where c.Id == id
                          select new GetArticleBYCategoryDTO
                          {
                              GetAllArticle = ar,
                              GetAllCategory = cat
                          }).ToList();
            return result;
        }
        //کوئری براساس KeywordID نمایش مقاله ها
        public async Task<IEnumerable<GetArticleBYKeywordDTO>> GetArticleBYKeywordID(long id)
        {
            var ar = (from ak in db.ArticleKeywords
                      join a in db.Articles on ak.ArticleId equals a.Id
                      join k in db.Keywords on ak.KeywordId equals k.Id
                      where k.Id == id
                      select new ArticlesDTO
                      {
                          Description = a.Description,
                          posterImage = a.posterImage,
                          RegDate = a.RegDate,
                          Title = a.Title,
                      }).ToList();
            var key = (from ak in db.ArticleKeywords
                       join a in db.Articles on ak.ArticleId equals a.Id
                       join k in db.Keywords on ak.KeywordId equals k.Id
                       where k.Id == id
                       select new KeywordDTO
                       {
                           KeywordText = k.KeywordText
                       }).ToList();
            var result = (from ak in db.ArticleKeywords
                          join a in db.Articles on ak.ArticleId equals a.Id
                          join k in db.Keywords on ak.KeywordId equals k.Id
                          where k.Id == id
                          select new GetArticleBYKeywordDTO
                          {
                              GetAllArticle = ar,
                              GetAllKeyWords = key
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
