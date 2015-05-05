namespace ChevronMonthlySub.UI.ViewModels
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Text.RegularExpressions;
	using System.Threading.Tasks;
	using System.Windows;
	using Caliburn.Micro;
	using Domain;
	using Extractor;
	using FluentValidation;
	using Infra;

	public class ShellViewModel : Screen, IShell
	{
		private readonly IInvoiceService _invoiceService;
		private readonly ISourcePathFactory<FlexCelOrderLineDto> _sourcePathFactory;
		private readonly IValidator<SourcePath<FlexCelOrderLineDto>> _sourcePathValidator;
		private readonly IWindowManager _windowManager;
		private readonly IRequestorService _requestorService;
		private readonly IEventAggregator _eventAggregator;
		private readonly Dictionary<string, OrderKey> _orderKeys; 
		private string _invoiceId;
		private readonly List<PurchaseOrder> _poList;
		private TotalsViewModel _totals;

		public ShellViewModel(
			IInvoiceService invoiceService, 
			IRequestorService requestorService,
			IOrderKeyService orderKeyService,
			ISourcePathFactory<FlexCelOrderLineDto> sourcePathFactory,
			IValidator<SourcePath<FlexCelOrderLineDto>> sourcePathValidator,
			IWindowManager windowManager,
			IEventAggregator eventAggregator)
		{
			_invoiceService = invoiceService;
			_requestorService = requestorService;
			_orderKeys = orderKeyService.AcquireOrderKeys();
			_sourcePathFactory = sourcePathFactory;
			_sourcePathValidator = sourcePathValidator;
			_windowManager = windowManager;
			_eventAggregator = eventAggregator;

			PurchaseOrders = new PurchaseOrdersViewModel();
			_poList = new List<PurchaseOrder>();

			Totals = new TotalsViewModel(_eventAggregator);
		}

		protected override void OnActivate()
		{
			DisplayName = "Drag your Excel file into this window";
		}

		private void Reset()
		{
			PurchaseOrders.Items.Clear();
			_poList.Clear();
			InvoiceId = string.Empty;
			DisplayName = "Drag your Excel file into this window";
			Totals = new TotalsViewModel(_eventAggregator);
		}

		public PurchaseOrdersViewModel PurchaseOrders { get; set; }

		public TotalsViewModel Totals
		{
			get { return _totals; }
			set
			{
				if (Equals(value, _totals)) return;
				_totals = value;
				NotifyOfPropertyChange();
			}
		}

		public string InvoiceId
		{
			get { return _invoiceId; }
			set
			{
				if (value == _invoiceId) return;
				_invoiceId = value;
				NotifyOfPropertyChange(() => InvoiceId);
			}
		}

		public void HandleFileDrag(object evtArgs)
		{
			var args = (DragEventArgs)evtArgs;

			var filePaths = (string[])args.Data.GetData(DataFormats.FileDrop);
			var info = new FileInfo(filePaths[0]);

			if (info.IsDirectory()) return;
			if (info.Extension != ".xlsx") return;

			args.Effects = DragDropEffects.Link;
			args.Handled = true;
		}

		public void HandleFileDrop(ActionExecutionContext ctx)
		{
			var args = (DragEventArgs)ctx.EventArgs;
			var folderPaths = (string[])args.Data.GetData(DataFormats.FileDrop);
			var info = new FileInfo(folderPaths[0]);

			Reset();

			var sourcePath = _sourcePathFactory.Create(info.FullName);
			var result = _sourcePathValidator.Validate(sourcePath, ruleSet: "Name,Structure");

			if (!result.IsValid) {
				var errMessage = string.Join("\n", result.Errors.Select(e => e.ErrorMessage));
				var errDialog = new ErrorWindowViewModel {Errors = errMessage};
				_windowManager.ShowDialog(errDialog);

				return;
			}

			string id;
			if (!TryExtractInvoiceIdFromFilePath(info.FullName, out id)) return;
			DisplayName = string.Format("Invoice {0}", id);

			args.Effects = DragDropEffects.Link;

			args.Handled = true;

			try {
				// Setting the SourcePath initiates extraction, extraction errors will propagate from there.
				_invoiceService.SourcePath = info.FullName;
				_poList.AddRange(_invoiceService.GetFreightPurchaseOrders(id));
				_poList.AddRange(_invoiceService.GetProductPurchaseOrders(id));
				_eventAggregator.Publish(new TotalsEvent(_invoiceService), a => Task.Factory.StartNew(a));

				foreach (var purchaseOrder in _poList)
				{
					purchaseOrder.UpdateWithOrderKey(_orderKeys);
				}

				PurchaseOrders.Items.AddRange(_poList.Select(p => new PurchaseOrderRowViewModel(p, _requestorService)));
			}
			catch (Exception exc) {
				var errDialog = new ErrorWindowViewModel {Errors = exc.Message};
				_windowManager.ShowDialog(errDialog);
			}
		}

		public void RunReports()
		{
			foreach (var purchaseOrder in _poList) {
				purchaseOrder.ConfigureReport();
				purchaseOrder.RunReports();
			}
		}

		private static bool TryExtractInvoiceIdFromFilePath(string filePath, out string invoiceId)
		{
			invoiceId = String.Empty;

			if (!File.Exists(filePath)) return false;

			var fileName = Path.GetFileNameWithoutExtension(filePath);
			if (fileName == null) return false;

			var match = Regex.Match(fileName, @"\d{6,}");
			if (!match.Success) return false;

			invoiceId = match.Value;
			return true;
		}
	}
}