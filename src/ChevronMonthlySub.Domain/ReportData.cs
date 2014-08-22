namespace ChevronMonthlySub.Domain
{
	using System.Collections.Generic;

	public class ReportData<T> where T : OrderLine 
	{
		public TaxType TaxType { get; set; }
    public string PoNumber { get; set; }
		public string InvoiceNumber { get; set; }
		public Recipient Recipient { get; set; }

		public IEnumerable<StateOrderGroup<T>> States { get; set; }
	}
}