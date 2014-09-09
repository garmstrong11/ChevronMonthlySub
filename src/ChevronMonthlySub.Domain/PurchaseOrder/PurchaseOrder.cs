namespace ChevronMonthlySub.Domain
{
	using System.Collections.Generic;

	public abstract class PurchaseOrder
	{
	  protected IChevronReportAdapter ReportAdapter;

		protected PurchaseOrder(
      IChevronReportAdapter reportAdapter)
	  {
	    ReportAdapter = reportAdapter;
	  }

    public TaxType TaxType { get; set; }
		public string InvoiceNumber { get; set; }
		public Requestor Requestor { get; set; }
		public string Description { get; set; }
		public string PoNumber { get; set; }

		public virtual void ConfigureReport()
	  {
	    ReportAdapter.SetValue("TaxType", TaxType);
      ReportAdapter.SetValue("PoNumber", PoNumber);
      ReportAdapter.SetValue("InvoiceNumber", InvoiceNumber);
      ReportAdapter.SetValue("RecipientInit", Requestor.Initials);
			ReportAdapter.SetValue("RecipientName", Requestor.Name);
			ReportAdapter.SetValue("Description", Description);
	  }

		public abstract void UpdatePoNumber(string newPo);
	
		public abstract void RunReports();

		public abstract void UpdateWithOrderKey(Dictionary<string, OrderKey> orderKeys);

	  public override string ToString()
	  {
	    return string.Format("{0} {1} {2} {3}", InvoiceNumber, PoNumber, Requestor.Initials, TaxType);
	  }
	}
}