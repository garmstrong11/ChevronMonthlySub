namespace ChevronMonthlySub.Domain
{
	using System;
	using System.Collections.Generic;
	using System.Text.RegularExpressions;

	public class OrderLine
	{
		private string _state;
		private string _taxGroup = "NET";

		private static readonly Regex _regex = 
			new Regex(@"(^.*)\n?\r?Shipped to ([^,]+), ?([^,]+), ?([A-Z]{2})", 
				RegexOptions.Singleline | RegexOptions.Compiled);

		public DateTime DateShipped { get; private set; }
		public string PoNumber { get; private set; }
		public int OrderNumber { get; private set; }
		public string LineDesc { get; private set; }

		public string TaxGroup
		{
			get { return _taxGroup; }
		}

		public string Site { get; private set; }
		public string City { get; private set; }

		public string State
		{
			get { return _state; }
			private set
			{
				_state = value;
				string taxGroup;

				if (_taxDict.TryGetValue(value, out taxGroup)) {
					_taxGroup = taxGroup;
				}
			}
		}

		public string Product { get; private set; }

		public OrderLine(FlexCelOrderLineDto dto)
		{
			DateShipped = dto.DateShipped;
			PoNumber = dto.PoNumber;
			OrderNumber = dto.OrderNumber;
			LineDesc = dto.LineDesc;

			ExtractAddress(LineDesc);
		}

		private void ExtractAddress(string desc)
		{
			var match = _regex.Match(desc);

			City = match.Groups[3].Value.Trim();
			State = match.Groups[4].Value.Trim();
			Site = match.Groups[2].Value.Trim();
			Product = match.Groups[1].Value.Trim();
		}

		private static Dictionary<string, string> _taxDict = new Dictionary<string, string>
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