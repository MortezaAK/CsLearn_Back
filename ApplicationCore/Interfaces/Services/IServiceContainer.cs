using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.Services
{
    public interface IServiceContainer 
    {
        bool Status { get; }

        string Code { get; }

        string Message { get; }

        ICategories Category { get; }
        IArticle Article { get; }
    }
}
