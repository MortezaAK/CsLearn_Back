using ApplicationCore.DTOs;
using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.DataAccess;
using ApplicationCore.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class CategoryServices :BaseService, ICategories
    {
        //IRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity, new();
        public CategoryServices(IUnitOfWork _uow, IMapper _mapper, BaseServiceExceptionDTO serviceException) : base(_uow, _mapper, serviceException)
        {
            
        }
       

        public async Task<bool> AddCategory(CategoryDTO categoryDTO)
        {
            try
            {
                Categories categories = mapper.Map<Categories>(categoryDTO);
                bool result = await uow.Repository<Categories>().AddAsync(categories);

                if (!result)
                    return NotOKResponse(false, "000014", "error");

                return OKResponse(true, "Added successfully");
            }
            catch (Exception ex)
            {
                return NotOKResponse(false, "000014", ex.Message);
            }
        }

        public async Task<bool> DeleteCategory(long id)
        {
            var result = await uow.Repository<Categories>().DeleteAsync(id);
            if (result == null)
            {
                return NotOKResponse(false, "000013", "Category not found");
            }
            return result;
        }

        public async Task<CategoryDTO> FindCategory(long id)
        {

            var result = await uow.Repository<Categories>().GetByIdAsync(id);

            if(result == null)
                return null;

            var categoryDTO = mapper.Map<CategoryDTO>(result);

            return categoryDTO;
        }

        public async Task<List<CategoryDTO>> GetAllCategories()
        {
            
            var categories = await uow.Repository<Categories>().GetAllAsync();
           
           
            var categoryDTOs = categories.Where(c => c.isDelete == 0).Select(category => mapper.Map<CategoryDTO>(category)).ToList();

            return categoryDTOs;
        }

        public async Task<bool> UpdateCategory(long id, CategoryDTO categoryDTO)
        {
            try
            {
                
                var existingCategory = await uow.Repository<Categories>().GetByIdAsync(id);
                if (existingCategory == null)
                {
                    return NotOKResponse(false, "000013", "Category not found");
                }

                
                existingCategory.Name = categoryDTO.Name; 
                existingCategory.isDelete = categoryDTO.isDelete;
                existingCategory.GroupId = categoryDTO.GroupId;

                return await uow.Repository<Categories>().UpdateAsync(existingCategory); 
            }
            catch (Exception ex)
            {
                return NotOKResponse(false, "000014", ex.Message);
            }
        }


    }

}
