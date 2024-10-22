using ApplicationCore.DTOs;
using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.DataAccess;
using ApplicationCore.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class ArticleService : BaseService, IArticle
    {
        public ArticleService(IUnitOfWork _uow, IMapper _mapper, BaseServiceExceptionDTO serviceException) : base(_uow, _mapper, serviceException)
        {
        }

        public async Task<bool> AddArticleAsync(ArticleDTO articleDto)
        {
            try
            {
                // بررسی اینکه تمام CategoryIds در دیتابیس وجود داشته باشند
                var existingCategories = new List<Categories>();
                foreach (var categoryId in articleDto.CategoryIds)
                {
                    var category = await uow.Repository<Categories>().GetByIdAsync(categoryId);
                    if (category != null)
                    {
                        existingCategories.Add(category);
                    }
                }

                if (existingCategories.Count != articleDto.CategoryIds.Count)
                {
                    // اگر دسته‌بندی‌های درخواستی در دیتابیس یافت نشدند
                    return NotOKResponse(false, "CategoryNotFound", "برخی از دسته‌بندی‌ها وجود ندارند.");
                }

                // بررسی اینکه تمام KeywordIds در دیتابیس وجود داشته باشند
                var existingKeywords = new List<Keyword>();
                foreach (var keywordId in articleDto.KeywordIds)
                {
                    var keyword = await uow.Repository<Keyword>().GetByIdAsync(keywordId);
                    if (keyword != null)
                    {
                        existingKeywords.Add(keyword);
                    }
                }

                if (existingKeywords.Count != articleDto.KeywordIds.Count)
                {
                    // اگر کلمات کلیدی درخواستی در دیتابیس یافت نشدند
                    return NotOKResponse(false, "KeywordNotFound", "برخی از کلمات کلیدی وجود ندارند.");
                }

                // ایجاد یک نمونه از Article با استفاده از داده‌های DTO
                var newArticle = mapper.Map<Articles>(articleDto);

                // اضافه کردن مقاله به دیتابیس
                await uow.Repository<Articles>().AddAsync(newArticle);
                await uow.Save();

                // ایجاد ارتباط بین مقاله و دسته‌بندی‌ها (ArticlePermissions)
                var articlePermissions = existingCategories.Select(category => new ArticlesPermission
                {
                    ArticlesId = newArticle.Id,
                    CategoriesId = category.Id
                }).ToList();

                await uow.Repository<ArticlesPermission>().AddRangeAsync(articlePermissions);

                // ایجاد ارتباط بین مقاله و کلمات کلیدی
                var articleKeywords = existingKeywords.Select(keyword => new ArticleKeyword
                {
                    ArticlesId = newArticle.Id,
                    KeywordsId = keyword.Id
                }).ToList();

                await uow.Repository<ArticleKeyword>().AddRangeAsync(articleKeywords);

                // ذخیره تغییرات نهایی در دیتابیس
                await uow.Save();
                return OKResponse(true, "Success", "مقاله با موفقیت اضافه شد.");
            }
            catch (Exception ex)
            {

                serviceException.HandleException(ex);
                return NotOKResponse(false, "AddArticleError", "خطایی در اضافه کردن مقاله رخ داد.");
            }
        }
        //پاک کردن مقاله ها بر اساس id
        public async Task<bool> DeleteArticle(long articleId)
        {
            try
            {
                var article = await uow.Repository<Articles>().DeleteAsync(articleId);
                return article;
            }
            catch (Exception)
            {

                throw;
            }
        }
        //نمایش تمامی مقاله ها
        //public async Task<IEnumerable<ArticleByDTO>> GetAllArticle()
        //{
        //    var articles = await uow.Repository<Articles>().GetAllAsync();

        //    return articles.Select(article => new ArticleByDTO
        //    {
        //        Id = article.Id,
        //        Title = article.Title,
        //        CategoryIds = article.ArticlesPermissions?.Select(ap => new CategoryInOtherTableDTO
        //        {
        //            Id = ap.CategoriesId,
        //            Name = ap.Category?.Name // بررسی null برای Category
        //        }).ToList() ?? new List<CategoryInOtherTableDTO>(), // اگر ArticlesPermissions نال باشد، یک لیست خالی برمی‌گرداند
        //        KeywordIds = article.ArticleKeywords?.Select(ak => new KeywordDTO
        //        {
        //            Id = ak.KeywordsId,
        //            KeywordText = ak.Keyword?.KeywordText // بررسی null برای Keyword
        //        }).ToList() ?? new List<KeywordDTO>() // اگر ArticleKeywords نال باشد، یک لیست خالی برمی‌گرداند
        //    }).ToList(); // تبدیل به لیست
        //}

        public async Task<IEnumerable<ArticleByDTO>> GetAllArticle()
        {
            var articles = await uow.Repository<Articles>()
        .GetAllWithIncludesAsync("ArticlesPermissions.Category", "ArticleKeywords.Keyword"); // شامل وابستگی‌ها


            return articles.Select(article => new ArticleByDTO
            {
                Id = article.Id,
                Title = article.Title,
                CategoryIds = article.ArticlesPermissions?.Select(ap => new CategoryInOtherTableDTO
                {
                    Id = ap.CategoriesId,
                    Name = ap.Category?.Name // بررسی null برای Category
                }).ToList() ?? new List<CategoryInOtherTableDTO>(),
                KeywordIds = article.ArticleKeywords?.Select(ak => new KeywordDTO
                {
                    Id = ak.KeywordsId,
                    KeywordText = ak.Keyword?.KeywordText // بررسی null برای Keyword
                }).ToList() ?? new List<KeywordDTO>()
            }).ToList();

        }





        //نمایش مقاله ها براساس CategoryID
        public async Task<IEnumerable<GetArticleBYCategoryDTO>> GetArticleBYCategoryID(long id)
        {
            return await uow.GetArticleBYCategoryID(id);
        }
        //نمایش مقاله ها براساس id
        public async Task<Articles> GetArticleById(long id)
        {
            return await uow.Repository<Articles>().GetByIdAsync(id);
        }
        //نمایش مقاله ها براساس KeywordID
        public async Task<IEnumerable<GetArticleBYKeywordDTO>> GetArticleBYKeywordID(long id)
        {
            return await uow.GetArticleBYKeywordID(id);
        }
        // بروزرسانی مقاله ها

        public async Task<bool> UpdateArticleAsync(ArticleDTO articlesDTO, long articleID)
        {
            try
            {
                // پیدا کردن مقاله موجود
                var existingArticle = await uow.Repository<Articles>().GetByIdAsync(articleID);
                if (existingArticle == null)
                {
                    return NotOKResponse(false, "ArticleNotFound", "مقاله‌ای با این شناسه یافت نشد.");
                }

                // به‌روزرسانی اطلاعات مقاله
                existingArticle.Title = articlesDTO.Title;
                existingArticle.Description = articlesDTO.Description;
                existingArticle.RegDate = articlesDTO.RegDate;
                existingArticle.posterImage = articlesDTO.PosterImage;
                existingArticle.isDelete = articlesDTO.IsDelete;
                existingArticle.LikeCount = articlesDTO.LikeCount;
                existingArticle.ViewCount = articlesDTO.ViewCount;

                // به‌روزرسانی ارتباطات مقاله با دسته‌بندی‌ها
                var existingArticlePermissions = await uow.Repository<ArticlesPermission>()
                    .FindAsync(ap => ap.ArticlesId == articleID);
                foreach (var permission in existingArticlePermissions)
                {
                    await uow.Repository<ArticlesPermission>().DeleteAsync(permission.Id);
                }

                var existingCategories = new List<Categories>();
                foreach (var categoryId in articlesDTO.CategoryIds)
                {
                    var category = await uow.Repository<Categories>().GetByIdAsync(categoryId);
                    if (category != null)
                    {
                        existingCategories.Add(category);
                    }
                }
                if (existingCategories.Count != articlesDTO.CategoryIds.Count)
                {
                    return NotOKResponse(false, "CategoryNotFound", "برخی از دسته‌بندی‌ها وجود ندارند.");
                }

                var articlePermissions = existingCategories.Select(category => new ArticlesPermission
                {
                    ArticlesId = articleID,
                    CategoriesId = category.Id
                }).ToList();
                foreach (var articlePermission in articlePermissions)
                {
                    await uow.Repository<ArticlesPermission>().AddAsync(articlePermission);
                }

                // به‌روزرسانی ارتباطات مقاله با کلمات کلیدی
                var existingArticleKeywords = await uow.Repository<ArticleKeyword>()
                    .FindAsync(ak => ak.ArticlesId == articleID);
                foreach (var keyword in existingArticleKeywords)
                {
                    await uow.Repository<ArticleKeyword>().DeleteAsync(keyword.ID);
                }

                var existingKeywords = new List<Keyword>();
                foreach (var keywordId in articlesDTO.KeywordIds)
                {
                    var keyword = await uow.Repository<Keyword>().GetByIdAsync(keywordId);
                    if (keyword != null)
                    {
                        existingKeywords.Add(keyword);
                    }
                }
                if (existingKeywords.Count != articlesDTO.KeywordIds.Count)
                {
                    return NotOKResponse(false, "KeywordNotFound", "برخی از کلمات کلیدی وجود ندارند.");
                }

                var articleKeywords = existingKeywords.Select(keyword => new ArticleKeyword
                {
                    ArticlesId = articleID,
                    KeywordsId = keyword.Id
                }).ToList();
                foreach (var articleKeyword in articleKeywords)
                {
                    await uow.Repository<ArticleKeyword>().AddAsync(articleKeyword);
                }

                // ذخیره تغییرات نهایی
                await uow.Save();

                return OKResponse(true, "Success", "مقاله با موفقیت به‌روزرسانی شد.");
            }
            catch (Exception ex)
            {
                // ثبت خطا برای خطایابی بهتر
                return NotOKResponse(false, "Error", "خطا در هنگام به‌روزرسانی مقاله رخ داد.");
            }
        }

    }

}

