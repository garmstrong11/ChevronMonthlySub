namespace ChevronMonthlySub.Domain
{
	using System;
	using System.Collections.Generic;
	using System.Text.RegularExpressions;

	public class OrderLine
	{
		private static readonly Regex StateRegex = new Regex(@"([A-Z]{2})\.?$", RegexOptions.Compiled);

	  public OrderLine(FlexCelOrderLineDto dto)
	  {
	    DateShipped = dto.DateShipped;
	    PoNumber = dto.PoNumber;
	    OrderNumber = dto.OrderNumber;
	    LineDesc = dto.LineDesc;

	    ExtractDestinationAndState(LineDesc);
	  }

	  public DateTime DateShipped { get; private set; }
	  public string PoNumber { get; private set; }
	  public int OrderNumber { get; private set; }
	  public string LineDesc { get; private set; }
		public string Destination { get; private set; }
		public string TaxGroup { get; private set; }
	  public string State { get; private set; }

	  private void ExtractDestinationAndState(string desc)
	  {
			var split = desc.Split(new [] {"Shipped to "}, StringSplitOptions.RemoveEmptyEntries);
		  Destination = split[1].Trim();

		  var match = StateRegex.Match(Destination);
			var state = match.Groups[1].Value;

			string taxGroup;
			if (!TaxDict.TryGetValue(state, out taxGroup))
			{
				throw new InvalidStateException(PoNumber, OrderNumber, LineDesc, state);
			}

		  State = state;
		  TaxGroup = taxGroup;
	  }

		private static readonly Dictionary<string, string> TaxDict = new Dictionary<string, string>
			{
			{"AL", "NET"},		{"AK", "NOMAD"},		{"AZ", "NET"},		{"AR", "NET"},		{"CA", "NET"},
			{"CO", "GROSS"},	{"CT", "NET"},			{"DE", "NOMAD"},	{"DC", "GROSS"},	{"FL", "GROSS"},
			{"GA", "NET"},		{"HI", "NET"},			{"ID", "GROSS"},	{"IL", "NET"},		{"IN", "NET"},
			{"IA", "NET"},		{"KS", "NET"},			{"KY", "NET"},		{"LA", "NET"},		{"ME", "NET"},
			{"MD", "GROSS"},	{"MA", "GROSS"},		{"MI", "NET"},		{"MN", "GROSS"},	{"MS", "NET"},
			{"MO", "NET"},		{"MT", "NOMAD"},		{"NE", "NET"},		{"NV", "NET"},		{"NH", "NOMAD"},
			{"NJ", "GROSS"},	{"NM", "NET"},			{"NY", "GROSS"},	{"NC", "GROSS"},	{"ND", "NET"},
			{"OH", "GROSS"},	{"OK", "GROSS"},		{"OR", "NOMAD"},	{"PA", "GROSS"},	{"RI", "NET"},
			{"SC", "NET"},		{"SD", "NET"},			{"TN", "NET"},		{"TX", "TX"},			{"UT", "NET"},
			{"VT", "GROSS"},	{"VA", "NET"},			{"WA", "NET"},		{"WV", "GROSS"},	{"WI", "NET"},
			{"WY", "GROSS"}
			};
	}
}