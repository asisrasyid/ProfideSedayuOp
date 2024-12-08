using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;
using ProfideSedayuOp.Controllers;


namespace ProfideSedayuOp.Models.Helper
{
    public class ExcdataHelper
    {
        public async Task<DataTable> ExecuteDataTableExtendConn(SqlConnection connection, string CommandText, SqlParameter[] parameters, SqlTransaction Transaction = null)
        {
            DataTable dt = new DataTable();
            using (SqlCommand command = new SqlCommand())
            {
                try
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = CommandText;
                    command.CommandTimeout = 30;
                    if (Transaction != null)
                    {
                        command.Transaction = Transaction;
                    }

                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters.ToArray());
                    }

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        try
                        {
                            if (reader.HasRows)
                            {
                                dt.Load(reader);
                            }

                            reader.Close();
                            reader.Dispose();
                            Transaction?.Commit();
                        }
                        catch
                        {
                            reader.Close();
                            reader.Dispose();
                            Transaction?.Rollback();
                        }
                }
                catch (Exception ex2)
                {
                    Exception ex = ex2;
                    string msg = ex.Message.ToString();
                    Transaction?.Rollback();
                    connection.Close();
                }
            }

            return dt;
        }

        public async Task<DataTable> ExecuteDataTable(string strconnection, string CommandText, SqlParameter[] parameters, SqlTransaction Transaction = null)
        {
            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(strconnection))
            {
                using (SqlCommand command = new SqlCommand())
                    try
                    {
                        command.Connection = connection;
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = CommandText;
                        command.CommandTimeout = 30;
                        if (Transaction != null)
                        {
                            command.Transaction = Transaction;
                        }

                        await connection.OpenAsync();
                        if (parameters != null)
                        {
                            command.Parameters.AddRange(parameters.ToArray());
                        }

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                            try
                            {
                                if (reader.HasRows)
                                {
                                    dt.Load(reader);
                                }

                                reader.Close();
                                reader.Dispose();
                                Transaction?.Commit();
                            }
                            catch
                            {
                                reader.Close();
                                reader.Dispose();
                                Transaction?.Rollback();
                            }
                    }
                    catch (Exception ex2)
                    {
                        Exception ex = ex2;
                        string msg = ex.Message.ToString();
                        Transaction?.Rollback();
                        connection.Close();
                    }
            }

            return dt;
        }

        public DataTable ExecuteDataTableNonAsync(string strconnection, string CommandText, SqlParameter[] parameters, SqlTransaction Transaction = null)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection sqlConnection = new SqlConnection(strconnection))
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                    try
                    {
                        sqlCommand.Connection = sqlConnection;
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.CommandText = CommandText;
                        sqlCommand.CommandTimeout = 30;
                        if (Transaction != null)
                        {
                            sqlCommand.Transaction = Transaction;
                        }

                        sqlConnection.Open();
                        if (parameters != null)
                        {
                            sqlCommand.Parameters.AddRange(parameters.ToArray());
                        }

                        using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                            try
                            {
                                if (sqlDataReader.HasRows)
                                {
                                    dataTable.Load(sqlDataReader);
                                }

                                sqlDataReader.Close();
                                sqlDataReader.Dispose();
                                Transaction?.Commit();
                            }
                            catch
                            {
                                sqlDataReader.Close();
                                sqlDataReader.Dispose();
                                Transaction?.Rollback();
                            }
                    }
                    catch (Exception ex)
                    {
                        string lines = ex.Message.ToString();
                        Transaction?.Rollback();
                        sqlConnection.Close();
                    }
            }

            return dataTable;
        }

        public async Task<string> ExecuteNonQuery(string strconnection, string CommandText, SqlParameter[] parameters, SqlTransaction Transaction = null)
        {
            string result = "";
            using (SqlConnection connection = new SqlConnection(strconnection))
            {
                using (SqlCommand command = new SqlCommand())
                    try
                    {
                        command.Connection = connection;
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = CommandText;
                        command.CommandTimeout = 30;
                        if (Transaction != null)
                        {
                            command.Transaction = Transaction;
                        }

                        await connection.OpenAsync();
                        if (parameters != null)
                        {
                            command.Parameters.AddRange(parameters.ToArray());
                        }

                        await command.ExecuteNonQueryAsync();
                        Transaction?.Commit();
                    }
                    catch (Exception ex2)
                    {
                        Exception ex = ex2;
                        result = ex.Message.ToString();
                        Transaction?.Rollback();
                        connection.Close();
                    }
            }

            return result;
        }

        public async Task<SqlCommand> ExecuteNonQueryWithOutput(string strconnection, string CommandText, SqlParameter[] parameters, SqlTransaction Transaction = null)
        {
            SqlCommand result = new SqlCommand();
            using (SqlConnection connection = new SqlConnection(strconnection))
            {
                using (SqlCommand command = new SqlCommand())
                    try
                    {
                        command.Connection = connection;
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = CommandText;
                        command.CommandTimeout = 30;
                        if (Transaction != null)
                        {
                            command.Transaction = Transaction;
                        }

                        await connection.OpenAsync();
                        if (parameters != null)
                        {
                            command.Parameters.AddRange(parameters.ToArray());
                        }

                        await command.ExecuteNonQueryAsync();
                        _ = command.Parameters;
                        result = command;
                        Transaction?.Commit();
                    }
                    catch (Exception ex2)
                    {
                        Exception ex = ex2;
                        string msg = ex.Message.ToString();
                        Transaction?.Rollback();
                        command.Parameters.Clear();
                        connection.Close();
                    }
            }

            return result;
        }
        public List<Dictionary<string, object>> ConvertDataTableToList(DataTable dt)
        {
            var result = new List<Dictionary<string, object>>();
            foreach (DataRow row in dt.Rows)
            {
                var rowDict = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    rowDict[col.ColumnName] = row[col];
                }
                result.Add(rowDict);
            }
            return result;
        }

    }

    public class ExcelHelper
    {
        public async Task AddTableDataToExcel(string filePath, string sheetName, List<Response> data, string outputPath)
        {
            if (!System.IO.File.Exists(filePath))
            {
                throw new Exception($"File Excel tidak ditemukan: {filePath}");
            }
            // Pastikan file output tidak terkunci sebelumnya
            if (System.IO.File.Exists(outputPath))
            {
                System.IO.File.Delete(outputPath); // Hapus file lama jika sudah ada
            }

            // Salin template ke lokasi output
            System.IO.File.Copy(filePath, outputPath);

            using (SpreadsheetDocument document = SpreadsheetDocument.Open(outputPath, true))
            {
                WorkbookPart workbookPart = document.WorkbookPart;
                if (workbookPart == null)
                {
                    throw new Exception("WorkbookPart tidak ditemukan!");
                }

                Sheet sheet = workbookPart.Workbook.Descendants<Sheet>()
                    .FirstOrDefault(s => s.Name == sheetName);

                if (sheet == null)
                {
                    throw new Exception($"Sheet {sheetName} tidak ditemukan!");
                }

                WorksheetPart worksheetPart = (WorksheetPart)workbookPart.GetPartById(sheet.Id);
                if (worksheetPart == null)
                {
                    throw new Exception("WorksheetPart tidak ditemukan!");
                }

                AddTableToSheet(worksheetPart, data);
            }
        }

        private void AddTableToSheet(WorksheetPart worksheetPart, List<Response> data)
        {
            SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

            int no = 0;
            foreach (var row in data)
            {
                no++;
                Row newRow = new Row();

                string tglPengajuan = FormatDate(row.Tgl_Pengajuan);
                string tglUpdate = FormatDate(row.Tgl_Update);
                newRow.Append(
                    CreateCell(no.ToString()),
                    CreateCell(row.NoTransaksi),
                    CreateCell(row.cabang),
                    CreateCell(row.NamaPT_Ceking),
                    CreateCell(row.Status_PengajuanDesc),
                    CreateCell(tglPengajuan),  // Menggunakan tanggal yang diformat
                    CreateCell(tglUpdate),
                    CreateCell(row.Jenis_Badan),
                    CreateCell(row.Wilayah_PT),
                    CreateCell(row.Kode_Voucher),
                    CreateCell(row.TipeTransaksi_CekingDesc),
                    CreateCell(row.User_Pengaju),
                    CreateCell(row.SLA),
                    CreateCell(row.Keterangan)
                );
                sheetData.Append(newRow);
            }
            worksheetPart.Worksheet.Save();
        }
        private string FormatDate(string date)
        {
            if (DateTime.TryParse(date, out DateTime parsedDate))
            {
                return parsedDate.ToString("yyyy-MM-dd"); // Formatkan tanggal ke YYYY-MM-DD
            }
            return string.Empty; // Jika tidak valid, kembalikan string kosong
        }

        private Cell CreateCell(string text)
        {
            return new Cell
            {
                DataType = CellValues.String,
                CellValue = new CellValue(text)
            };
        }
    }

}
