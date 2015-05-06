namespace ChevronMonthlySub.Domain
{
	using System;

	public class FlexCelOrderLineDto
	{
		public DateTime DateShipped { get; set; }
		public string PoNumber { get; set; }
		public int OrderNumber { get; set; }
		public string InventoryItemId { get; set; }
		public decimal LineAmount { get; set; }
		public decimal TaxAmount { get; set; }
		public int QtyShipped { get; set; }
		public string LineDistribution { get; set; }
		public int FreightLineCount { get; set; }
		public int OrderLineCount { get; set; }
		public string LineDesc { get; set; }
		public int RowIndex { get; set; }
	}
}