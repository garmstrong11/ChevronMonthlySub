<Query Kind="Statements">
  <Reference>F:\CODE\ChevronMonthlySub\src\ChevronMonthlySub.Extractor\bin\Debug\ChevronMonthlySub.Domain.dll</Reference>
  <Reference>F:\CODE\ChevronMonthlySub\src\ChevronMonthlySub.Extractor\bin\Debug\ChevronMonthlySub.Extractor.dll</Reference>
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

var freightQuery =
	from fl in freightLines
	group fl by fl.PoNumber into orders
	select new
	{
		PurchaseOrder = orders.Key,
		TaxGroups = 
			from order in orders
			group order by order.TaxGroup into taxGroups
			select new
			{
				TaxGroup = taxGroups.Key,
				States = 
					from txGroup in taxGroups
					group txGroup by txGroup.State into states
					select new 
					{
						State = states.Key,
						FreightLines = states.ToList()
					}
			}
	};
	
var productQuery =
	from line in productLines
	group line by line.PoNumber into orders
	select new
	{
		PurchaseOrder = orders.Key,
		TaxGroups = 
			from order in orders
			group order by order.TaxGroup into taxGroups
			select new
			{
				TaxGroup = taxGroups.Key,
				States = 
					from txGroup in taxGroups
					group txGroup by txGroup.State into states
					select new 
					{
						State = states.Key,
						FreightLines = states.ToList()
					}
			}
	};
freightQuery.Dump("Freight Reports", 1);
//productQuery.Dump("Product Reports", 1);


var freightReports = 
	from po in freightQuery
	from grp in po.TaxGroups
	select new {
		PoNumber = po.PurchaseOrder,
		TaxGroup = grp.TaxGroup,
		States = from state in grp.States
			select new {
				State = state.State,
				Lines = state.FreightLines
			}
	};

freightReports.Dump();