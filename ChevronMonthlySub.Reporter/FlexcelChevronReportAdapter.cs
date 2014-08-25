namespace ChevronMonthlySub.Reporter
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using Domain;
	using FlexCel.Core;
	using FlexCel.Report;

  public class FlexcelChevronReportAdapter : IChevronReportAdapter
  {
    private readonly FlexCelReport _report;
	  private readonly ITemplatePathService _templatePathService;

	  public FlexcelChevronReportAdapter(ITemplatePathService templatePathService)
    {
	    _templatePathService = templatePathService;
		  _report = new FlexCelReport(true);
    }
    
    public void AddTable<T>(string tableName, IEnumerable<T> value)
    {
      _report.AddTable(tableName, value);
    }

    public void SetValue(string name, object value)
    {
      _report.SetValue(name, value);
    }

    public void Run(bool isSummary)
    {
      _report.SetValue("LastDay", GetLastDayOfThisMonthAsDouble());

			var prefix = isSummary ? "Summary" : "Chevron FG";
      var outputFilename = string.Format("{0} {1}", prefix, OutputFileNameWithoutPrefix);

	    var outputPath = Path.Combine(_templatePathService.OutputDirectory, outputFilename);
	    var templatePath = _templatePathService.GetTemplatePath(isSummary, Type);

      _report.Run(templatePath, outputPath);
    }

	  public Type Type { get; set; }

	  public string OutputFileNameWithoutPrefix { get; set; }

		private static double GetLastDayOfThisMonthAsDouble()
		{
			var now = DateTime.Now;
			var lastDay = new DateTime(now.Year, now.Month, DateTime.DaysInMonth(now.Year, now.Month));
			return FlxDateTime.ToOADate(lastDay, false);
		}
  }
}