<Query Kind="Program">
  <Reference Relative="..\..\ChevronMonthlySub.Extractor\bin\Debug\ChevronMonthlySub.Domain.dll">F:\CODE\ChevronMonthlySub\src\ChevronMonthlySub.Extractor\bin\Debug\ChevronMonthlySub.Domain.dll</Reference>
  <Reference Relative="..\..\ChevronMonthlySub.Extractor\bin\Debug\ChevronMonthlySub.Extractor.dll">F:\CODE\ChevronMonthlySub\src\ChevronMonthlySub.Extractor\bin\Debug\ChevronMonthlySub.Extractor.dll</Reference>
  <GACReference>FlexCel, Version=6.3.0.0, Culture=neutral, PublicKeyToken=cb8f6080e6d5a4d6</GACReference>
  <Namespace>ChevronMonthlySub.Domain</Namespace>
  <Namespace>ChevronMonthlySub.Extractor</Namespace>
  <Namespace>FlexCel.Core</Namespace>
  <Namespace>FlexCel.Report</Namespace>
  <Namespace>FlexCel.XlsAdapter</Namespace>
</Query>

void Main()
{
	const string testFilePath = @"F:\code\ChevronMonthlySub\src\ChevronMonthlySub.Tests\DataFiles\Chevron June FG 462988.xlsx";
	var extractor = new OrderLineExtractor(testFilePath);
	var repo = new ReportDataRepository(extractor);
	
	//repo.FreightLines.Dump("Freight Lines:", 0);
	var freightTable = repo.GetFreightReports("462988");
	//freightTable.Dump("Freight Reports:", 0);
	foreach (var item in freightTable) {
		using (var report = new FlexCelReport(true)) {
			BuildFreightReport(report, item);		
		}
	}
}

// Define other methods and classes here
public static double GetLastDayOfThisMonthAsDouble()
    {
        var now = DateTime.Now;
		var lastDay = new DateTime(now.Year, now.Month, DateTime.DaysInMonth(now.Year, now.Month));
		return FlxDateTime.ToOADate(lastDay, false);
    }
	
public static void BuildFreightReport(FlexCelReport report, ReportData<FreightLine> freightData) {
	const string reportTestDir = @"F:\Chevron\Monthly Subsequent Orders";
	var reportTemplatePath = Path.Combine(reportTestDir, "FreightTemplate.xlsx");
	var reportFileName = string.Format("{0} {1} {2} {3}.xlsx", 
		freightData.InvoiceNumber, freightData.PoNumber, freightData.Recipient, freightData.TaxType);
	var reportDestinationPath = Path.Combine(reportTestDir, reportFileName);
		
	report.SetValue("TaxType", freightData.TaxType);
	report.SetValue("LastDay", GetLastDayOfThisMonthAsDouble());
	report.SetValue("PoNumber", freightData.PoNumber);
	report.SetValue("InvoiceNumber", freightData.InvoiceNumber);
	report.SetValue("Recipient", freightData.Recipient);
	report.SetValue("SubTotal", freightData.States.SelectMany(line => line.OrderLines).Sum(n => n.LineAmount));
	report.SetValue("TaxTotal", freightData.States.SelectMany(line => line.OrderLines).Sum(n => n.TaxAmount));
	report.AddTable("States", freightData.States.OrderBy(s => s.StateName));
	
	report.Run(reportTemplatePath, reportDestinationPath);
}