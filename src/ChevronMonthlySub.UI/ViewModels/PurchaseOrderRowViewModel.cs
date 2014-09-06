namespace ChevronMonthlySub.UI.ViewModels
{
	using System.Collections.Generic;
	using Caliburn.Micro;
	using Domain;

	public class PurchaseOrderRowViewModel : PropertyChangedBase
	{
		private readonly PurchaseOrder _purchaseOrder;
		private TaxType _taxType;
		private string _poNumber;
		private string _invoiceNumber;
		private Requestor _selectedRequestor;
		private string _description;
		private List<Requestor> _requestors;

		public PurchaseOrderRowViewModel(PurchaseOrder purchaseOrder, IRequestorService requestorService)
		{
			_purchaseOrder = purchaseOrder;
			_requestors = new List<Requestor>(requestorService.GetAll());
			Map();
		}

		private void Map()
		{
			// Set the underlying fields to avoid setter logic during mapping.
			_taxType = _purchaseOrder.TaxType;
			_poNumber = _purchaseOrder.PoNumber;
			_invoiceNumber = _purchaseOrder.InvoiceNumber;
			_selectedRequestor = _purchaseOrder.Requestor;
			_description = _purchaseOrder.Description;
		}

		public TaxType TaxType
		{
			get { return _taxType; }
			set
			{
				if (value == _taxType) return;
				_taxType = value;
				NotifyOfPropertyChange(() => TaxType);
			}
		}

		public string PoNumber
		{
			get { return _poNumber; }
			set
			{
				if (value == _poNumber) return;
				_poNumber = value;
				_purchaseOrder.PoNumber = value;
				NotifyOfPropertyChange(() => PoNumber);
			}
		}

		public string InvoiceNumber
		{
			get { return _invoiceNumber; }
			set
			{
				if (value == _invoiceNumber) return;
				_invoiceNumber = value;
				NotifyOfPropertyChange(() => InvoiceNumber);
			}
		}

		public List<Requestor> Requestors
		{
			get { return _requestors; }
			set
			{
				if (Equals(value, _requestors)) return;
				_requestors = value;
				NotifyOfPropertyChange(() => Requestors);
			}
		}

		public Requestor SelectedRequestor
		{
			get { return _selectedRequestor; }
			set
			{
				if (Equals(value, _selectedRequestor)) return;
				_selectedRequestor = value;
				_purchaseOrder.Requestor = value;
				NotifyOfPropertyChange(() => SelectedRequestor);
			}
		}

		public string Description
		{
			get { return _description; }
			set
			{
				if (value == _description) return;
				_description = value;
				_purchaseOrder.Description = value;
				NotifyOfPropertyChange(() => Description);
			}
		}

		public string PoType
		{
			get
			{
				if (_purchaseOrder is FreightPurchaseOrder) return "Freight";
				if (_purchaseOrder is ProductPurchaseOrder) return "Product";
				return "Unknown";
			}
		}
	}
}