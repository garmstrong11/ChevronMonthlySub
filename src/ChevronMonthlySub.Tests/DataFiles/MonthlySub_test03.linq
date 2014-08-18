<Query Kind="Statements">
  <Reference Relative="..\..\ChevronMonthlySub.Extractor\bin\Debug\ChevronMonthlySub.Domain.dll">F:\CODE\ChevronMonthlySub\src\ChevronMonthlySub.Extractor\bin\Debug\ChevronMonthlySub.Domain.dll</Reference>
  <Reference Relative="..\..\ChevronMonthlySub.Extractor\bin\Debug\ChevronMonthlySub.Extractor.dll">F:\CODE\ChevronMonthlySub\src\ChevronMonthlySub.Extractor\bin\Debug\ChevronMonthlySub.Extractor.dll</Reference>
  <GACReference>FlexCel, Version=6.3.0.0, Culture=neutral, PublicKeyToken=cb8f6080e6d5a4d6</GACReference>
  <Namespace>ChevronMonthlySub.Domain</Namespace>
  <Namespace>ChevronMonthlySub.Extractor</Namespace>
  <Namespace>FlexCel.Core</Namespace>
  <Namespace>FlexCel.XlsAdapter</Namespace>
</Query>

const string testFilePath = @"F:\code\ChevronMonthlySub\src\ChevronMonthlySub.Tests\DataFiles\Chevron June FG 462988.xlsx";
var xls = new XlsFile(testFilePath, false);
var extractor = new OrderLineExtractor(xls);
var dtos = extractor.Extract();

var factory = new LineFactory();
var lines = dtos.Select(factory.Create).ToList();

var freightLines = lines.OfType<FreightLine>();
var productLines = lines.OfType<ProductLine>();


var freightQuery = 
	from fl in freightLines
	group fl by new {fl.PoNumber, fl.TaxGroup} into orders
	select new TaxGroup<FreightLine> {
		PoNumber = orders.Key.PoNumber,
		GroupName = orders.Key.TaxGroup,
		States = 
			from order in orders
			group order by order.State into states
			select new StateOrderGroup<FreightLine> {
				StateName = states.Key,
				OrderLines = states.ToList()
			}
	};
	
freightQuery.First().Dump();

var productQuery = 
	from fl in productLines
	group fl by new {fl.PoNumber, fl.TaxGroup} into orders
	select new TaxGroup<ProductLine> {
		PoNumber = orders.Key.PoNumber,
		GroupName = orders.Key.TaxGroup,
		States = 
			from order in orders
			group order by order.State into states
			select new StateOrderGroup<ProductLine> {
				StateName = states.Key,
				OrderLines = states.ToList()
			}
	};
	
productQuery.First().Dump();