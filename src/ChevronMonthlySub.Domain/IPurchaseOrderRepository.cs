namespace ChevronMonthlySub.Domain
{
	using System.Collections.Generic;

	public interface IPurchaseOrderRepository
	{
		IEnumerable<FreightLine> FreightLines { get; }
		IEnumerable<ProductLine> ProductLines { get; }
		IEnumerable<PurchaseOrder<ProductLine>> GetProductPurchaseOrders(string invoiceId);
		IEnumerable<PurchaseOrder<FreightLine>> GetFreightPurchaseOrders(string invoiceId);
	}
}