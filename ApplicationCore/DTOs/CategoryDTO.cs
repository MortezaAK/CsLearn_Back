using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.DTOs
{
    public class CategoryDTO
    {
        public string Name { get; set; }
        public int GroupId { get; set; }
        public int isDelete { get; set; }
    }
}
