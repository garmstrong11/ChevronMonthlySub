namespace ChevronMonthlySub.Reporter
{
	using Domain;
	using FlexCel.Report;

	public class ChevronReport
	{
	  private readonly ITemplatePathService _templatePathService;
    
    public PurchaseOrder PurchaseOrder { get; set; }
    public bool IsSummary { get; set; }
    public string DestinationDirectory { get; set; }

	  public ChevronReport(ITemplatePathService templatePathService)
	  {
	    _templatePathService = templatePathService;
	  }

		public FlexCelReport CreateAndConfigureReport()
		{
		  var report = new FlexCelReport(true);
      report.SetValue("TaxType", PurchaseOrder.TaxType);
			report.SetValue("PoNumber", PurchaseOrder.PoNumber);
			report.SetValue("InvoiceNumber", PurchaseOrder.InvoiceNumber);
			report.SetValue("Recipient", PurchaseOrder.Recipient);

		  var fpo = PurchaseOrder as FreightPurchaseOrder;
		  if (fpo != null)
		  {
        report.AddTable("States", fpo.States);
        report.SetValue("SubTotal", fpo.LineAmountSubtotal);
        report.SetValue("TaxTotal", fpo.TaxAmountSubtotal);
		  }

		  var ppo = PurchaseOrder as ProductPurchaseOrder;
		  if (ppo == null) return report;

		  report.AddTable("States", ppo.States);
		  report.SetValue("PickPackTotal", ppo.PickPackCharge);
		  report.SetValue("BoxTotal", ppo.BoxCharge);

		  return report;
		}

		public string Prefix
		{
		  get { return IsSummary ? "Summary" : "Chevron FG"; }
		}

	  public string Suffix
	  {
	    get { return PurchaseOrder is FreightPurchaseOrder ? " FRT.xlsx" : ".xlsx"; }
	  }

	  public string GetOutputFilename
	  {
	    get
	    {
	      return string.Format("{0} {1} {2} {3} {4}{5}",
	        Prefix,
	        PurchaseOrder.InvoiceNumber,
	        PurchaseOrder.PoNumber,
	        PurchaseOrder.TaxType,
	        PurchaseOrder.Recipient.Initials,
	        Suffix);
	    }
	  }
	}
}