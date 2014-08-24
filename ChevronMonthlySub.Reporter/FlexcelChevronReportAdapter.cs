namespace ChevronMonthlySub.Reporter
{
  using System.Collections.Generic;
  using Domain;
  using FlexCel.Report;

  public class FlexcelChevronReportAdapter : IChevronReportAdapter
  {
    private readonly FlexCelReport _report;

    public FlexcelChevronReportAdapter()
    {
      _report = new FlexCelReport();
    }
    
    public void AddTable<T>(string tableName, IEnumerable<T> value)
    {
      _report.AddTable(tableName, value);
    }

    public void SetValue(string name, object value)
    {
      _report.SetValue(name, value);
    }

    public void Run(string templatePath, string outputPathWithoutPrefix)
    {
      var prefix = IsSummary ? "Summary" : "Chevron FG";
      var outputPath = string.Format("{0} {1}", prefix, outputPathWithoutPrefix);

      _report.Run(templatePath, outputPath);
    }

    public bool IsSummary { get; set; }
  }
}