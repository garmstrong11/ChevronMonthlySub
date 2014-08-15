namespace ChevronMonthlySub.Domain
{
	public class LineFactory
	{		
		public OrderLine Create(FlexCelOrderLineDto dto)
		{
			if (!string.IsNullOrWhiteSpace(dto.InventoryItemId)) {
				return new ProductLine(dto);
			}

			return new FreightLine(dto);
		}
	}
}