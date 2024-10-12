using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.DTOs
{
    public class BaseServiceExceptionDTO
    {
        public bool Status { get; set; }

        public string Code { get; set; }

        public string Message { get; set; }

        internal void HandleException(Exception ex)
        {
            throw new NotImplementedException();
        }
    }
}
