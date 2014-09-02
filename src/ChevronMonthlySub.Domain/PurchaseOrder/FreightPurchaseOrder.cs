namespace ChevronMonthlySub.Domain
{
  using System.Collections.Generic;
  using System.Linq;

	public class FreightPurchaseOrder : PurchaseOrder
	{
		public FreightPurchaseOrder(
			IChevronReportAdapter chevronReportAdapter)
			: base(chevronReportAdapter)
		{
			chevronReportAdapter.Type = GetType();
		}

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
	    return string.Format("{0} {1}", base.ToString(), "FRT.xlsx");
	  }

	  public override void ConfigureReport()
	  {
	    base.ConfigureReport();
      ReportAdapter.AddTable("States", States.OrderBy(s => s.StateName));
      ReportAdapter.SetValue("SubTotal", LineAmountSubtotal);
      ReportAdapter.SetValue("TaxTotal", TaxAmountSubtotal);
	  }

	  public override void RunReports()
	  {
		  ReportAdapter.OutputFileNameWithoutPrefix = ToString();
			ReportAdapter.Run(false);
		  ReportAdapter.Run(true);
	  }
	}
}