namespace ChevronMonthlySub.Domain
{
	public class FreightLine : OrderLine
	{
		public FreightLine(FlexCelOrderLineDto dto) : base(dto)
		{
			LineAmount = dto.LineAmount;
			TaxAmount = dto.TaxAmount;
		}

		public decimal LineAmount { get; private set; }
		public decimal TaxAmount {get; private set; }
	}
}