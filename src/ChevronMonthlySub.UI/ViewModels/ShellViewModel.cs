namespace ChevronMonthlySub.UI.ViewModels
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Text.RegularExpressions;
	using System.Windows;
	using Caliburn.Micro;
	using Domain;
	using Infra;

	public class ShellViewModel : Screen, IShell
	{
		private string _rowSourcePath;
		private readonly IPurchaseOrderService _purchaseOrderService;
		private readonly IRequestorService _requestorService;
		private readonly Dictionary<string, OrderKey> _orderKeys; 
		private string _invoiceId;
		private readonly List<PurchaseOrder> _poList;

		public ShellViewModel(
			IPurchaseOrderService purchaseOrderService, 
			IRequestorService requestorService,
			IOrderKeyService orderKeyService)
		{
			_purchaseOrderService = purchaseOrderService;
			_requestorService = requestorService;
			_orderKeys = orderKeyService.AcquireOrderKeys();

			PurchaseOrders = new PurchaseOrdersViewModel();
			_poList = new List<PurchaseOrder>(); 
		}

		private void Reset()
		{
			PurchaseOrders.Items.Clear();
			_poList.Clear();
			InvoiceId = string.Empty;
			RowSourcePath = string.Empty;
		}

		public string RowSourcePath
		{
			get { return _rowSourcePath; }
			set
			{	
				if (value == _rowSourcePath) return;
				_rowSourcePath = value;
				NotifyOfPropertyChange();
			}
		}

		public PurchaseOrdersViewModel PurchaseOrders { get; set; }

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

			string id;
			if (!TryExtractInvoiceIdFromFilePath(info.FullName, out id)) return;

			args.Effects = DragDropEffects.Link;

			RowSourcePath = info.FullName;
			args.Handled = true;

			_purchaseOrderService.SourcePath = RowSourcePath;
			_poList.AddRange(_purchaseOrderService.GetFreightPurchaseOrders(id));
			_poList.AddRange(_purchaseOrderService.GetProductPurchaseOrders(id));

			foreach (var purchaseOrder in _poList)
			{
				purchaseOrder.UpdateWithOrderKey(_orderKeys);
			}

			PurchaseOrders.Items.AddRange(_poList.Select(p => new PurchaseOrderRowViewModel(p, _requestorService)));
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