using System;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace VagrantAtlas.WebApi
{
    public class SingletonRepositoriesHttpControllerActivator : IHttpControllerActivator
    {
        private readonly IBoxRepository _boxRepository;

        public SingletonRepositoriesHttpControllerActivator(IBoxRepository boxRepository)
        {
            if (boxRepository == null) throw new ArgumentNullException("boxRepository");

            _boxRepository = boxRepository;
        }

        public IHttpController Create(HttpRequestMessage request, HttpControllerDescriptor controllerDescriptor, Type controllerType)
        {
            if (controllerType == typeof(BoxesController))
            {
                return new BoxesController(_boxRepository);
            }

            if (controllerType == typeof(AtlasController))
            {
                return new AtlasController(_boxRepository);
            }

            throw new NotImplementedException();
        }
    }
}