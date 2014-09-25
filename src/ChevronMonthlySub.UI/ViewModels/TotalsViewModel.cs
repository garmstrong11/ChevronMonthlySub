namespace ChevronMonthlySub.UI.ViewModels
{
	using Caliburn.Micro;
	using Infra;

	public class TotalsViewModel : PropertyChangedBase, IHandle<TotalsEvent>
	{
		private decimal _freightFee;
		private int _pickPackCount;
		private decimal _pickPackCharges;
		private int _salesLineCount;
		private int _boxCount;
		private decimal _boxCharges;
		private decimal _totalInvoice;

		public TotalsViewModel(IEventAggregator eventAggregator)
		{
			eventAggregator.Subscribe(this);
		}

		public decimal FreightFee
		{
			get { return _freightFee; }
			set
			{
				if (value == _freightFee) return;
				_freightFee = value;
				NotifyOfPropertyChange(() => FreightFee);
				//NotifyOfPropertyChange(() => TotalInvoice);
			}
		}

		public int SalesLineCount
		{
			get { return _salesLineCount; }
			set
			{
				if (value == _salesLineCount) return;
				_salesLineCount = value;
				NotifyOfPropertyChange();
			}
		}

		public int PickPackCount
		{
			get { return _pickPackCount; }
			set
			{
				if (value == _pickPackCount) return;
				_pickPackCount = value;
				NotifyOfPropertyChange();
			}
		}

		public decimal PickPackCharges
		{
			get { return _pickPackCharges; }
			set
			{
				if (value == _pickPackCharges) return;
				_pickPackCharges = value;
				NotifyOfPropertyChange();
			}
		}

		public int BoxCount
		{
			get { return _boxCount; }
			set
			{
				if (value == _boxCount) return;
				_boxCount = value;
				NotifyOfPropertyChange();
			}
		}

		public decimal BoxCharges
		{
			get { return _boxCharges; }
			set
			{
				if (value == _boxCharges) return;
				_boxCharges = value;
				NotifyOfPropertyChange();
			}
		}

		public decimal TotalInvoice
		{
			get { return _totalInvoice; }
			set
			{
				if (value == _totalInvoice) return;
				_totalInvoice = value;
				NotifyOfPropertyChange();
			}
		}

		public void Handle(TotalsEvent message)
		{
			var invoice = message.InvoiceService;
			FreightFee = invoice.FreightFee;
			SalesLineCount = invoice.SalesLines;
			PickPackCount = invoice.PickPackCount;
			PickPackCharges = invoice.PickPackFee;
			BoxCount = invoice.BoxCount;
			BoxCharges = invoice.BoxFee;
			TotalInvoice = invoice.TotalInvoice;

			Refresh();
		}
	}
}