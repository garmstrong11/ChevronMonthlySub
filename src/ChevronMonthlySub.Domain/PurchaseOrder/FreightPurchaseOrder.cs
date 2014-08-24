namespace ChevronMonthlySub.Domain
{
  using System.Collections.Generic;
  using System.Linq;

	public class FreightPurchaseOrder : PurchaseOrder
	{
	  public FreightPurchaseOrder(
      ITemplatePathService templatePathService, 
      IChevronReportAdapter chevronReportAdapter) 
      : base(templatePathService, chevronReportAdapter)
	  { }

	  public IEnumerable<FreightStateGroup> States { get; set; } 

		public decimal LineAmountSubtotal
		{
			get { return States.Sum(t => t.LineAmountSubtotal); }
		}

		public decimal TaxAmountSubtotal
		{
			get { return States.Sum(t => t.TaxAmountSubtotal); }
		}

		public decimal Total
		{
			get { return LineAmountSubtotal + TaxAmountSubtotal; }
		}

	  public override string ToString()
	  {
	    return string.Format("{0}{1}", base.ToString(), " FRT.xlsx");
	  }

	  public override void ConfigureReport()
	  {
	    base.ConfigureReport();
      ReportAdapter.AddTable("States", States);
      ReportAdapter.SetValue("SubTotal", LineAmountSubtotal);
      ReportAdapter.SetValue("TaxTotal", TaxAmountSubtotal);
	  }

	  public void RunReports()
	  {
	    var templatePath = TemplatePathService.
      ReportAdapter.IsSummary = false;
      ReportAdapter.Run();
	  }
	}
}