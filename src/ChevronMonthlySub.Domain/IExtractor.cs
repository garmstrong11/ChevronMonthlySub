namespace ChevronMonthlySub.Domain
{
	using System.Collections.Generic;
	
	public interface IExtractor<T>
	{
		IList<T> Extract();
		string SourcePath { get; set; }
	}
}