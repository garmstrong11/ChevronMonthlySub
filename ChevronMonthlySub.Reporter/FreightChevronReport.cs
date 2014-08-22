namespace ChevronMonthlySub.Reporter
{
	using Domain;

	public class FreightChevronReport : ChevronReport<FreightPurchaseOrder>
	{
		private readonly ITemplatePathService _templatePathService;
		private const string FileFormat = "{0} {1} {2} {3} {4}{5}";

		internal FreightChevronReport(ITemplatePathService templatePathService)
		{
			_templatePathService = templatePathService;
		}

		public override void BuildReport()
		{
			base.BuildReport();

			Report.AddTable("States", PurchaseOrder.States);
			Report.SetValue("SubTotal", PurchaseOrder.LineAmountSubtotal);
			Report.SetValue("TaxTotal", PurchaseOrder.TaxAmountSubtotal);
			var templateFilePath = _templatePathService.GetTemplatePath(IsSummary, GetType());

		}

		public override string OutputFilename
		{
			// Chevron FG InvoiceID PoNumber TaxType RecipInitials Suffix
			get { return string.Format(FileFormat, 
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