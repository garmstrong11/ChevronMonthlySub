namespace ChevronMonthlySub.Domain
{
	using System.Collections.Generic;

	public interface IPurchaseOrderRepository
	{
		IEnumerable<FreightLine> FreightLines { get; }
		IEnumerable<ProductLine> ProductLines { get; }
		IEnumerable<PurchaseOrder<ProductLine>> GetProductReports(string InvoiceId);
		IEnumerable<PurchaseOrder<FreightLine>> GetFreightReports(string InvoiceId);
	}
}