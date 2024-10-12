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
    public class ArticleService : BaseService,IArticle
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
    }


}

