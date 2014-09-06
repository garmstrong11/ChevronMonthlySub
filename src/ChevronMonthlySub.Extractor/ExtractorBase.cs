﻿namespace ChevronMonthlySub.Extractor
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using FlexCel.XlsAdapter;

	public abstract class ExtractorBase<T>
	{
		protected readonly XlsFile Xls;
		private string _sourcePath;

		protected ExtractorBase()
		{
			Xls = new XlsFile();
		}

		public string SourcePath
		{
			get { return _sourcePath; }
			set
			{
				_sourcePath = value;
				if (!File.Exists(_sourcePath))
				{
					throw new FileNotFoundException("Excel data file not found");
				}
				Xls.Open(_sourcePath);
			}
		}

		public virtual IList<T> Extract()
		{
			if (string.IsNullOrWhiteSpace(SourcePath))
			{
				throw new InvalidOperationException("No source file specified from which to extract data");
			}

			var result = new List<T>();
			Xls.ActiveSheet = 1;

			return result;
		}

		protected string ExtractString(int rowIndex, int columnIndex)
		{
			var val = Xls.GetCellValue(rowIndex, columnIndex);

			return val != null ? val.ToString() : string.Empty;
		}

		protected int ExtractInt(int rowIndex, int columnIndex)
		{
			var extract = Xls.GetCellValue(rowIndex, columnIndex);
			if (extract == null) return default(int);

			if (!(extract is double)) return default (int);

			return Convert.ToInt32(extract);
		}

		protected int ExtractIntFromString(int rowIndex, int columnIndex)
		{
			var val = default(int);
			var extract = Xls.GetCellValue(rowIndex, columnIndex);
			if (extract == null) return val;

			int.TryParse(extract.ToString(), out val);
			return val;
		}

		protected decimal ExtractDecimal(int rowIndex, int columnIndex)
		{
			var extract = Xls.GetCellValue(rowIndex, columnIndex);
			if (extract == null) return default(decimal);

			if (!(extract is double)) return default(decimal);

			return Convert.ToDecimal(extract);
		}

		protected DateTime ExtractDateTime(int rowIndex, int columnIndex)
		{
			var val = Xls.GetStringFromCell(rowIndex, columnIndex);

			DateTime dt;
			if (DateTime.TryParse(val, out dt)) return dt;

			var msg = string.Format("Bad date format at row {0} column {1} of file {2}",
				rowIndex, columnIndex, Xls.ActiveFileName);

			throw new FormatException(msg);
		}
	}
}