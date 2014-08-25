namespace ChevronMonthlySub.Domain
{
	public abstract class PurchaseOrder
	{
	  protected IChevronReportAdapter ReportAdapter;

	  protected PurchaseOrder(
      IChevronReportAdapter reportAdapter)
	  {
	    ReportAdapter = reportAdapter;
	  }

    public TaxType TaxType { get; set; }
    public string PoNumber { get; set; }
		public string InvoiceNumber { get; set; }
		public Recipient Recipient { get; set; }

	  public virtual void ConfigureReport()
	  {
	    ReportAdapter.SetValue("TaxType", TaxType);
      ReportAdapter.SetValue("PoNumber", PoNumber);
      ReportAdapter.SetValue("InvoiceNumber", InvoiceNumber);
      ReportAdapter.SetValue("Recipient", Recipient);
	  }

	  public override string ToString()
	  {
	    return string.Format("{0} {1} {2} {3}", InvoiceNumber, PoNumber, TaxType, Recipient.Initials);
	  }
	}
}