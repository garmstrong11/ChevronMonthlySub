namespace ChevronMonthlySub.UI.Services
{
	using System.Collections.Generic;
	using System.Linq;
	using Domain;

	public class OrderKeyService : IOrderKeyService
	{
		private readonly IOrderKeyFactory _orderKeyFactory;
		private readonly IExtractor<OrderKeyRowDto> _extractor;

		public OrderKeyService(
			ITemplatePathService templatePathService, 
			IOrderKeyFactory orderKeyFactory, 
			IExtractor<OrderKeyRowDto> extractor)
		{
			_orderKeyFactory = orderKeyFactory;
			_extractor = extractor;

			_extractor.SourcePath = templatePathService.OrderKeyPath;
		}

		public Dictionary<string, OrderKey> AcquireOrderKeys()
		{
			return _extractor.Extract()
				.Select(p => _orderKeyFactory.Create(p))
				.ToDictionary(k => k.ProductId);
		}
	}
}