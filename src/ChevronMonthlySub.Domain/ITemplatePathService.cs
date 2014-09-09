namespace ChevronMonthlySub.Domain
{
	using System;

	public interface ITemplatePathService
	{
		string GetTemplatePath(bool isSummary, Type purchaseOrderType);
		//string FreightTemplatePath { get; }
		//string SummaryFreightTemplatePath { get; }
		//string ProductTemplatePath { get; }
		//string SummaryProductTemplatePath { get; }
		string OrderKeyPath { get; }
    string OutputDirectory { get; set; }
	}
}