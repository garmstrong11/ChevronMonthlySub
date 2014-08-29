namespace ChevronMonthlySub.UI.ViewModels
{
	using System.IO;
	using System.Windows;
	using Caliburn.Micro;
	using Infra;

	public class ShellViewModel : Screen, IShell
	{
		private string _rowSourcePath;

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

		public void HandleFolderDrag(object evtArgs)
		{
			var args = (DragEventArgs)evtArgs;

			var filePaths = (string[])args.Data.GetData(DataFormats.FileDrop);
			var info = new FileInfo(filePaths[0]);

			if (info.IsDirectory()) return;
			if (info.Extension != ".xlsx")

			args.Effects = DragDropEffects.Link;
			args.Handled = true;
		}

		public void HandleFolderDrop(ActionExecutionContext ctx)
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
	}
}