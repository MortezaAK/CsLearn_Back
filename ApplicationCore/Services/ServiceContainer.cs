using ApplicationCore.DTOs;
using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.DataAccess;
using ApplicationCore.Interfaces.Services;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class ServiceContainer : IServiceContainer
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor accessor;
        private readonly BaseServiceExceptionDTO serviceException;
        private readonly Dictionary<Type, object> PrivateServices;
        public ServiceContainer(IUnitOfWork _uow, IMapper _mapper, IHttpContextAccessor _accessor)
        {
            uow = _uow ?? throw new ArgumentNullException(nameof(_uow));
            mapper = _mapper ?? throw new ArgumentNullException(nameof(_mapper));
            accessor = _accessor ?? throw new ArgumentNullException(nameof(_accessor));
            PrivateServices = new Dictionary<Type, object>();
            serviceException = new BaseServiceExceptionDTO();
        }

        //public ServiceContainer(IUnitOfWork _uow, IMapper _mapper, IHttpContextAccessor accessor)
        //{
        //    uow = _uow;
        //    mapper = _mapper;
        //    // this.smsProvider = smsProvider;
        //    this.accessor = accessor;
        //    PrivateServices = new Dictionary<Type, object>();
        //    this.serviceException = new BaseServiceExceptionDTO();
        //}
        public bool Status => serviceException.Status;

        public string Code => serviceException.Code;

        public string Message => serviceException.Message;

        public ICategories Category 
        {
            get
            {
                object service;

                PrivateServices.TryGetValue(typeof(ICategories), out service);
                if (service == null)
                {
                    service = new CategoryServices(uow, mapper, serviceException);
                    PrivateServices.Add(typeof(CategoryServices), service);
                }

                return (CategoryServices)service;
            }
        }


        public IArticle Article
        {
            get
            {
                object service;

                PrivateServices.TryGetValue(typeof(ICategories), out service);
                if (service == null)
                {
                    service = new ArticleService(uow, mapper, serviceException);
                    PrivateServices.Add(typeof(CategoryServices), service);
                }

                return (ArticleService)service;
            }
        }
        //public async ValueTask DisposeAsync()
        //{
        //    await uow.DisposeAsync();
        //    GC.SuppressFinalize(this);
        //}
    }
}
