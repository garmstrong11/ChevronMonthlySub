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
	var recipientRepo = new HardCodedRecipientRepository();
	var repo = new ReportDataRepository(extractor, recipientRepo);
	var invoiceId = GetInvoiceFromFilename(testFilePath);
	
	var fr = repo.GetFreightReports(invoiceId);
	var pr = repo.GetProductReports(invoiceId);
	
	pr.Dump("Product Lines: ", 0);
	fr.Dump("Freight Lines: ", 0);
		
	foreach (var item in fr) {
		using (var report = new FlexCelReport(true)) {
			BuildReport(report, item);		
		}
	}
	
	foreach (var item in pr) {
		using (var report = new FlexCelReport(true)) {
			BuildReport(report, item);		
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
	
public static string GetInvoiceFromFilename(string path) {
	var fileName = Path.GetFileNameWithoutExtension(path);
	var invoiceRegex = new Regex(@"^.*?(\d+)$", RegexOptions.Compiled);
	return invoiceRegex.Matches(fileName)[0].Groups[1].Value;
}

public static void BuildReport<T>(FlexCelReport report, ReportData<T> data) where T : OrderLine {
	const string reportTestDir = @"F:\Chevron\Monthly Subsequent Orders";
	var templateFilename = string.Empty;
	var suffix = string.Empty;
	var formatString = "{0} {1} {2} {3}{4}";
	T firstOrderLine = data.States.First().OrderLines.First();
	
	report.AddTable("States", data.States.OrderBy(s => s.StateName));
	report.SetValue("TaxType", data.TaxType);
	report.SetValue("LastDay", GetLastDayOfThisMonthAsDouble());
	report.SetValue("PoNumber", data.PoNumber);
	report.SetValue("InvoiceNumber", data.InvoiceNumber);
	report.SetValue("Recipient", data.Recipient.Initials);
	
	if (firstOrderLine is FreightLine) {
		templateFilename = "FreightTemplate.xlsx";
		suffix = " FRT.xlsx";
		report.SetValue("SubTotal", data.States.SelectMany(line => line.OrderLines.Cast<FreightLine>()).Sum(n => n.LineAmount));
		report.SetValue("TaxTotal", data.States.SelectMany(line => line.OrderLines.Cast<FreightLine>()).Sum(n => n.TaxAmount));
	} 
	if (firstOrderLine is ProductLine) {
		templateFilename = "ProductTemplate.xlsx";
		suffix = ".xlsx";
		report.SetValue("PickPackTotal", data.States.SelectMany(line => line.OrderLines.Cast<ProductLine>()).Sum(n => n.ShipQty) * 0.50m);
		report.SetValue("BoxTotal", data.States.SelectMany(line => line.OrderLines.Cast<ProductLine>()).Sum(n => n.Boxes) * 2.50m);
	}
	
	var reportTemplatePath = Path.Combine(reportTestDir, templateFilename);
	var reportFileName = string.Format(formatString, 
		data.InvoiceNumber, data.PoNumber, data.Recipient.Initials, data.TaxType, suffix);
	var reportDestinationPath = Path.Combine(reportTestDir, reportFileName);
	
	report.Run(reportTemplatePath, reportDestinationPath);
}