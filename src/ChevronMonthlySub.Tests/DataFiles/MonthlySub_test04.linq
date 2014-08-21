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
var extractor = new OrderLineExtractor(testFilePath);
var repo = new ReportDataRepository(extractor);

repo.FreightLines.Dump("Freight Lines:", 0);
repo.ProductLines.Dump("Product Lines:", 0);

repo.GetFreightReports("696969").Dump("Freight Reports:", 0);
repo.GetProductReports("707070").Dump("Product Reports:", 0);