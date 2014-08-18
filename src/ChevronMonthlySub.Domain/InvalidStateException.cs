namespace ChevronMonthlySub.Domain
{
	using System;

	public class InvalidStateException : Exception
	{
		private readonly string _poNumber;
		private readonly int _orderNumber;
		private readonly string _lineDesc ;
		private readonly string _badState;

		public InvalidStateException(string poNumber, int orderNumber, string lineDesc, string badState)
		{
			_poNumber = poNumber;
			_orderNumber = orderNumber;
			_lineDesc = lineDesc;
			_badState = badState;
		}

		public override string Message
		{
			get { return FormExceptionMessage(); }
		}

		private string FormExceptionMessage()
		{
			return string.Format("The input \"{0}\" from PO Number {1}, Order Number {2}, LineDesc {3} is not a valid state.",
				_badState, _poNumber, _orderNumber, _lineDesc);
		}
	}
}