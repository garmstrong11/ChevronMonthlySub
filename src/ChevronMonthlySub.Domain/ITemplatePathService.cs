namespace ChevronMonthlySub.Domain
{
	public interface ITemplatePathService
	{
		//string GetTemplatePath(bool isSummary, PurchaseOrder purchaseOrder);
    string FreightTemplatePath { get; }
    string SummaryFreightTemplatePath { get; }
    string ProductTemplatePath { get; }
    string SummaryProductTemplatePath { get; }

    string OutputDirectory { get; set; }
	}
}