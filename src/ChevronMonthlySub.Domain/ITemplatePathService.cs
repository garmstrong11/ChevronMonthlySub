namespace ChevronMonthlySub.Domain
{
	using System;

	public interface ITemplatePathService
	{
		string GetTemplatePath(bool isSummary, Type reportType);
	}
}