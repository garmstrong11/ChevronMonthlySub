namespace ChevronMonthlySub.Domain
{
	using System;
	using System.Collections.Generic;

  public interface IChevronReportAdapter
  {
    void AddTable<T>(string tableName, IEnumerable<T> value);
    void SetValue(string name, object value);
    void Run(bool isSummary);

		Type Type { get; set; }
		string OutputFileNameWithoutPrefix { get; set; }
  }
}