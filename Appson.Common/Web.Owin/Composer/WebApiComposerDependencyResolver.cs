using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Dependencies;
using Appson.Composer;
using Appson.Composer.WebApi.Resolver;

namespace Appson.Common.Web.Owin.Composer
{
    [Component]
	public class WebApiComposerDependencyResolver : IWebApiDependencyResolverContract
	{
		private readonly IDependencyResolver _baseResolver;

		[ComponentPlug]
		public IComposer Composer { get; set; }

		public WebApiComposerDependencyResolver()
		{
			_baseResolver = GlobalConfiguration.Configuration.DependencyResolver;
		}

		public void Dispose()
		{
			_baseResolver.Dispose();
		}

		public object GetService(Type serviceType)
		{
			object result = null;

			if (ComponentContextUtils.HasContractAttribute(serviceType))
				result = Composer.GetComponent(serviceType);

			if (result == null)
			{
				result = _baseResolver.GetService(serviceType);

				if (result != null)
					Composer.InitializePlugs(result, result.GetType());
			}

			return result;
		}

		public IEnumerable<object> GetServices(Type serviceType)
		{
			var baseResult = _baseResolver.GetServices(serviceType).ToList();

			baseResult.ForEach(o => Composer.InitializePlugs(o, o.GetType()));

			if (ComponentContextUtils.HasContractAttribute(serviceType))
				return Composer.GetAllComponents(serviceType).Concat(baseResult);

			return baseResult;
		}

		public IDependencyScope BeginScope()
		{
			return this;
		}
	}
}