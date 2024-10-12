using ApplicationCore.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface ICategories
    {
        Task<bool> AddCategory(CategoryDTO categoryDTO);
        Task<bool> UpdateCategory(long id, CategoryDTO categoryDTO);
        Task<CategoryDTO> FindCategory(long id);
        Task<List<CategoryDTO>> GetAllCategories();
        Task<bool> DeleteCategory(long id);

    }
}
