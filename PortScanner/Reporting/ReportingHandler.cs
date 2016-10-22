﻿using Microsoft.Office.Interop.Excel;
using System;
using System.Windows.Forms;
using TextBox = System.Windows.Forms.TextBox;

namespace PortScanner.Reporting
{
    public class ReportingHandler
    {
        public static int ReportType = 1;

        public void SetReportType(Enum.ReportType reportType)
        {
            ReportType = (int)reportType;
        }

        public SaveFileDialog GetSaveFileDialog()
        {
            var saveFileDialog = new SaveFileDialog();
            switch (ReportType)
            {
                case 1:
                    //configure text
                    saveFileDialog = ConfigureSaveFileDialog((Enum.ReportType)ReportType);

                    break;

                case 2:
                    //configure xls
                    saveFileDialog = ConfigureSaveFileDialog((Enum.ReportType)ReportType);

                    break;

                case 3:
                    //configure csv
                    saveFileDialog = ConfigureSaveFileDialog((Enum.ReportType)ReportType);
                    break;
            }
            return saveFileDialog;
        }

        public string BuildTextFile(TextBox mainWindowTextBox)
        {
            return mainWindowTextBox.Text;
        }

        public int GetReportType()
        {
            return ReportType;
        }

        public void BuildWorkBook(TextBox mainWindowTextBox, string path)
        {
            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();

            if (xlApp == null)
            {
                MessageBox.Show("Excel is not properly installed!!");
                return;
            }

            Workbook xlWorkBook;
            Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;

            xlWorkBook = xlApp.Workbooks.Add(misValue);
            xlWorkSheet = (Worksheet)xlWorkBook.Worksheets.get_Item(1);
            xlWorkSheet.Cells[1, 1] = "Sheet 1 content";

            xlWorkBook.SaveAs(path, XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();

            releaseObject(xlWorkSheet);
            releaseObject(xlWorkBook);
            releaseObject(xlApp);

            MessageBox.Show("Excel file created , you can find the file here: " + path);
        }

        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Exception Occured while releasing object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }

        public string GetSaveFileLocation(Enum.ReportType reportType)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (reportType == Enum.ReportType.Txt)
            {
                saveFileDialog = ConfigureSaveFileDialog(reportType);
            }
            else if (reportType == Enum.ReportType.Xls)
            {
                saveFileDialog = ConfigureSaveFileDialog(reportType);
            }
            var result = saveFileDialog.ShowDialog();
            return saveFileDialog.FileName;
        }

        private SaveFileDialog ConfigureSaveFileDialog(Enum.ReportType reportType)
        {
            var saveFileDialog = new SaveFileDialog();
            if (reportType == Enum.ReportType.Txt)
            {
                saveFileDialog.DefaultExt = ".txt";
                saveFileDialog.Filter = "Text documents (.txt)|*.txt";
            }
            else if (reportType == Enum.ReportType.Xls)
            {
                saveFileDialog.DefaultExt = ".xls";
                saveFileDialog.Filter = "Excel documents (.xls)|*.xls";
            }
            else
            {
                saveFileDialog.DefaultExt = ".csv";
                saveFileDialog.Filter = "CSV documents (.csv)|*.csv";
            }
            return saveFileDialog;
        }
    }
}