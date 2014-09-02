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
		private string _invoiceId;
		private readonly List<PurchaseOrder> _poList;

		public ShellViewModel(IPurchaseOrderService purchaseOrderService)
		{
			_purchaseOrderService = purchaseOrderService;
			PurchaseOrders = new PurchaseOrdersViewModel();
			_poList = new List<PurchaseOrder>(); 
		}

		private void Reset()
		{
			PurchaseOrders.Items.Clear();
			_poList.Clear();
			InvoiceId = string.Empty;
		}

		public string RowSourcePath
		{
			get { return _rowSourcePath; }
			set
			{
				Reset();
				string id;
				if (value == _rowSourcePath) return;

				if (!TryExtractInvoiceIdFromFilePath(value, out id)) return;

				InvoiceId = id;
				_rowSourcePath = value;
				_purchaseOrderService.SourcePath = _rowSourcePath;
				_poList.AddRange(_purchaseOrderService.GetFreightPurchaseOrders(id));
				_poList.AddRange(_purchaseOrderService.GetProductPurchaseOrders(id));

				PurchaseOrders.Items.AddRange(_poList.Select(p => new PurchaseOrderRowViewModel(p)));

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
			var boxName = ctx.Source.Name;
			var folderPaths = (string[])args.Data.GetData(DataFormats.FileDrop);
			var info = new FileInfo(folderPaths[0]);

			if (info.IsDirectory()) return;

			args.Effects = DragDropEffects.Link;

			if (boxName == "RowSourcePath")
			{
				RowSourcePath = folderPaths[0];
			}
			//else
			//{
			//	SourcePath = folderPaths[0];
			//}
			args.Handled = true;
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