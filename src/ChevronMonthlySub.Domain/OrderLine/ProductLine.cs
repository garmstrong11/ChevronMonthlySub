namespace ChevronMonthlySub.Domain
{
	public class ProductLine : OrderLine
	{
		public ProductLine(FlexCelOrderLineDto dto) : base(dto)
		{
			ShipQty = dto.QtyShipped;
			InventoryItemId = dto.InventoryItemId;
			Boxes = 0;
		}

		public int Boxes { get; set; }
		public int ShipQty { get; private set; }
		public string InventoryItemId { get; private set; }
	}
}