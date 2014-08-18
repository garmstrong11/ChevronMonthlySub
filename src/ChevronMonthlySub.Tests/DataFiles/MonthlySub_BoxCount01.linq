<Query Kind="Statements">
  <Reference Relative="..\..\ChevronMonthlySub.Extractor\bin\Debug\ChevronMonthlySub.Domain.dll">F:\CODE\ChevronMonthlySub\src\ChevronMonthlySub.Extractor\bin\Debug\ChevronMonthlySub.Domain.dll</Reference>
  <Reference Relative="..\..\ChevronMonthlySub.Extractor\bin\Debug\ChevronMonthlySub.Extractor.dll">F:\CODE\ChevronMonthlySub\src\ChevronMonthlySub.Extractor\bin\Debug\ChevronMonthlySub.Extractor.dll</Reference>
  <GACReference>FlexCel, Version=6.3.0.0, Culture=neutral, PublicKeyToken=cb8f6080e6d5a4d6</GACReference>
  <Namespace>ChevronMonthlySub.Domain</Namespace>
  <Namespace>ChevronMonthlySub.Extractor</Namespace>
  <Namespace>FlexCel.Core</Namespace>
  <Namespace>FlexCel.XlsAdapter</Namespace>
</Query>

const string testFilePath = @"F:\CODE\ChevronMonthlySub\src\ChevronMonthlySub.Tests\DataFiles\Chevron June FG 462988.xlsx";
var xls = new XlsFile(testFilePath, false);
var extractor = new OrderLineExtractor(xls);
var dtos = extractor.Extract();

var factory = new LineFactory();
var lines = dtos.Select(factory.Create).ToList();

var freightLines = lines.OfType<FreightLine>();
var productLines = lines.OfType<ProductLine>();

var siteDict = (from line in freightLines
			group line by line.Destination into sites
			select new { 
				Key = sites.Key,
				BoxCount = sites.Count()
			}).ToDictionary(k => k.Key, v => v.BoxCount);
			
//siteDict.Dump();

// A list of first product in shipments
var firstProducts = 
	from line in productLines
	group line by line.Destination into shipment
	select shipment.First();
	
firstProducts.Dump("First Products:", 0);

// Set the boxes quantity for all firstProducts:
foreach (var product in firstProducts) {
	var count = 0;
	
	// Make sure at least one box is sent to each site:
	if (!siteDict.TryGetValue(product.Destination, out count)) {
		count = 1;
	}
	
	product.Boxes = count;
}

productLines.OrderBy(k => k.PoNumber).ThenBy(k => k.Destination).Dump();