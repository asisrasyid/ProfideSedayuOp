using DocumentFormat.OpenXml.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Web;

namespace ProfideSedayuOp.Models.Helper
{
    public class ParamAuth
    {
        public string Acces { get; set; }
    }
    public class Response
    {
        public string NoTransaksi { get; set; }
        public string cabang { get; set; }
        public string Nama_PT { get; set; }
        public string Nama { get; set; }
        public string Alamat { get; set; }
        public string Email { get; set; }
        public string NoTelp { get; set; }
        public string NamaPT_Ceking { get; set; }
        public string NIK_Ceking { get; set; }
        public string TglLahir_Ceking { get; set; }
        public string Status_Pengajuan { get; set; }
        public string Status_PengajuanDesc { get; set; }
        public string Tgl_Pengajuan { get; set; }
        public string Tgl_Update { get; set; }
        public string Tgl_Voucher { get; set; }
        public string SLA { get; set; }
        public string TipeTransaksi_Ceking { get; set; }
        public string Jenis_Badan { get; set; }
        public string Wilayah_PT { get; set; }
        public string Kode_Voucher { get; set; }
        public string PNBP { get; set; }
        public string TipeTransaksi_CekingDesc { get; set; }
        public string TipeTransaksi_Par { get; set; }
        public string TipeTransaksi_Val { get; set; }
        public string File_pdf { get; set; }
        public string User_Pengaju { get; set; }
        public string data_id { get; set; }
        public string mxy_no { get; set; }
        public string Keterangan { get; set; }
    }
    public class InvoiceInput
    {
        public string invoice { get; set; }
        public string tglinv { get; set; }
        public string tgltmp { get; set; }
        public string pnbp { get; set; }
        public string jasatot { get; set; }
        public string periodetot { get; set; }
        public string jpsatu { get; set; }
        public string jidua { get; set; }
        public string jdpp { get; set; }
        public string jppn { get; set; }
        public string jpph { get; set; }
        public string jtot { get; set; }
        public string tanggalctts { get; set; }
        public string templatePath { get; set; }
        public string outputPath { get; set; }
        public string terbilang { get; set; }
        public string NamaCabang { get; set;}
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string cabang { get; set;}
        public string NoFaktur { get; set; }
    }
    public class TableRow2
    {
        public string Column1 { get; set; }
        public string Column2 { get; set; }
        public string Column3 { get; set; }
    }
}