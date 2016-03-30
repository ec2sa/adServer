using ADServerDAL.Abstract;
using ADServerDAL.Concrete;
using Ninject;
using Ninject.Syntax;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Web.Http.Dependencies;


namespace ADServerManagementWebApplication.Infrastructure
{
	public class NinjectDependencyScope : IDependencyScope
	{
		#region - Fields -
		private IResolutionRoot resolver;
		#endregion

		#region - Constructors -
		internal NinjectDependencyScope(IResolutionRoot resolver)
		{
			Contract.Assert(resolver != null);
			this.resolver = resolver;
		}
		#endregion

		#region - Public methods -
		public void Dispose()
		{
			IDisposable disposable = resolver as IDisposable;
			if (disposable != null)
				disposable.Dispose();
			resolver = null;
		}

		public object GetService(Type serviceType)
		{
			if (resolver == null)
				throw new ObjectDisposedException("this", "This scope has already been disposed");

			return resolver.TryGet(serviceType);
		}

		public IEnumerable<object> GetServices(Type serviceType)
		{
			if (resolver == null)
				throw new ObjectDisposedException("this", "This scope has already been disposed");

			return resolver.GetAll(serviceType);
		}
		#endregion
	}

	public class NinjectDependencyResolver : NinjectDependencyScope, IDependencyResolver
	{
		#region - Fields -
		private IKernel kernel;
		#endregion

		#region - Constructors -
		public NinjectDependencyResolver(IKernel kernel)
			: base(kernel)
		{
			this.kernel = kernel;
			AddBindings();
		}
		#endregion

		#region - Public methods -
		public IDependencyScope BeginScope()
		{
			return new NinjectDependencyScope(kernel.BeginBlock());
		}
		#endregion

		#region - Private methods -
		/// <summary>
		/// Zarejestrowanie repozytoriów
		/// </summary>
		private void AddBindings()
		{
			kernel.Bind<ICampaignRepository>().To<EFCampaignRepository>();
			kernel.Bind<ICategoryRepository>().To<EFCategoryRepository>();
			kernel.Bind<IMultimediaObjectRepository>().To<EFMultimediaObjectRepository>();
			kernel.Bind<ITypeRepository>().To<EFTypeRepository>();
			kernel.Bind<IPriorityRepository>().To<EFPriorityRepository>();
			kernel.Bind<IStatisticRepository>().To<EFStatisticRepository>();
			kernel.Bind<IMultimediaObject_CampaignRepository>().To<EFMultimediaObject_CampaignRepository>();
			kernel.Bind<IStatistics_CampaignRepository>().To<EFStatistic_CampaignRepository>();
			kernel.Bind<IStatistics_CategoryRepository>().To<EFStatistic_CategoryRepository>();
			kernel.Bind<IUsersRepository>().To<EFUsersRepository>();
			kernel.Bind<IRoleRepository>().To<EFRoleRepository>();
			kernel.Bind<IDeviceRepository>().To<EFDeviceRepository>();
		}
		#endregion
	}
}