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
	//const string testFilePath = @"F:\Chevron\Monthly Subsequent Orders\April Subsequent Orders 454411.xlsx";
	var extractor = new OrderLineExtractor(testFilePath);
	var repo = new ReportDataRepository(extractor);
	var invoiceId = GetInvoiceFromFilename(testFilePath);
	
	//repo.FreightLines.Dump("Freight Lines:", 0);
	var freightTable = repo.GetFreightReports(invoiceId);
	//freightTable.Dump("Freight Reports:", 0);
	foreach (var item in freightTable) {
		using (var report = new FlexCelReport(true)) {
			BuildFreightReport(report, item);		
		}
	}
	
	var productTable = repo.GetProductReports(invoiceId);
	foreach (var item in productTable) {
		using (var report = new FlexCelReport(true)) {
			BuildProductReport(report, item);		
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
	
public static void BuildFreightReport(FlexCelReport report, ReportData<FreightLine> freightData) {
	const string reportTestDir = @"F:\Chevron\Monthly Subsequent Orders";
	var reportTemplatePath = Path.Combine(reportTestDir, "FreightTemplate.xlsx");
	var reportFileName = string.Format("{0} {1} {2} {3} FRT.xlsx", 
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

public static void BuildProductReport(FlexCelReport report, ReportData<ProductLine> productData) {
	const string reportTestDir = @"F:\Chevron\Monthly Subsequent Orders";
	var reportTemplatePath = Path.Combine(reportTestDir, "ProductTemplate.xlsx");
	var reportFileName = string.Format("{0} {1} {2} {3}.xlsx", 
		productData.InvoiceNumber, productData.PoNumber, productData.Recipient, productData.TaxType);
	var reportDestinationPath = Path.Combine(reportTestDir, reportFileName);
		
	report.SetValue("TaxType", productData.TaxType);
	report.SetValue("LastDay", GetLastDayOfThisMonthAsDouble());
	report.SetValue("PoNumber", productData.PoNumber);
	report.SetValue("InvoiceNumber", productData.InvoiceNumber);
	report.SetValue("Recipient", productData.Recipient);
	report.SetValue("PickPackTotal", productData.States.SelectMany(line => line.OrderLines).Sum(n => n.ShipQty) * 0.50m);
	report.SetValue("BoxTotal", productData.States.SelectMany(line => line.OrderLines).Sum(n => n.Boxes) * 2.50m);
	report.AddTable("States", productData.States.OrderBy(s => s.StateName));
	
	report.Run(reportTemplatePath, reportDestinationPath);
}

//public static void BuildReport<T>(FlexCelReport report, ReportData<T> data) where T : OrderLine {
//	const string reportTestDir = @"F:\Chevron\Monthly Subsequent Orders";
//	var templateFilename = string.Empty;
//	var suffix = string.Empty;
//	var baseFormatString = "{0} {1} {2} {3}";
//	
//	report.AddTable("States", data.States.OrderBy(s => s.StateName));
//	report.SetValue("TaxType", data.TaxType);
//	report.SetValue("LastDay", GetLastDayOfThisMonthAsDouble());
//	report.SetValue("PoNumber", data.PoNumber);
//	report.SetValue("InvoiceNumber", data.InvoiceNumber);
//	report.SetValue("Recipient", data.Recipient);
//	
//	if (T is FreightLine) {
//		templateFilename = "FreightTemplate.xlsx";
//		suffix = " FRT.xlsx";
//		report.SetValue("SubTotal", data.States.SelectMany(line => line.OrderLines).Sum(n => n.LineAmount));
//		report.SetValue("TaxTotal", data.States.SelectMany(line => line.OrderLines).Sum(n => n.TaxAmount));
//	} 
//	if (T is ProductLine) {
//		templateFilename = "ProductTemplate.xlsx";
//		suffix = ".xlsx";
//		report.SetValue("PickPackTotal", data.States.SelectMany(line => line.OrderLines).Sum(n => n.ShipQty) * 0.50m);
//		report.SetValue("BoxTotal", data.States.SelectMany(line => line.OrderLines).Sum(n => n.Boxes) * 2.50m);
//	}
//	
//	var reportTemplatePath = Path.Combine(reportTestDir, templateFilename);
//	var formatString = string.Format("{0} {1}", baseFormatString, suffix);
//	var reportFileName = string.Format(formatString, data.InvoiceNumber, data.PoNumber, data.Recipient, data.TaxType);
//	var reportDestinationPath = Path.Combine(reportTestDir, reportFileName);
//	
//	report.Run(reportTemplatePath, reportDestinationPath);
//}