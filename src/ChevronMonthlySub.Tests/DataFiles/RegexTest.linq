<Query Kind="Statements" />

const string testFilePath = @"F:\code\ChevronMonthlySub\src\ChevronMonthlySub.Tests\DataFiles\April Subsequent Orders 454411.xlsx";

var fileName = Path.GetFileNameWithoutExtension(testFilePath);
var invoiceRegex = new Regex(@"^.*?(\d+)$", RegexOptions.Compiled);
var invoiceNumber = invoiceRegex.Matches(fileName)[0].Groups[1].Value;

invoiceNumber.Dump();