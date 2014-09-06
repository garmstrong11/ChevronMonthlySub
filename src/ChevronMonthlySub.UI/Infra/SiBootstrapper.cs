namespace ChevronMonthlySub.UI.Infra
{
	using System;
	using System.Collections.Generic;
	using System.Windows;
	using Caliburn.Micro;
	using Domain;
	using Extractor;
	using Reporter;
	using Services;
	using SimpleInjector;
	using ViewModels;

	public class SiBootstrapper : BootstrapperBase
	{
		private Container _container;

		public SiBootstrapper()
		{
			Initialize();
			LogManager.GetLog = type => new DebugLogger(type);
		}

		protected override void Configure()
		{
			_container = new Container();

			_container.Register<IWindowManager, WindowManager>();
			_container.Register<IShell, ShellViewModel>();
			_container.RegisterSingle<IEventAggregator, EventAggregator>();

			_container.RegisterSingle<IRequestorService, RequestorService>();
			_container.RegisterSingle<IShippingCostService, ShippingCostService>();
			_container.RegisterSingle<ITemplatePathService, TemplatePathService>();
			_container.RegisterSingle<IPurchaseOrderService, PurchaseOrderService>();
			_container.RegisterSingle<IOrderKeyFactory, OrderKeyFactory>();

			_container.Register<IExtractor<FlexCelOrderLineDto>, OrderLineExtractor>();
			_container.Register<IChevronReportAdapter, FlexcelChevronReportAdapter>();

			_container.Verify();
		}

		protected override object GetInstance(Type service, string key)
		{
			return _container.GetInstance(service);
		}

		protected override IEnumerable<object> GetAllInstances(Type service)
		{
			return _container.GetAllInstances(service);
		}

		protected override void BuildUp(object instance)
		{
			_container.InjectProperties(instance);
		}

		protected override void OnStartup(object sender, StartupEventArgs e)
		{
			DisplayRootViewFor<IShell>();
		}
	}
}