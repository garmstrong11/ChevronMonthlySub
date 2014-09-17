namespace ChevronMonthlySub.Domain
{
	using System.Collections.Generic;

	public interface IInvoiceService
	{
		string SourcePath { get; set; }
		IEnumerable<FreightLine> FreightLines { get; }
		IEnumerable<ProductLine> ProductLines { get; }
		IEnumerable<ProductPurchaseOrder> GetProductPurchaseOrders(string invoiceId);
		IEnumerable<FreightPurchaseOrder> GetFreightPurchaseOrders(string invoiceId);

		int SalesLines { get; }
		decimal FreightFee { get; }

		int PickPackCount { get; }
		decimal PickPackFee { get; }

		int BoxCount { get; }
		decimal BoxFee { get; }

		decimal TotalInvoice { get; }
	}
}