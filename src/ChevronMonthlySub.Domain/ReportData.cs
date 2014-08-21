namespace ChevronMonthlySub.Domain
{
	using System.Collections.Generic;

	public class ReportData<T> where T : OrderLine 
	{
		private string _recipient = "KH";

		public TaxType TaxType { get; set; }
    public string PoNumber { get; set; }
		public string InvoiceNumber { get; set; }

		public string Recipient
		{
			get { return _recipient; }
			set { _recipient = value; }
		}

		public IEnumerable<StateOrderGroup<T>> States { get; set; }
	}
}