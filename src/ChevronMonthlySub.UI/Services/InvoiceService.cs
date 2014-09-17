namespace ChevronMonthlySub.UI.Services
{
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using Domain;
	using Reporter;

	public class InvoiceService : IInvoiceService
	{
		private readonly IShippingCostService _shippingCostService;
		private readonly IExtractor<FlexCelOrderLineDto> _extractor;
		private readonly ITemplatePathService _templatePathService;
		private string _sourcePath;

		public InvoiceService(
      IExtractor<FlexCelOrderLineDto> extractor, 
      IShippingCostService shippingCostService,
			ITemplatePathService templatePathService
			)
		{
			_extractor = extractor;
		  _shippingCostService = shippingCostService;
			_templatePathService = templatePathService;
		}

		// Setting the SourcePath triggers extraction and population of object model.
		// This allows us to validate the SourcePath separately, then build the model.
		public string SourcePath
		{
			get { return _sourcePath; }
			set
			{
				if (value.Equals(_sourcePath)) return;
				_sourcePath = value;
				ExtractAndPopulateModel(value);
			}
		}

		private void ExtractAndPopulateModel(string sourcePath)
		{
			_extractor.SourcePath = sourcePath;
			_templatePathService.OutputDirectory = Path.GetDirectoryName(sourcePath);
			var orderLines = _extractor.Extract().Select(CreateOrderLine).ToList();
			FreightLines = orderLines.OfType<FreightLine>().ToList();
			ProductLines = orderLines.OfType<ProductLine>().ToList();

			AssignBoxCountsToProductLines();
		}

		public IEnumerable<FreightLine> FreightLines { get; private set; }

		public IEnumerable<ProductLine> ProductLines { get; private set; }

		private static OrderLine CreateOrderLine(FlexCelOrderLineDto dto)
		{
			if (!string.IsNullOrWhiteSpace(dto.InventoryItemId))
			{
				return new ProductLine(dto);
			}

			return new FreightLine(dto);
		}

		private void AssignBoxCountsToProductLines()
		{
			var siteList = (from line in FreightLines
											group line by new { line.Destination, line.PoNumber } into sites
											select new
											{
												sites.Key.Destination,
												sites.Key.PoNumber,
												BoxCount = sites.Count()
											}).ToList();

			var firstProducts = from line in ProductLines
													group line by new { line.Destination, line.PoNumber } into shipment
													select shipment.First();

			foreach (var product in firstProducts)
			{
				var matches = siteList
					.FindAll(s => s.Destination == product.Destination && s.PoNumber == product.PoNumber);

				// Make sure at least one box is sent to each destination:
				product.Boxes = matches.Count == 0 ? 1 : matches.Sum(b => b.BoxCount);
			}
		}

		public IEnumerable<FreightPurchaseOrder> GetFreightPurchaseOrders(string invoiceId)
		{
			var query =
				from line in FreightLines
				group line by new { line.PoNumber, TaxGroup = line.TaxType }
					into orders
					select new FreightPurchaseOrder(new FlexcelChevronReportAdapter(_templatePathService))
					{
						PoNumber = orders.Key.PoNumber,
						TaxType = orders.Key.TaxGroup,
						InvoiceNumber = invoiceId,
						Requestor = Requestor.UnknownRequestor,
						States =
							from order in orders
							group order by order.State
								into states
								select new FreightStateGroup
								{
									StateName = states.Key,
									OrderLines = states.ToList()
								}
					};

			return query.ToList();
		}

		public IEnumerable<ProductPurchaseOrder> GetProductPurchaseOrders(string invoiceId)
		{
			var query =
				from line in ProductLines
				group line by new { line.PoNumber, TaxGroup = line.TaxType }
				into orders
				select new ProductPurchaseOrder(new FlexcelChevronReportAdapter(_templatePathService))
					{
					PoNumber = orders.Key.PoNumber,
					TaxType = orders.Key.TaxGroup,
					InvoiceNumber = invoiceId,
					Requestor = Requestor.UnknownRequestor,
					States =
					from order in orders
					group order by order.State
					into states
					select new ProductStateGroup(_shippingCostService)
						{
						StateName = states.Key,
						OrderLines = states.ToList()
						}
					};

			return query.ToList();
		}

		public int SalesLines
		{
			get { return ProductLines.Count(); }
		}

		public decimal FreightFee
		{
			get { return FreightLines.Sum(p => p.LineAmount + p.TaxAmount); }
		}

		public int PickPackCount
		{
			get { return ProductLines.Sum(s => s.ShipQty); }
		}

		public decimal PickPackFee
		{
			get { return PickPackCount * _shippingCostService.PickPackFee; }
		}

		public int BoxCount
		{
			get { return FreightLines.Count(); }
		}

		public decimal BoxFee
		{
			get { return BoxCount * _shippingCostService.BoxFee; }
		}

		public decimal TotalInvoice
		{
			get { return FreightFee + BoxFee + PickPackFee; }
		}
	}
}