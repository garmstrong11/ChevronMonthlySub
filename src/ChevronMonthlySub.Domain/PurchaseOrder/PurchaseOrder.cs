namespace ChevronMonthlySub.Domain
{
	using System.Collections.Generic;

	public class PurchaseOrder 
	{
		public TaxType TaxType { get; set; }
    public string PoNumber { get; set; }
		public string InvoiceNumber { get; set; }
		public Recipient Recipient { get; set; }

		public IEnumerable<StateGroup> States { get; set; } 
	}
}