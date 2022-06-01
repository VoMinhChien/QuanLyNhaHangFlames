﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI_QLNH
{
    public class Utils
    {
        // Kiểm tra xem textbox có rỗng hay ko
        // Rỗng thì hiển thị messagebox và focus => false
        // Đã nhập => true
        public static bool KiemTraTextBox(TextBox textBox, string name)
        {
            if (textBox.Text.Trim().Length == 0)
            {
                HienWarning($"Vui lòng nhập {name}");

                textBox.Focus();

                return false;
            }

            return true;
        }

        // Xác nhận như thoát, xoá, sửa
        public static bool XacNhan(string message)
        {
            DialogResult result = MessageBox.Show(message, "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            return result == DialogResult.Yes;
        }

        public static void HienError(string message)
        {
            MessageBox.Show(message
                    , "Lỗi"
                    , MessageBoxButtons.OK
                    , MessageBoxIcon.Error);
        }

        public static void HienWarning(string message)
        {
            MessageBox.Show(message
                    , "Cảnh báo"
                    , MessageBoxButtons.OK
                    , MessageBoxIcon.Warning);
        }

        public static void HienThongBao(string message)
        {
            MessageBox.Show(message
                    , "Thông báo"
                    , MessageBoxButtons.OK
                    , MessageBoxIcon.Information);
        }

        //----------------
        public static string MaHoa(string password)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] encrypt;
            UTF8Encoding encode = new UTF8Encoding();
            encrypt = md5.ComputeHash(encode.GetBytes(password));
            StringBuilder encrypdata = new StringBuilder();
            for (int i = 0; i < encrypt.Length; i++)
            {
                encrypdata.Append(encrypt[i].ToString("X2"));

            }
            return encrypdata.ToString();
        }
        // export excel
        public void Export(DataGridView dtgv,DataTable dt, string sheetName, string title)
        {

            //Tạo các đối tượng Excel

            Microsoft.Office.Interop.Excel.Application oExcel = new Microsoft.Office.Interop.Excel.Application();

            Microsoft.Office.Interop.Excel.Workbooks oBooks;

            Microsoft.Office.Interop.Excel.Sheets oSheets;

            Microsoft.Office.Interop.Excel.Workbook oBook;

            Microsoft.Office.Interop.Excel.Worksheet oSheet;

            //Tạo mới một Excel WorkBook 

            oExcel.Visible = true;

            oExcel.DisplayAlerts = false;

            oExcel.Application.SheetsInNewWorkbook = 1;

            oBooks = oExcel.Workbooks;

            oBook = (Microsoft.Office.Interop.Excel.Workbook)(oExcel.Workbooks.Add(Type.Missing));

            oSheets = oBook.Worksheets;

            oSheet = (Microsoft.Office.Interop.Excel.Worksheet)oSheets.get_Item(1);

            oSheet.Name = sheetName;

            // Tạo phần đầu 

            Microsoft.Office.Interop.Excel.Range head = oSheet.get_Range("A3", "F3");

            head.MergeCells = true;

            head.Value2 = title;

            head.Font.Bold = true;

            head.Font.Name = "Tahoma";

            head.Font.Size = "18";

            head.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            //chèn logo
            string saveDirectory = Application.StartupPath.Substring(0, (Application.StartupPath.Length - 10));
            oSheet.Shapes.AddPicture(saveDirectory + "\\Resources\\logo.png", Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, 1, 1, 60, 60);
            // Tạo tiêu đề cột 
            for (int i = 0; i < dtgv.ColumnCount; i++)
            {
                Microsoft.Office.Interop.Excel.Range cl1 = oSheet.get_Range("A5", "A5");

                cl1.Value2 = dtgv.Columns[0].HeaderText;

                cl1.ColumnWidth = 13.5;
                Microsoft.Office.Interop.Excel.Range cl2 = oSheet.get_Range("B5", "B5");

                cl2.Value2 = dtgv.Columns[1].HeaderText;

                cl2.ColumnWidth = 25.0;

                Microsoft.Office.Interop.Excel.Range cl3 = oSheet.get_Range("C5", "C5");

                cl3.Value2 = dtgv.Columns[2].HeaderText;

                cl3.ColumnWidth = 15.0;
                Microsoft.Office.Interop.Excel.Range cl4 = oSheet.get_Range("D5", "D5");

                cl4.Value2 = dtgv.Columns[3].HeaderText;

                cl4.ColumnWidth = 25.0;
                Microsoft.Office.Interop.Excel.Range cl5 = oSheet.get_Range("E5", "E5");

                cl5.Value2 = dtgv.Columns[4].HeaderText;

                cl5.ColumnWidth = 25.0;
                Microsoft.Office.Interop.Excel.Range cl6= oSheet.get_Range("F5", "F5");

                cl6.Value2 = dtgv.Columns[5].HeaderText;

                cl6.ColumnWidth = 25.0;
            }
            Microsoft.Office.Interop.Excel.Range rowHead = oSheet.get_Range("A5", "F5");

            rowHead.Font.Bold = true;

            // Kẻ viền

            rowHead.Borders.LineStyle = Microsoft.Office.Interop.Excel.Constants.xlSolid;

            // Thiết lập màu nền

            rowHead.Interior.ColorIndex = 20;

            rowHead.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

            // Tạo mẳng đối tượng để lưu dữ toàn bồ dữ liệu trong DataTable,

            // vì dữ liệu được được gán vào các Cell trong Excel phải thông qua object thuần.

            object[,] arr = new object[dt.Rows.Count, dt.Columns.Count];

            //Chuyển dữ liệu từ DataTable vào mảng đối tượng

            for (int r = 0; r < dt.Rows.Count; r++)
            {
                DataRow dr = dt.Rows[r];

                for (int c = 0; c < dt.Columns.Count; c++)
                {
                    arr[r, c] = dr[c];
                }
            }
            
            //Thiết lập vùng điền dữ liệu

            int rowStart = 6;

            int columnStart = 1;

            int rowEnd = rowStart + dt.Rows.Count - 1;

            int columnEnd = dt.Columns.Count;

            // Ô bắt đầu điền dữ liệu

            Microsoft.Office.Interop.Excel.Range c1 = (Microsoft.Office.Interop.Excel.Range)oSheet.Cells[rowStart, columnStart];

            // Ô kết thúc điền dữ liệu

            Microsoft.Office.Interop.Excel.Range c2 = (Microsoft.Office.Interop.Excel.Range)oSheet.Cells[rowEnd, columnEnd];

            // Lấy về vùng điền dữ liệu

            Microsoft.Office.Interop.Excel.Range range = oSheet.get_Range(c1, c2);

            //Điền dữ liệu vào vùng đã thiết lập

            range.Value2 = arr;

            // Kẻ viền

            range.Borders.LineStyle = Microsoft.Office.Interop.Excel.Constants.xlSolid;

            // Căn giữa cột STT

            Microsoft.Office.Interop.Excel.Range c3 = (Microsoft.Office.Interop.Excel.Range)oSheet.Cells[rowEnd, columnStart];

            Microsoft.Office.Interop.Excel.Range c4 = oSheet.get_Range(c1, c3);

            oSheet.get_Range(c3, c4).HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

            // căn giữa cột ngày
            Microsoft.Office.Interop.Excel.Range c7 = (Microsoft.Office.Interop.Excel.Range)oSheet.Cells[6, 2];
            Microsoft.Office.Interop.Excel.Range c9 = (Microsoft.Office.Interop.Excel.Range)oSheet.Cells[rowEnd, 2];
            Microsoft.Office.Interop.Excel.Range c10 = oSheet.get_Range(c7, c9);
            oSheet.get_Range(c9, c10).HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
        }
    }
}

