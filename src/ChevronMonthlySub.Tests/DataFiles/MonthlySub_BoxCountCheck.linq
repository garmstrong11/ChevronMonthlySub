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
	const string testFileName = @"F:\Chevron\Monthly Subsequent Orders\Chevron June FG 462988.xlsx";

	var _extractor = new OrderLineExtractor(testFileName);
	var _recipRepo = new HardCodedRecipientRepository();
	var _shipService = new HardCodedShippingCostService();
	var _templatePathService = new HardCodedTemplatePathService();

	var repo = new PurchaseOrderRepo(_extractor, _recipRepo, _shipService, _templatePathService);
	var productLines = repo.ProductLines;
	
	//productLines.Dump();
	var siteList1 = repo.FreightLines.GroupBy(s => new {s.Destination, s.PoNumber});
	var siteList = (from line in repo.FreightLines
					group line by new { line.Destination, line.PoNumber } into sites
					select new
					{
						sites.Key.Destination,
						sites.Key.PoNumber,
						BoxCount = sites.Count()
					}).ToList();
					
	//siteList1.Dump();
	siteList.Where(p => p.PoNumber == "15146759" && p.Destination.EndsWith("AL.")).Dump();
	
	var firstProducts = from line in repo.ProductLines
						group line by new { line.Destination, line.PoNumber } into shipment
						select shipment;
						
	firstProducts.Where(p => p.Key.Destination.EndsWith("AL.")).Dump();
						
	//firstProducts.Where(p => p.PoNumber == "15146759" && p.Destination.EndsWith("AL.")).Dump();
}

// Define other methods and classes here