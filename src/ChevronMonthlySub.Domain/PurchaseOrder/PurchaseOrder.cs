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
		public string Description { get; set; }

	  public virtual void ConfigureReport()
	  {
	    ReportAdapter.SetValue("TaxType", TaxType);
      ReportAdapter.SetValue("PoNumber", PoNumber);
      ReportAdapter.SetValue("InvoiceNumber", InvoiceNumber);
      ReportAdapter.SetValue("RecipientInit", Recipient.Initials);
			ReportAdapter.SetValue("RecipientName", Recipient.Name);
			ReportAdapter.SetValue("Description", Description);
	  }

	  public override string ToString()
	  {
	    return string.Format("{0} {1} {2} {3}", InvoiceNumber, PoNumber, Recipient.Initials, TaxType);
	  }
	}
}