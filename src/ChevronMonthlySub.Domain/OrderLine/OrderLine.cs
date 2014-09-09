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
	  public string PoNumber { get; set; }
	  public int OrderNumber { get; private set; }
	  public string LineDesc { get; private set; }
		public string Destination { get; private set; }
		public TaxType TaxType { get; private set; }
	  public string State { get; private set; }

	  private void ExtractDestinationAndState(string desc)
	  {
			var split = desc.Split(new [] {"Shipped to "}, StringSplitOptions.RemoveEmptyEntries);
		  Destination = split[1].Trim();

		  var match = StateRegex.Match(Destination);
			var state = match.Groups[1].Value;

			TaxType taxType;
			if (!TaxDict.TryGetValue(state, out taxType))
			{
				throw new InvalidStateException(PoNumber, OrderNumber, LineDesc, state);
			}

		  State = state;
		  TaxType = taxType;
	  }

		private static readonly Dictionary<string, TaxType> TaxDict = new Dictionary<string, TaxType>
			{

			{"TX", TaxType.TX},

			{"AK", TaxType.NOMAD},
			{"DE", TaxType.NOMAD},
			{"MT", TaxType.NOMAD},
			{"NH", TaxType.NOMAD},
			{"OR", TaxType.NOMAD},

			{"CO", TaxType.GROSS},
			{"DC", TaxType.GROSS},
			{"FL", TaxType.GROSS},
			{"ID", TaxType.GROSS},
			{"MD", TaxType.GROSS},
			{"MA", TaxType.GROSS},
			{"MN", TaxType.GROSS},
			{"NJ", TaxType.GROSS},
			{"NY", TaxType.GROSS},
			{"NC", TaxType.GROSS},
			{"OH", TaxType.GROSS},
			{"OK", TaxType.GROSS},
			{"PA", TaxType.GROSS},
			{"VT", TaxType.GROSS},
			{"WV", TaxType.GROSS},
			{"WY", TaxType.GROSS},

			{"AL", TaxType.NET},
			{"AZ", TaxType.NET},
			{"AR", TaxType.NET},
			{"CA", TaxType.NET},
			{"CT", TaxType.NET},
			{"GA", TaxType.NET},
			{"HI", TaxType.NET},
			{"IL", TaxType.NET},
			{"IN", TaxType.NET},
			{"IA", TaxType.NET},
			{"KS", TaxType.NET},
			{"KY", TaxType.NET},
			{"LA", TaxType.NET},
			{"ME", TaxType.NET},
			{"MI", TaxType.NET},
			{"MS", TaxType.NET},
			{"MO", TaxType.NET},
			{"NE", TaxType.NET},
			{"NV", TaxType.NET},
			{"NM", TaxType.NET},
			{"ND", TaxType.NET},
			{"RI", TaxType.NET},
			{"SC", TaxType.NET},
			{"SD", TaxType.NET},
			{"TN", TaxType.NET},
			{"UT", TaxType.NET},
			{"VA", TaxType.NET},
			{"WA", TaxType.NET},
			{"WI", TaxType.NET},
			
			};
	}
}