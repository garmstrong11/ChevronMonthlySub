namespace ChevronMonthlySub.Domain
{
	using System.Collections.Generic;

	public interface IPurchaseOrderRepository
	{
		IEnumerable<FreightLine> FreightLines { get; }
		IEnumerable<ProductLine> ProductLines { get; }
		IEnumerable<ProductPurchaseOrder> GetProductPurchaseOrders(string invoiceId);
		IEnumerable<FreightPurchaseOrder> GetFreightPurchaseOrders(string invoiceId);


	}
}