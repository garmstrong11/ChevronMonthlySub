namespace ChevronMonthlySub.UI.Infra
{
	using Domain;

	public class TotalsEvent
	{
		public TotalsEvent(IInvoiceService invoiceService)
		{
			InvoiceService = invoiceService;
		}

		public IInvoiceService InvoiceService { get; private set; }
	}
}