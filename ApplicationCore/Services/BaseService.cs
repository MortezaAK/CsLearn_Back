using ApplicationCore.DTOs;
using ApplicationCore.Interfaces.DataAccess;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public abstract class BaseService 
    {
        protected readonly IUnitOfWork uow;
        protected readonly IMapper mapper;

        protected BaseServiceExceptionDTO serviceException;

        protected string Message { get => serviceException.Message; }
        protected string Code { get => serviceException.Code; }
        protected bool Status { get => serviceException.Status; }

        public BaseService(IUnitOfWork _uow, IMapper _mapper, BaseServiceExceptionDTO serviceException)
        {
            uow = _uow;
            mapper = _mapper;
            this.serviceException = serviceException;
        }

        //000000
        protected T NotOKResponse<T>(T result, string errorCode, string errorMessage, bool status = false)
        {
            serviceException.Status = status;
            serviceException.Code = errorCode;
            serviceException.Message = errorMessage;
            return result;
        }

        protected T OKResponse<T>(T result, string code = null, string message = null, bool status = true)
        {
            serviceException.Status = status;
            serviceException.Code = code;
            serviceException.Message = message;
            return result;
        }

        //public async ValueTask DisposeAsync()
        //{
        //    GC.SuppressFinalize(this);
        //}
    }
}
