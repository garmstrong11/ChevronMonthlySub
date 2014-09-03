namespace ChevronMonthlySub.Domain
{
  using System.Collections.Generic;
  using System.Linq;

	public class ProductStateGroup : StateGroup
	{
		private readonly IShippingCostService _shipCostService;
		private IEnumerable<ProductLine> _orderLines;

		public ProductStateGroup(IShippingCostService shipCostService)
		{
			_shipCostService = shipCostService;
		}

		public IEnumerable<ProductLine> OrderLines
		{
			get { return _orderLines.OrderBy(p => p.OrderNumber); }
			set { _orderLines = value; }
		}

		public int ShipQty
		{
			get { return OrderLines.Sum(t => t.ShipQty); }
		}

		public int BoxCount
		{
			get { return OrderLines.Sum(t => t.Boxes); }
		}

		public decimal PickPackCharges
		{
			get { return ShipQty * PickPackFee;}
		}

		public decimal BoxCharges
		{
			get { return BoxCount * BoxFee; }
		}

		public decimal BoxFee
		{
			get { return _shipCostService.BoxFee; }
		}

		public decimal PickPackFee
		{
			get { return _shipCostService.PickPackFee; }
		}
	}
}