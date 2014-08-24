namespace ChevronMonthlySub.Domain
{
  using System.Collections.Generic;

  public interface IChevronReportAdapter
  {
    void AddTable<T>(string tableName, IEnumerable<T> value);
    void SetValue(string name, object value);
    void Run(string templatePath, string outputPath);

    bool IsSummary { get; set; }
  }
}