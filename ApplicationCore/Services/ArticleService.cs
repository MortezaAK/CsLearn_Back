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
                    ArticleId = newArticle.Id,
                    KeywordId = keyword.Id
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
        public async Task<IEnumerable<Articles>> GetAllArticle()
        {
            return await uow.Repository<Articles>().GetAllAsync();
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
        //Error dare bayad hal besh - Akbari(hal mikonm)
        public async Task<bool> UpdateArticleAsync(ArticleDTO articlesDTO)
        {
            try
            {
                // بررسی اینکه تمام CategoryIds در دیتابیس وجود داشته باشند
                var existingCategories = await uow.Repository<Categories>()
                    .FindAsync(c => articlesDTO.CategoryIds.Contains(c.Id));

                if (existingCategories.Count() != articlesDTO.CategoryIds.Count)
                {
                    return NotOKResponse(false, "CategoryNotFound", "برخی از دسته‌بندی‌ها وجود ندارند.");
                }

                // بررسی اینکه تمام KeywordIds در دیتابیس وجود داشته باشند
                var existingKeywords = await uow.Repository<Keyword>()
                    .FindAsync(k => articlesDTO.KeywordIds.Contains(k.Id));

                if (existingKeywords.Count() != articlesDTO.KeywordIds.Count)
                {
                    return NotOKResponse(false, "KeywordNotFound", "برخی از کلمات کلیدی وجود ندارند.");
                }

                // نقشه‌برداری از ArticleDTO به Articles
                var newArticle = mapper.Map<Articles>(articlesDTO);
                var result = await uow.Repository<Articles>().UpdateAsync(newArticle);

                // ایجاد ارتباط بین مقاله و دسته‌بندی‌ها (ArticlePermissions)
                var articlePermissions = existingCategories.Select(category => new ArticlesPermission
                {
                    ArticlesId = newArticle.Id,
                    CategoriesId = category.Id
                }).ToList();

                foreach (var articlePermission in articlePermissions)
                {
                    await uow.Repository<ArticlesPermission>().AddAsync(articlePermission);
                }

                // ایجاد ارتباط بین مقاله و کلمات کلیدی
                var articleKeywords = existingKeywords.Select(keyword => new ArticleKeyword
                {
                    ArticleId = newArticle.Id,
                    KeywordId = keyword.Id
                }).ToList();

                foreach (var articleKeyword in articleKeywords)
                {
                    await uow.Repository<ArticleKeyword>().AddAsync(articleKeyword);
                }

                // ذخیره تغییرات نهایی در دیتابیس
                await uow.Save();
                return OKResponse(true, "Success", "مقاله با موفقیت به‌روزرسانی شد.");
            }
            catch (Exception ex)
            {
                // لاگ خطا برای خطایابی بهتر
                // Log the exception here using your logging framework
                return NotOKResponse(false, "Error", "خطا در هنگام به‌روزرسانی مقاله رخ داد.");
            }
        }

    }

}

