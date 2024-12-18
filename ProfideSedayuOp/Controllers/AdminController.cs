using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ProfideSedayuOp.Models;
using ProfideSedayuOp.Models.Mitsui;
using ProfideSedayuOp.Models.Helper;
using System.Web.DynamicData;
using System.Web.Http.Results;
using System.IO;
using System.Drawing;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml;
using System.Web.Http;
using DocumentFormat.OpenXml.Office.Word;
using Microsoft.Ajax.Utilities;
using Spire.Doc;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using Microsoft.Office.Interop.Word;

namespace ProfideSedayuOp.Controllers
{
    public class AdminController : Controller
    {
        public ActionResult Index()
        {
            var xx = Session["DataLog"];
            var log = Session["Name"];
            var cap = Session["Caption"];
            if (xx == null || xx.ToString() == "")
            {
                return RedirectToAction("auth_login_basic", "Auth");
            }
            ViewBag.User = log;
            ViewBag.Data = xx;
            ViewBag.Caption = cap;

            return View();
        }
        //public ActionResult app_invoice_add()
        //{
        //    var xx = Session["DataLog"];
        //    var log = Session["Name"];
        //    if (xx == null || xx.ToString() == "")
        //    {
        //        return RedirectToAction("auth_login_basic", "Auth");
        //    }
        //    ViewBag.User = log;
        //    ViewBag.Data = xx;

        //    return View();
        //}
        //public ActionResult app_invoice_edit()
        //{
        //    var xx = Session["DataLog"];
        //    var log = Session["Name"];
        //    if (xx == null || xx.ToString() == "")
        //    {
        //        return RedirectToAction("auth_login_basic", "Auth");
        //    }
        //    ViewBag.User = log;
        //    ViewBag.Data = xx;

        //    return View();
        //}
        public ActionResult app_invoice_list()
        {
            var xx = Session["DataLog"];
            var log = Session["Name"];
            if (xx == null || xx.ToString() == "")
            {
                return RedirectToAction("auth_login_basic", "Auth");
            }
            ViewBag.User = log;
            ViewBag.Data = xx;

            return View();
        }
        //public ActionResult app_invoice_preview()
        //{
        //    var xx = Session["DataLog"];
        //    var log = Session["Name"];
        //    if (xx == null || xx.ToString() == "")
        //    {
        //        return RedirectToAction("auth_login_basic", "Auth");
        //    }
        //    ViewBag.User = log;
        //    ViewBag.Data = xx;

        //    return View();
        //}
        //public ActionResult app_invoice_print()
        //{
        //    var xx = Session["DataLog"];
        //    var log = Session["Name"];
        //    if (xx == null || xx.ToString() == "")
        //    {
        //        return RedirectToAction("auth_login_basic", "Auth");
        //    }
        //    ViewBag.User = log;
        //    ViewBag.Data = xx;

        //    return View();
        //}
        //public ActionResult vmapp_invoice_EditMit()
        //{
        //    var xx = Session["DataLog"];
        //    var log = Session["Name"];
        //    if (xx == null || xx.ToString() == "")
        //    {
        //        return RedirectToAction("auth_login_basic", "Auth");
        //    }
        //    ViewBag.User = log;
        //    ViewBag.Data = xx;

        //    return View();
        //}
        //public ActionResult app_invoice_select()
        //{
        //    var xx = Session["DataLog"];
        //    var log = Session["Name"];
        //    if (xx == null || xx.ToString() == "")
        //    {
        //        return RedirectToAction("auth_login_basic", "Auth");
        //    }
        //    ViewBag.User = log;
        //    ViewBag.Data = xx;

        //    return View();
        //}
        //public ActionResult vmapp_invoice_edit()
        //{
        //    var xx = Session["DataLog"];
        //    var log = Session["Name"];
        //    if (xx == null || xx.ToString() == "")
        //    {
        //        return RedirectToAction("auth_login_basic", "Auth");
        //    }
        //    ViewBag.User = log;
        //    ViewBag.Data = xx;

        //    return View();
        //}

        public ActionResult Result()
        {
            var xx = Session["DataLog"];
            var log = Session["Name"];
            if (xx == null || xx.ToString() == "")
            {
                return RedirectToAction("auth_login_basic", "Auth");
            }
            ViewBag.User = log;
            ViewBag.Data = xx;

            return View();
        }
        public ActionResult Laporan()
        {
            var xx = Session["DataLog"];
            var log = Session["Name"];
            if (xx == null || xx.ToString() == "")
            {
                return RedirectToAction("auth_login_basic", "Auth");
            }
            ViewBag.User = log;
            ViewBag.Data = xx;

            return View();
        }

        //public async Task<ActionResult> TestApiIntegration(ParamGetData param)
        //{
        //    MitsuiPengecekan data = new MitsuiPengecekan();
        //    var dt = new DataTable();
        //    dt = await data.dbGetDataPengecekan(param);
        //    var jsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(dt);
        //    return Content(jsonResult, "application/json");
        //}
        public async Task<ActionResult> GroupAksesChange(string Action)
        {
            var AccCheck = Session["Admin"];
            var xx = Session["DataLog"];
            var DbCek = xx?.ToString();
           
            if (AccCheck.ToString()== "ADMINISTRATOR")
            {
                GetData nw = new GetData();
                ParamGetPengajuan dbs = new ParamGetPengajuan();
                if (Action == "Mitsui")
                {
                    Session["DataLog"] = "MIT-FIN";
                    string jsonFilePath = Server.MapPath("~/Content/assets/json/BackData.json");
                    string strconnectionmit = OwinLibrary.GetAPIMitsu();
                    var apiResponse = await nw.GetDataPengecekanAPI(dbs, strconnectionmit);
                    if (string.IsNullOrEmpty(apiResponse))
                    {
                        ViewBag.Error = "Terjadi Kesalahan Silahkan Coba Lagi";
                        return View("auth_login_basic");
                    }
                    var responseObject = JsonConvert.DeserializeObject<object>(apiResponse);
                    string jsonContent = JsonConvert.SerializeObject(responseObject, Formatting.Indented);
                    System.IO.File.WriteAllText(jsonFilePath, jsonContent);
                    if (Action == "Mitsui")
                    {
                        string jsonFilePath2 = Server.MapPath("~/Content/assets/json/BackDataDone.json");
                        ParamGetPengajuan dbo = new ParamGetPengajuan();
                        dbo.Status = "40";
                        var apiResponse2 = await nw.GetDataPengecekanAPI(dbo, strconnectionmit);
                        if (string.IsNullOrEmpty(apiResponse2))
                        {
                            ViewBag.Error = "Terjadi Kesalahan Silahkan Coba Lagi";
                            return View("auth_login_basic");
                        }
                        var responseObject2 = JsonConvert.DeserializeObject<object>(apiResponse2);
                        string jsonContent2 = JsonConvert.SerializeObject(responseObject2, Formatting.Indented);
                        System.IO.File.WriteAllText(jsonFilePath2, jsonContent2);
                        if (Action == "Mitsui")
                        {
                            string jsonFilePathInvoice = Server.MapPath("~/Content/assets/json/BackDataInv.json");
                            ParamGetPengajuan NewForINVdbo = new ParamGetPengajuan();
                            NewForINVdbo.Tipe = "002";
                            var ResponseDataInv = await nw.GetDataPengecekanAPI(NewForINVdbo, strconnectionmit);
                            if (string.IsNullOrEmpty(ResponseDataInv))
                            {
                                ViewBag.Error = "Terjadi Kesalahan Silahkan Coba Lagi";
                                return View("auth_login_basic");
                            }
                            var ResponseDataInvObject = JsonConvert.DeserializeObject<object>(ResponseDataInv);
                            string ResponseDataInvContent = JsonConvert.SerializeObject(ResponseDataInvObject, Formatting.Indented);
                            System.IO.File.WriteAllText(jsonFilePathInvoice, ResponseDataInvContent);
                            Session["Caption"] = "MITSUI LEASING CAPITAL INDONESIA";
                            return RedirectToAction("Index", "Home");
                        }
                    }

                } else if (Action == "Mandiri")
                {
                    Session["DataLog"] = "MTF-FIN";
                    string jsonFilePath = Server.MapPath("~/Content/assets/json/BackData.json");
                    string strconnectionmtf = OwinLibrary.GetAPIMTF();
                    var apiResponse = await nw.GetDataPengecekanAPI(dbs, strconnectionmtf);
                    if (string.IsNullOrEmpty(apiResponse))
                    {
                        ViewBag.Error = "Terjadi Kesalahan Silahkan Coba Lagi";
                        return View("auth_login_basic"); ;
                    }
                    var responseObject = JsonConvert.DeserializeObject<object>(apiResponse);
                    string jsonContent = JsonConvert.SerializeObject(responseObject, Formatting.Indented);
                    System.IO.File.WriteAllText(jsonFilePath, jsonContent);
                    if (Action == "Mandiri")
                    {
                        string jsonFilePath2 = Server.MapPath("~/Content/assets/json/BackDataDone.json");
                        ParamGetPengajuan dbo = new ParamGetPengajuan();
                        dbo.Status = "40";
                        var apiResponse2 = await nw.GetDataPengecekanAPI(dbo, strconnectionmtf);
                        if (string.IsNullOrEmpty(apiResponse))
                        {
                            ViewBag.Error = "Terjadi Kesalahan Silahkan Coba Lagi";
                            return View("auth_login_basic");
                        }
                        var responseObject2 = JsonConvert.DeserializeObject<object>(apiResponse2);
                        string jsonContent2 = JsonConvert.SerializeObject(responseObject2, Formatting.Indented);
                        System.IO.File.WriteAllText(jsonFilePath2, jsonContent2);
                        if (Action == "Mandiri")
                        {
                            string jsonFilePathInvoice = Server.MapPath("~/Content/assets/json/BackDataInv.json");
                            ParamGetPengajuan NewForINVdbo = new ParamGetPengajuan();
                            NewForINVdbo.Tipe = "002";
                            var ResponseDataInv = await nw.GetDataPengecekanAPI(NewForINVdbo, strconnectionmtf);
                            if (string.IsNullOrEmpty(ResponseDataInv))
                            {
                                ViewBag.Error = "Terjadi Kesalahan Silahkan Coba Lagi";
                                return View("auth_login_basic");
                            }
                            var ResponseDataInvObject = JsonConvert.DeserializeObject<object>(ResponseDataInv);
                            string ResponseDataInvContent = JsonConvert.SerializeObject(ResponseDataInvObject, Formatting.Indented);
                            System.IO.File.WriteAllText(jsonFilePathInvoice, ResponseDataInvContent);
                            Session["Caption"] = "MANDIRI TUNAS FINANCE";
                            return RedirectToAction("Index", "Home");
                        }
                    }
                } else if (Action == "Admin"){
                    Session["DataLog"] = "ADMIN";
                    string jsonMitsui = Server.MapPath("~/Content/assets/json/BackData-Mit.json");
                    string jsonMandiri = Server.MapPath("~/Content/assets/json/BackData-Mtf.json");
                    string strconnectionmit = OwinLibrary.GetAPIMitsu();
                    string strconnectionmandiri = OwinLibrary.GetAPIMTF();
                    var apiResponseMisui = await nw.GetDataPengecekanAPI(dbs, strconnectionmit);
                    var apiResponseMandiri = await nw.GetDataPengecekanAPI(dbs, strconnectionmandiri);
                    if (string.IsNullOrEmpty(apiResponseMisui) || string.IsNullOrEmpty(apiResponseMandiri))
                    {
                        ViewBag.Error = "Terjadi Kesalahan Silahkan Coba Lagi";
                        return View("auth_login_basic");
                    }
                    var responseObjectMitsui = JsonConvert.DeserializeObject<object>(apiResponseMisui);
                    var responseObjectMandiri = JsonConvert.DeserializeObject<object>(apiResponseMandiri);
                    string jsonContentMitsui = JsonConvert.SerializeObject(responseObjectMitsui, Formatting.Indented);
                    string jsonContentMadiri = JsonConvert.SerializeObject(responseObjectMandiri, Formatting.Indented);
                    System.IO.File.WriteAllText(jsonMitsui, jsonContentMitsui);
                    System.IO.File.WriteAllText(jsonMandiri, jsonContentMadiri);
                    if (Action == "Admin")
                    {
                        string jsonFilePathDoneMit = Server.MapPath("~/Content/assets/json/BackDataDone-Mit.json");
                        string jsonFilePathDoneMan = Server.MapPath("~/Content/assets/json/BackDataDone-Mtf_.json");
                        ParamGetPengajuan dbo = new ParamGetPengajuan();
                        dbo.Status = "40";
                        var apiResponseMitsuiDone = await nw.GetDataPengecekanAPI(dbo, strconnectionmit);
                        var apiResponseMandiriDone = await nw.GetDataPengecekanAPI(dbo, strconnectionmandiri);
                        if (string.IsNullOrEmpty(apiResponseMitsuiDone) || string.IsNullOrEmpty(apiResponseMandiriDone))
                        {
                            ViewBag.Error = "Terjadi Kesalahan Silahkan Coba Lagi";
                            return View("auth_login_basic");
                        }
                        var responseObjectMistuiDone = JsonConvert.DeserializeObject<object>(apiResponseMitsuiDone);
                        var responseObjectMandiriDone = JsonConvert.DeserializeObject<object>(apiResponseMandiriDone);
                        string jsonContentMitsuiDone = JsonConvert.SerializeObject(responseObjectMistuiDone, Formatting.Indented);
                        string jsonContentMandiriDone = JsonConvert.SerializeObject(responseObjectMandiriDone, Formatting.Indented);
                        System.IO.File.WriteAllText(jsonFilePathDoneMit, jsonContentMitsuiDone);
                        System.IO.File.WriteAllText(jsonFilePathDoneMan, jsonContentMandiriDone);
                        if (Action == "Admin")
                        {
                            string jsonFilePathInvoiceMitsui = Server.MapPath("~/Content/assets/json/BackDataInv-Mit.json");
                            string jsonFilePathInvoiceMandiri = Server.MapPath("~/Content/assets/json/BackDataInv-Mtf.json");
                            ParamGetPengajuan NewForINVdbo = new ParamGetPengajuan();
                            NewForINVdbo.Tipe = "002";
                            var ResponseDataInvMitsui = await nw.GetDataPengecekanAPI(NewForINVdbo, strconnectionmit);
                            var ResponseDataInvMandiri = await nw.GetDataPengecekanAPI(NewForINVdbo, strconnectionmandiri);
                            if (string.IsNullOrEmpty(ResponseDataInvMitsui) || string.IsNullOrEmpty(ResponseDataInvMandiri))
                            {
                                ViewBag.Error = "Terjadi Kesalahan Silahkan Coba Lagi";
                                return View("auth_login_basic");
                            }
                            var ResponseDataInvObjectMitsui = JsonConvert.DeserializeObject<object>(ResponseDataInvMitsui);
                            string ResponseDataInvContentMitsui = JsonConvert.SerializeObject(ResponseDataInvObjectMitsui, Formatting.Indented);
                            System.IO.File.WriteAllText(jsonFilePathInvoiceMitsui, ResponseDataInvContentMitsui);
                            var ResponseDataInvObjectMandiri = JsonConvert.DeserializeObject<object>(ResponseDataInvMandiri);
                            string ResponseDataInvContentMandiri = JsonConvert.SerializeObject(ResponseDataInvObjectMandiri, Formatting.Indented);
                            System.IO.File.WriteAllText(jsonFilePathInvoiceMandiri, ResponseDataInvContentMandiri);
                            Session["Caption"] = "ADMIN-PENGECEKAN PERUSAHAAN MTF-MITSUI";
                            return RedirectToAction("Index", "Admin");
                        }
                    }
                }
                return RedirectToAction("auth_login_basic", "Auth");
            }
            else
            {
                return View();
            }

        }

        public async Task<ActionResult> SaveDataPengecekan(SavePengajuan db)
        {
            try
            {
                var xx = Session["DataLog"];
                var log = Session["Name"];
                if (xx == null || xx.ToString() == "")
                {
                    return RedirectToAction("auth_login_basic", "Auth");
                }
                ViewBag.User = log;
                ViewBag.Data = xx;
                var DbCek = xx?.ToString();
                GetData mp = new GetData();

                if (DbCek == "MTF-FIN")
                {
                    string strconnection = OwinLibrary.GetAPIMTF();
                    var apiResponse = await mp.SaveDataPengecekanAPI(db, strconnection);
                    if (string.IsNullOrEmpty(apiResponse))
                    {
                        return Json(new { success = false, message = "Response kosong dari API" });
                    }
                    return Json(new { success = true, message = "Data Di Update" });
                }
                else if (DbCek == "MIT-FIN")
                {
                    string strconnection = OwinLibrary.GetAPIMitsu();
                    var apiResponse = await mp.SaveDataPengecekanAPI(db, strconnection);
                    if (string.IsNullOrEmpty(apiResponse))
                    {
                        return Json(new { success = false, message = "Response kosong dari API" });
                    }
                    return Json(new { success = true, message = "Data Di Update" });
                }

                return RedirectToAction("auth_login_basic", "Auth");


            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


        public async Task<ActionResult> GetDataPengecekan(ParamGetPengajuan dt, string Asal)
        {
            try
            {
                var xx = Session["DataLog"];
                var log = Session["Name"];
                if (xx == null || xx.ToString() == "")
                {
                    return RedirectToAction("auth_login_basic", "Auth");
                }
                ViewBag.User = log;
                ViewBag.Data = xx;
                var DbCek = xx?.ToString();
                GetData nw = new GetData();
                //PT Setia Darma Barokah
                //PT SEDAYU DANA BANDA
                if (DbCek == "ADMIN")
                {
                    if (Asal == "PT Setia Darma Barokah")
                    {
                        string strconnection = OwinLibrary.GetAPIMTF();
                        var apiResponse = await nw.GetDataPengecekanAPI(dt, strconnection);
                        if (string.IsNullOrEmpty(apiResponse))
                        {
                            return Json(new { success = false, message = "Response kosong dari API" });
                        }
                        return Content(apiResponse, "application/json");
                    }
                    else if (Asal == "PT SEDAYU DANA BANDA")
                    {
                        string strconnection = OwinLibrary.GetAPIMitsu();
                        var apiResponse = await nw.GetDataPengecekanAPI(dt, strconnection);
                        if (string.IsNullOrEmpty(apiResponse))
                        {
                            return Json(new { success = false, message = "Response kosong dari API" });
                        }
                        return Content(apiResponse, "application/json");
                    }


                }
                else
                {
                    return RedirectToAction("auth_login_basic", "Auth");
                }
               
                return RedirectToAction("auth_login_basic", "Auth");


            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        public async Task<ActionResult> SaveDBInvoiceDataNew([FromBody] List<List<string>> tableData, InvoiceInput dt, ParamSaveInvoice ds)
        {
            var xx = Session["DataLog"];
            var log = Session["Name"];
            if (xx == null || xx.ToString() == "")
            {
                return RedirectToAction("auth_login_basic", "Auth");
            }
            ViewBag.User = log;
            ViewBag.Data = xx;
            var DbCek = xx?.ToString();
            GetData nw = new GetData();
            MitsuiPengecekan dataInv = new MitsuiPengecekan();

            try
            {
                if (DbCek == "MTF-FIN")
                {
                    dt.templatePath = Server.MapPath("~/Content/tasks/template.docx");
                    dt.outputPath = Server.MapPath("~/Temp/UpdatedDocument.docx");
                    string strconnection = OwinLibrary.GetAPIMTF();
                    List<Response> rs = await dataInv.GetdataInvoice(strconnection, dt.StartDate, dt.EndDate, dt.cabang);
                    var listOfString = rs.Select((r, Index) => new List<string>
                     {
                        (Index + 1).ToString(),
                        r.Tgl_Pengajuan,
                        r.NamaPT_Ceking,
                        r.cabang,
                        r.TipeTransaksi_CekingDesc == "Data Terakhir" ? "Rp. 50.000,00" : "Rp. 500.000,00",
                        "Rp. 20.000,00"
                     }).ToList();
                    var NotransUpdateINV = rs.Select(r => new List<string>
                     {
                        r.NoTransaksi,
                     }).ToList();

                    string bookmarkName = "HasilTable";
                    InvoiceInput mod = new InvoiceInput();
                    mod.invoice = dt.invoice;
                    mod.tglinv = dt.tglinv;
                    mod.tgltmp = dt.tgltmp;
                    mod.pnbp = dt.pnbp;
                    mod.jasatot = dt.jasatot;
                    mod.periodetot = dt.periodetot;
                    mod.jpsatu = dt.jpsatu;
                    mod.jidua = dt.jidua;
                    mod.jdpp = dt.jdpp;
                    mod.jppn = dt.jppn;
                    mod.jpph = dt.jpph;
                    mod.jtot = dt.jtot;
                    mod.tanggalctts = dt.tanggalctts;
                    mod.templatePath = dt.templatePath;
                    mod.outputPath = dt.outputPath;
                    mod.terbilang = dt.terbilang;
                    mod.cabang = dt.cabang;
                    GenProfide op = new GenProfide();
                    // Memanggil metode ReplacePlaceholdersAsync untuk mengganti placeholders dengan data yang diberikan
                    await op.ReplacePlaceholdersAsync(mod, bookmarkName, listOfString);
                    //await PopulateTableAtBookmarkAsync(dt.templatePath, dt.outputPath, bookmarkName, tableData);

                    // Menambahkan proses untuk mengunduh file setelah dokumen berhasil diubah
                    byte[] fileBytes = System.IO.File.ReadAllBytes(mod.outputPath);
                    string fileName = "UpdatedDocument.docx";
                    string tempWordPath = Path.Combine(Path.GetTempPath(), fileName);
                    System.IO.File.WriteAllBytes(tempWordPath, fileBytes);

                    // Load file Word ke Spire.Doc Document
                    Spire.Doc.Document document = new Spire.Doc.Document();
                    document.LoadFromFile(tempWordPath);

                    // Konversi ke PDF
                    string tempPdfPath = Path.Combine(Path.GetTempPath(), "ConvertedDocument.pdf");
                    document.SaveToFile(tempPdfPath, FileFormat.PDF);

                    // Ambil file PDF sebagai byte[] untuk return
                    byte[] pdfBytes = System.IO.File.ReadAllBytes(tempPdfPath);
                    string FileSave = Convert.ToBase64String(pdfBytes);

                    SavePengajuan savePengajuan = new SavePengajuan();
                    savePengajuan.KodeVoucher = mod.tglinv;
                    savePengajuan.PNBP = mod.tgltmp;
                    savePengajuan.Keterangan = mod.invoice;
                    savePengajuan.filed = FileSave;
                    savePengajuan.Status = "100";
                    savePengajuan.data_id = mod.jtot;
                    savePengajuan.mxy_no = mod.jpsatu;
                    savePengajuan.UserID = mod.jpph;
                    savePengajuan.GroupName = mod.jppn;
                    savePengajuan.Tglbeli = mod.jdpp;
                    savePengajuan.NoTransksi = log.ToString();
                    var apiResponse = await dataInv.UpdateInvoice(ds, strconnection, mod, savePengajuan, rs);
                    if (string.IsNullOrEmpty(apiResponse))
                    {
                        return Json(new { success = false, message = "Gagal Update Data" });
                    }
                    return Content(apiResponse, "application/json");
                }
                else if (DbCek == "MIT-FIN")
                {
                    dt.templatePath = Server.MapPath("~/Content/tasks/templateMitsui.docx");
                    dt.outputPath = Server.MapPath("~/Temp/UpdatedDocumentMitsui.docx");
                    string bookmarkName = "HasilTable";
                    string strconnection = OwinLibrary.GetAPIMitsu();
                    List<Response> rs = await dataInv.GetdataInvoice(strconnection, dt.StartDate, dt.EndDate, dt.cabang);
                    var listOfString = rs.Select((r, Index) => new List<string>
                     {
                        (Index + 1).ToString(),
                        r.Tgl_Pengajuan,
                        r.NamaPT_Ceking,
                        r.cabang,
                        r.TipeTransaksi_CekingDesc == "Data Terakhir" ? "Rp. 50.000,00" : "Rp. 500.000,00",
                        "Rp. 20.000,00"
                     }).ToList();
                    var NotransUpdateINV = rs.Select(r => new List<string>
                     {
                        r.NoTransaksi,
                     }).ToList();
                    InvoiceInput mod = new InvoiceInput();
                    mod.invoice = dt.invoice;
                    mod.tglinv = dt.tglinv;
                    mod.tgltmp = dt.tgltmp;
                    mod.pnbp = dt.pnbp;
                    mod.jasatot = dt.jasatot;
                    mod.periodetot = dt.periodetot;
                    mod.jpsatu = dt.jpsatu;
                    mod.jidua = dt.jidua;
                    mod.jdpp = dt.jdpp;
                    mod.jppn = dt.jppn;
                    mod.jpph = dt.jpph;
                    mod.jtot = dt.jtot;
                    mod.tanggalctts = dt.tanggalctts;
                    mod.templatePath = dt.templatePath;
                    mod.outputPath = dt.outputPath;
                    mod.terbilang = dt.terbilang;
                    GenProfide op = new GenProfide();
                    // Memanggil metode ReplacePlaceholdersAsync untuk mengganti placeholders dengan data yang diberikan
                    await op.ReplacePlaceholdersAsync(mod, bookmarkName, listOfString);
                    //await PopulateTableAtBookmarkAsync(dt.templatePath, dt.outputPath, bookmarkName, tableData);

                    // Menambahkan proses untuk mengunduh file setelah dokumen berhasil diubah
                    byte[] fileBytes = System.IO.File.ReadAllBytes(mod.outputPath);
                    string fileName = "UpdatedDocument.docx";
                    string tempWordPath = Path.Combine(Path.GetTempPath(), fileName);
                    System.IO.File.WriteAllBytes(tempWordPath, fileBytes);

                    // Load file Word ke Spire.Doc Document
                    Spire.Doc.Document document = new Spire.Doc.Document();
                    document.LoadFromFile(tempWordPath);

                    // Konversi ke PDF
                    string tempPdfPath = Path.Combine(Path.GetTempPath(), "ConvertedDocument.pdf");
                    document.SaveToFile(tempPdfPath, FileFormat.PDF);

                    // Ambil file PDF sebagai byte[] untuk return
                    byte[] pdfBytes = System.IO.File.ReadAllBytes(tempPdfPath);
                    string FileSave = Convert.ToBase64String(pdfBytes);
                    SavePengajuan savePengajuan = new SavePengajuan();
                    savePengajuan.KodeVoucher = mod.tglinv;
                    savePengajuan.PNBP = mod.tgltmp;
                    savePengajuan.Keterangan = mod.invoice;
                    savePengajuan.filed = FileSave;
                    savePengajuan.Status = "100";
                    savePengajuan.data_id = mod.jtot;
                    savePengajuan.mxy_no = mod.jpsatu;
                    savePengajuan.UserID = mod.jpph;
                    savePengajuan.GroupName = mod.jppn;
                    savePengajuan.Tglbeli = mod.jdpp;
                    savePengajuan.NoTransksi = log.ToString();
                    var apiResponse = await dataInv.UpdateInvoice(ds, strconnection, mod, savePengajuan, rs);
                    if (string.IsNullOrEmpty(apiResponse))
                    {
                        return Json(new { success = false, message = "Gagal Update Data" });
                    }
                    return Content(apiResponse, "application/json");

                }
                return RedirectToAction("auth_login_basic", "Auth");

            }
            catch (Exception ex)
            {
                return RedirectToAction("auth_login_basic", "Auth");
            }
        }



        //public async Task<ActionResult> UpdateInvoiceData([FromBody] List<List<string>> tableData, InvoiceInput dt, ParamSaveInvoice ds)
        //{
        //    try
        //    {
        //        MitsuiPengecekan nw = new MitsuiPengecekan();
        //        var xx = Session["DataLog"];
        //        var log = Session["Name"];
        //        if (xx == null || xx.ToString() == "")
        //        {
        //            return RedirectToAction("auth_login_basic", "Auth");
        //        }
        //        ViewBag.User = log;
        //        ViewBag.Data = xx;
        //        var DbCek = xx?.ToString();

        //        if (DbCek == "MTF-FIN")
        //        {
        //            dt.templatePath = Server.MapPath("~/Content/tasks/template.docx");
        //            dt.outputPath = Server.MapPath("~/Temp/UpdatedDocument.docx");
        //            string bookmarkName = "HasilTable";
        //            string strconnection = OwinLibrary.GetAPIMitsu();
        //            List<Response> rs = await nw.GetdataInvoice(strconnection, dt.StartDate, dt.EndDate, dt.cabang);
        //            var listOfString = rs.Select((r, Index) => new List<string>
        //             {
        //                (Index + 1).ToString(),
        //                r.Tgl_Pengajuan,
        //                r.NamaPT_Ceking,
        //                r.cabang,
        //                r.TipeTransaksi_CekingDesc == "Data Terakhir" ? "Rp. 50.000,00" : "Rp. 500.000,00",
        //                "Rp. 20.000,00"
        //             }).ToList();
        //            var NotransUpdateINV = rs.Select(r => new List<string>
        //             {
        //                r.NoTransaksi,
        //             }).ToList();
        //            InvoiceInput mod = new InvoiceInput();
        //            mod.invoice = dt.invoice;
        //            mod.tglinv = dt.tglinv;
        //            mod.tgltmp = dt.tgltmp;
        //            mod.pnbp = dt.pnbp;
        //            mod.jasatot = dt.jasatot;
        //            mod.periodetot = dt.periodetot;
        //            mod.jpsatu = dt.jpsatu;
        //            mod.jidua = dt.jidua;
        //            mod.jdpp = dt.jdpp;
        //            mod.jppn = dt.jppn;
        //            mod.jpph = dt.jpph;
        //            mod.jtot = dt.jtot;
        //            mod.tanggalctts = dt.tanggalctts;
        //            mod.templatePath = dt.templatePath;
        //            mod.outputPath = dt.outputPath;
        //            mod.terbilang = dt.terbilang;
        //            GenProfide op = new GenProfide();
        //            // Memanggil metode ReplacePlaceholdersAsync untuk mengganti placeholders dengan data yang diberikan
        //            await op.ReplacePlaceholdersAsync(mod, bookmarkName, tableData);
        //            //await PopulateTableAtBookmarkAsync(dt.templatePath, dt.outputPath, bookmarkName, tableData);

        //            // Menambahkan proses untuk mengunduh file setelah dokumen berhasil diubah
        //            byte[] fileBytes = System.IO.File.ReadAllBytes(mod.outputPath);
        //            string FileSave = Convert.ToBase64String(fileBytes);
        //            SavePengajuan savePengajuan = new SavePengajuan();
        //            var apiResponse = await nw.UpdateInvoice(ds, strconnection, mod, savePengajuan, NotransUpdateINV);
        //            if (string.IsNullOrEmpty(apiResponse))
        //            {
        //                return Json(new { success = false, message = "Gagal Update Data" });
        //            }
        //            return Content(apiResponse, "application/json");
        //        }
        //        else if (DbCek == "MIT-FIN")
        //        {
        //            dt.templatePath = Server.MapPath("~/Content/tasks/templateMitsui.docx");
        //            dt.outputPath = Server.MapPath("~/Temp/UpdatedDocumentMitsui.docx");
        //            string bookmarkName = "HasilTable";
        //            InvoiceInput mod = new InvoiceInput();
        //            mod.invoice = dt.invoice;
        //            mod.tglinv = dt.tglinv;
        //            mod.tgltmp = dt.tgltmp;
        //            mod.pnbp = dt.pnbp;
        //            mod.jasatot = dt.jasatot;
        //            mod.periodetot = dt.periodetot;
        //            mod.jpsatu = dt.jpsatu;
        //            mod.jidua = dt.jidua;
        //            mod.jdpp = dt.jdpp;
        //            mod.jppn = dt.jppn;
        //            mod.jpph = dt.jpph;
        //            mod.jtot = dt.jtot;
        //            mod.tanggalctts = dt.tanggalctts;
        //            mod.templatePath = dt.templatePath;
        //            mod.outputPath = dt.outputPath;
        //            mod.terbilang = dt.terbilang;
        //            GenProfide op = new GenProfide();
        //            // Memanggil metode ReplacePlaceholdersAsync untuk mengganti placeholders dengan data yang diberikan
        //            await op.ReplacePlaceholdersAsync(mod, bookmarkName, tableData);
        //            //await PopulateTableAtBookmarkAsync(dt.templatePath, dt.outputPath, bookmarkName, tableData);

        //            // Menambahkan proses untuk mengunduh file setelah dokumen berhasil diubah
        //            byte[] fileBytes = System.IO.File.ReadAllBytes(mod.outputPath);
        //            string FileSave = Convert.ToBase64String(fileBytes);
        //            string strconnection = OwinLibrary.GetAPIMitsu();
        //            SavePengajuan savePengajuan = new SavePengajuan();
        //            var apiResponse = await nw.UpdateInvoice(ds, strconnection, mod, savePengajuan);
        //            if (string.IsNullOrEmpty(apiResponse))
        //            {
        //                return Json(new { success = false, message = "Gagal Update Data" });
        //            }
        //            return Content(apiResponse, "application/json");
        //            // Mengirimkan file sebagai download

        //        }

        //        return RedirectToAction("auth_login_basic", "Auth");


        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { success = false, message = ex.Message });
        //    }
        //}


        public async Task<ActionResult> GenerateWordTable([FromBody] List<List<string>> tableData, InvoiceInput dt)
        {
            var xx = Session["DataLog"];
            var log = Session["Name"];
            if (xx == null || xx.ToString() == "")
            {
                return RedirectToAction("auth_login_basic", "Auth");
            }
            ViewBag.User = log;
            ViewBag.Data = xx;
            var DbCek = xx?.ToString();
            GetData nw = new GetData();
            MitsuiPengecekan dataInv = new MitsuiPengecekan();

            try
            {
                if (DbCek == "MTF-FIN")
                {
                    dt.templatePath = Server.MapPath("~/Content/tasks/template.docx");
                    dt.outputPath = Server.MapPath("~/Temp/UpdatedDocument.docx");
                    string strconnection = OwinLibrary.GetAPIMTF();
                    List<Response> rs = await dataInv.GetdataInvoice(strconnection, dt.StartDate, dt.EndDate, dt.cabang);
                    var listOfString = rs.Select((r, Index) => new List<string>
                     {
                        (Index + 1).ToString(),
                        r.Tgl_Pengajuan,
                        r.NamaPT_Ceking,
                        r.cabang,
                        r.TipeTransaksi_CekingDesc == "Data Terakhir" ? "Rp. 50.000,00" : "Rp. 500.000,00",
                        "Rp. 20.000,00"
                     }).ToList();

                    string bookmarkName = "HasilTable";
                    InvoiceInput mod = new InvoiceInput();
                    mod.invoice = dt.invoice;
                    mod.tglinv = dt.tglinv;
                    mod.tgltmp = dt.tgltmp;
                    mod.pnbp = dt.pnbp;
                    mod.jasatot = dt.jasatot;
                    mod.periodetot = dt.periodetot;
                    mod.jpsatu = dt.jpsatu;
                    mod.jidua = dt.jidua;
                    mod.jdpp = dt.jdpp;
                    mod.jppn = dt.jppn;
                    mod.jpph = dt.jpph;
                    mod.jtot = dt.jtot;
                    mod.tanggalctts = dt.tanggalctts;
                    mod.templatePath = dt.templatePath;
                    mod.outputPath = dt.outputPath;
                    mod.terbilang = dt.terbilang;
                    mod.cabang = dt.cabang;
                    mod.NoFaktur = dt.NoFaktur;
                    GenProfide op = new GenProfide();
                    // Memanggil metode ReplacePlaceholdersAsync untuk mengganti placeholders dengan data yang diberikan
                    await op.ReplacePlaceholdersAsync(mod, bookmarkName, listOfString);
                    //await PopulateTableAtBookmarkAsync(dt.templatePath, dt.outputPath, bookmarkName, tableData);

                    // Menambahkan proses untuk mengunduh file setelah dokumen berhasil diubah
                    byte[] fileBytes = System.IO.File.ReadAllBytes(mod.outputPath);
                    string fileName = "UpdatedDocument.docx";

                    // Mengirimkan file sebagai download
                    return File(fileBytes, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", fileName);

                }
                else if (DbCek == "MIT-FIN")
                {
                    dt.templatePath = Server.MapPath("~/Content/tasks/templateMitsui.docx");
                    dt.outputPath = Server.MapPath("~/Temp/UpdatedDocumentMitsui.docx");
                    string bookmarkName = "HasilTable";
                    string strconnection = OwinLibrary.GetAPIMitsu();
                    List<Response> rs = await dataInv.GetdataInvoice(strconnection, dt.StartDate, dt.EndDate, dt.cabang);
                    var listOfString = rs.Select((r, Index) => new List<string>
                     {
                        (Index + 1).ToString(),
                        r.Tgl_Pengajuan,
                        r.NamaPT_Ceking,
                        r.cabang,
                        r.TipeTransaksi_CekingDesc == "Data Terakhir" ? "Rp. 50.000,00" : "Rp. 500.000,00",
                        "Rp. 20.000,00"
                     }).ToList();
                    InvoiceInput mod = new InvoiceInput();
                    mod.invoice = dt.invoice;
                    mod.tglinv = dt.tglinv;
                    mod.tgltmp = dt.tgltmp;
                    mod.pnbp = dt.pnbp;
                    mod.jasatot = dt.jasatot;
                    mod.periodetot = dt.periodetot;
                    mod.jpsatu = dt.jpsatu;
                    mod.jidua = dt.jidua;
                    mod.jdpp = dt.jdpp;
                    mod.jppn = dt.jppn;
                    mod.jpph = dt.jpph;
                    mod.jtot = dt.jtot;
                    mod.tanggalctts = dt.tanggalctts;
                    mod.templatePath = dt.templatePath;
                    mod.outputPath = dt.outputPath;
                    mod.terbilang = dt.terbilang;
                    mod.NoFaktur = dt.NoFaktur;
                    GenProfide op = new GenProfide();
                    // Memanggil metode ReplacePlaceholdersAsync untuk mengganti placeholders dengan data yang diberikan
                    await op.ReplacePlaceholdersAsync(mod, bookmarkName, listOfString);
                    //await PopulateTableAtBookmarkAsync(dt.templatePath, dt.outputPath, bookmarkName, tableData);

                    // Menambahkan proses untuk mengunduh file setelah dokumen berhasil diubah
                    byte[] fileBytes = System.IO.File.ReadAllBytes(mod.outputPath);
                    string fileName = "UpdatedDocument.docx";

                    // Mengirimkan file sebagai download
                    return File(fileBytes, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", fileName);
                }
                return RedirectToAction("auth_login_basic", "Auth");

            }
            catch (Exception ex)
            {
                return RedirectToAction("auth_login_basic", "Auth");
            }
        }
        public async Task<ActionResult> GenerateFilePdfNew([FromBody] List<List<string>> tableData, InvoiceInput dt)
        {
            var xx = Session["DataLog"];
            var log = Session["Name"];
            if (xx == null || xx.ToString() == "")
            {
                return RedirectToAction("auth_login_basic", "Auth");
            }
            ViewBag.User = log;
            ViewBag.Data = xx;
            var DbCek = xx?.ToString();
            GetData nw = new GetData();
            MitsuiPengecekan dataInv = new MitsuiPengecekan();

            try
            {
                if (DbCek == "MTF-FIN")
                {
                    dt.templatePath = Server.MapPath("~/Content/tasks/template.docx");
                    dt.outputPath = Server.MapPath("~/Temp/UpdatedDocumentMtf.docx");
                    string strconnection = OwinLibrary.GetAPIMTF();
                    List<Response> rs = await dataInv.GetdataInvoice(strconnection, dt.StartDate, dt.EndDate, dt.cabang);
                    var listOfString = rs.Select((r, Index) => new List<string>
                     {
                        (Index + 1).ToString(),
                        r.Tgl_Pengajuan,
                        r.NamaPT_Ceking,
                        r.cabang,
                        r.TipeTransaksi_CekingDesc == "Data Terakhir" ? "Rp. 50.000,00" : "Rp. 500.000,00",
                        "Rp. 20.000,00"
                     }).ToList();

                    string bookmarkName = "HasilTable";
                    InvoiceInput mod = new InvoiceInput();
                    mod.invoice = dt.invoice;
                    mod.tglinv = dt.tglinv;
                    mod.tgltmp = dt.tgltmp;
                    mod.pnbp = dt.pnbp;
                    mod.jasatot = dt.jasatot;
                    mod.periodetot = dt.periodetot;
                    mod.jpsatu = dt.jpsatu;
                    mod.jidua = dt.jidua;
                    mod.jdpp = dt.jdpp;
                    mod.jppn = dt.jppn;
                    mod.jpph = dt.jpph;
                    mod.jtot = dt.jtot;
                    mod.tanggalctts = dt.tanggalctts;
                    mod.templatePath = dt.templatePath;
                    mod.outputPath = dt.outputPath;
                    mod.terbilang = dt.terbilang;
                    mod.cabang = dt.cabang;
                    mod.NoFaktur = dt.NoFaktur;
                    GenProfide op = new GenProfide();
                    // Memanggil metode ReplacePlaceholdersAsync untuk mengganti placeholders dengan data yang diberikan
                    await op.ReplacePlaceholdersAsync(mod, bookmarkName, listOfString);
                    //await PopulateTableAtBookmarkAsync(dt.templatePath, dt.outputPath, bookmarkName, tableData);

                    // Menambahkan proses untuk mengunduh file setelah dokumen berhasil diubah
                    byte[] fileBytes = System.IO.File.ReadAllBytes(mod.outputPath);
                    string fileName = "UpdatedDocumentMtf.docx";
                    string tempWordPath = Path.Combine(Path.GetTempPath(), fileName);
                    System.IO.File.WriteAllBytes(tempWordPath, fileBytes);

                    // Load file Word ke Spire.Doc Document
                    Spire.Doc.Document document = new Spire.Doc.Document();
                    document.LoadFromFile(tempWordPath);

                    // Konversi ke PDF
                    string tempPdfPath = Path.Combine(Path.GetTempPath(), "ConvertedDocument.pdf");
                    document.SaveToFile(tempPdfPath, FileFormat.PDF);

                    // Ambil file PDF sebagai byte[] untuk return
                    byte[] pdfBytes = System.IO.File.ReadAllBytes(tempPdfPath);

                    // Hapus file sementara
                    System.IO.File.Delete(tempWordPath);
                    System.IO.File.Delete(tempPdfPath);

                    // Return file PDF ke browser
                    return File(pdfBytes, "application/pdf", "ConvertedDocument.pdf");

                    // Mengirimkan file sebagai download
                    //return File(fileBytes, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", fileName);

                }
                else if (DbCek == "MIT-FIN")
                {
                    dt.templatePath = Server.MapPath("~/Content/tasks/templateMitsui.docx");
                    dt.outputPath = Server.MapPath("~/Temp/UpdatedDocumentMitsui.docx");
                    string bookmarkName = "HasilTable";
                    string strconnection = OwinLibrary.GetAPIMitsu();
                    List<Response> rs = await dataInv.GetdataInvoice(strconnection, dt.StartDate, dt.EndDate, dt.cabang);
                    var listOfString = rs.Select((r, Index) => new List<string>
                     {
                        (Index + 1).ToString(),
                        r.Tgl_Pengajuan,
                        r.NamaPT_Ceking,
                        r.cabang,
                        r.TipeTransaksi_CekingDesc == "Data Terakhir" ? "Rp. 50.000,00" : "Rp. 500.000,00",
                        "Rp. 20.000,00"
                     }).ToList();
                    InvoiceInput mod = new InvoiceInput();
                    mod.invoice = dt.invoice;
                    mod.tglinv = dt.tglinv;
                    mod.tgltmp = dt.tgltmp;
                    mod.pnbp = dt.pnbp;
                    mod.jasatot = dt.jasatot;
                    mod.periodetot = dt.periodetot;
                    mod.jpsatu = dt.jpsatu;
                    mod.jidua = dt.jidua;
                    mod.jdpp = dt.jdpp;
                    mod.jppn = dt.jppn;
                    mod.jpph = dt.jpph;
                    mod.jtot = dt.jtot;
                    mod.tanggalctts = dt.tanggalctts;
                    mod.templatePath = dt.templatePath;
                    mod.outputPath = dt.outputPath;
                    mod.terbilang = dt.terbilang;
                    mod.NoFaktur = dt.NoFaktur;
                    GenProfide op = new GenProfide();
                    // Memanggil metode ReplacePlaceholdersAsync untuk mengganti placeholders dengan data yang diberikan
                    await op.ReplacePlaceholdersAsync(mod, bookmarkName, listOfString);
                    //await PopulateTableAtBookmarkAsync(dt.templatePath, dt.outputPath, bookmarkName, tableData);

                    // Menambahkan proses untuk mengunduh file setelah dokumen berhasil diubah
                    byte[] fileBytes = System.IO.File.ReadAllBytes(mod.outputPath);
                    string fileName = "UpdatedDocumentMitsui.docx";
                    string tempWordPath = Path.Combine(Path.GetTempPath(), fileName);
                    System.IO.File.WriteAllBytes(tempWordPath, fileBytes);
                    //Spire.Doc.Document document = new Spire.Doc.Document();
                    //document.LoadFromFile(tempWordPath);
                    string tempPdfPath = Path.Combine(Path.GetTempPath(), "ConvertedDocument.pdf");
                    //document.SaveToFile(tempPdfPath, FileFormat.PDF);
                    //byte[] pdfBytes = System.IO.File.ReadAllBytes(tempPdfPath);
                    //System.IO.File.Delete(tempWordPath);
                    //System.IO.File.Delete(tempPdfPath);
                    //return File(pdfBytes, "application/pdf", "ConvertedDocument.pdf");
                    Application wordApp = new Application();
                    Microsoft.Office.Interop.Word.Document wordDoc = null;

                    try
                    {
                        wordDoc = wordApp.Documents.Open(tempWordPath);
                        wordDoc.ExportAsFixedFormat(tempPdfPath, WdExportFormat.wdExportFormatPDF);
                        byte[] pdfBytes = System.IO.File.ReadAllBytes(tempPdfPath);
                        return File(pdfBytes, "application/pdf", "ConvertedDocument.pdf");
                    }
                    catch (Exception ex)
                    {
                        return Content("Error Saat convert ke PDF : " + ex.Message);
                    }
                    finally
                    {
                        if (wordDoc != null)
                        {
                            wordDoc.Close(WdSaveOptions.wdDoNotSaveChanges);
                        }
                        wordApp.Quit();

                        // Hapus file sementara
                        if (System.IO.File.Exists(tempWordPath))
                            System.IO.File.Delete(tempWordPath);
                        if (System.IO.File.Exists(tempPdfPath))
                            System.IO.File.Delete(tempPdfPath);
                    }
                }
                return RedirectToAction("auth_login_basic", "Auth");

            }
            catch (Exception ex)
            {
                return RedirectToAction("auth_login_basic", "Auth");
            }
        }

        // Metode untuk mengganti placeholders di template Word dengan data yang sesuai

        public async Task<ActionResult> GenerateWordWithTableBookmark([FromBody] List<List<string>> tableData)
        {
            var xx = Session["DataLog"];
            var log = Session["Name"];
            if (xx == null || xx.ToString() == "")
            {
                return RedirectToAction("auth_login_basic", "Auth");
            }
            ViewBag.User = log;
            ViewBag.Data = xx;
            var DbCek = xx?.ToString();
            GetData nw = new GetData();
            try
            {
                if (DbCek == "MTF-FIN")
                {
                    string templatePath = Server.MapPath("~/Content/tasks/template.docx");
                    string outputPath = Server.MapPath("~/Temp/UpdatedDocumentMtf.docx");
                    string bookmarkName = "HasilTable";
                    GenProfide op = new GenProfide();
                    await op.PopulateTableAtBookmarkAsync(templatePath, outputPath, bookmarkName, tableData);

                    // Menambahkan proses untuk mengunduh file setelah dokumen berhasil diubah
                    byte[] fileBytes = System.IO.File.ReadAllBytes(outputPath);
                    string fileName = "UpdatedDocumentMtf.docx";

                    // Mengirimkan file sebagai download
                    return File(fileBytes, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", fileName);
                }
                else if (DbCek == "MIT-FIN")
                {
                    string templatePath = Server.MapPath("~/Content/tasks/templateMitsui.docx");
                    string outputPath = Server.MapPath("~/Temp/UpdatedDocumentMitsui.docx");
                    string bookmarkName = "HasilTable";
                    GenProfide op = new GenProfide();
                    await op.PopulateTableAtBookmarkAsync(templatePath, outputPath, bookmarkName, tableData);

                    // Menambahkan proses untuk mengunduh file setelah dokumen berhasil diubah
                    byte[] fileBytes = System.IO.File.ReadAllBytes(outputPath);
                    string fileName = "UpdatedDocumentMitsui.docx";

                    // Mengirimkan file sebagai download
                    return File(fileBytes, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", fileName);
                }

                return RedirectToAction("auth_login_basic", "Auth");
            }
            catch (Exception ex)
            {
                return RedirectToAction("auth_login_basic", "Auth");
            }
        }

        public async Task<ActionResult> AddTableDataToExcel(string Startdate, string EndDate, string jenisCek, string SumberData)
        {
            var xx = Session["DataLog"];
            var log = Session["Name"];
            if (xx == null || xx.ToString() == "")
            {
                return RedirectToAction("auth_login_basic", "Auth");
            }
            ViewBag.User = log;
            ViewBag.Data = xx;
            var DbCek = xx?.ToString();
            GetData nw = new GetData();

            string filePath = Server.MapPath("~/Content/tasks/ReportExcel.xlsx");
            string outputPath = Server.MapPath("~/Temp/ReportDocument.xlsx");
            string sheetName = "Sheet1";

            ParamGetPengajuan mod = new ParamGetPengajuan();
            mod.Status = "";
            mod.UserID = "";
            mod.GroupName = "";
            mod.Tipe = "";
            mod.endDate = "";
            mod.startDate = "";
            List<Response> dt = new List<Response>();
            GetData op = new GetData();
            if (DbCek == "ADMIN")
            {
                if (SumberData == "")
                {
                    ViewBag.Error = "Pilih Rekanan";
                    return View("Laporan");
                }
                else if (SumberData == "MTF")
                {
                    string strconnection = OwinLibrary.GetAPIMTF();
                    dt = await op.GetDataPengecekanIn(mod, strconnection);
                }
                else if (SumberData == "MIT")
                {
                    string strconnection = OwinLibrary.GetAPIMitsu();
                    dt = await op.GetDataPengecekanIn(mod, strconnection);
                } else {
                    ViewBag.Error = "Data Tidak Tersedia";
                    return View("Laporan");

                }
            }
            else
            {
                return RedirectToAction("auth_login_basic", "Auth");
            }
            
            try
            {
                ExcelHelper helper = new ExcelHelper();
                if (string.IsNullOrEmpty(Startdate) && string.IsNullOrEmpty(EndDate) && string.IsNullOrEmpty(jenisCek))
                {
                    await helper.AddTableDataToExcel(filePath, sheetName, dt, outputPath);
                }
                else if (string.IsNullOrEmpty(jenisCek))
                {
                    DateTime startDate = DateTime.Parse(Startdate);
                    DateTime endDate = DateTime.Parse(EndDate);

                    var filteredData = dt.Where(item =>
                    DateTime.TryParse(item.Tgl_Update, out DateTime tglPengajuan) && // Memastikan Tgl_Pengajuan valid
                    tglPengajuan >= startDate && tglPengajuan <= endDate).ToList();
                    await helper.AddTableDataToExcel(filePath, sheetName, filteredData, outputPath);

                }
                else if (string.IsNullOrEmpty(Startdate) && string.IsNullOrEmpty(EndDate))
                {
                    var filteredData = dt.Where(item =>
                    item.Status_Pengajuan == jenisCek
                    ).ToList();
                    await helper.AddTableDataToExcel(filePath, sheetName, filteredData, outputPath);
                }
                else
                {
                    DateTime startDate = DateTime.Parse(Startdate);
                    DateTime endDate = DateTime.Parse(EndDate);

                    var filteredData = dt.Where(item =>
                    DateTime.TryParse(item.Tgl_Update, out DateTime tglPengajuan) && // Memastikan Tgl_Pengajuan valid
                    tglPengajuan >= startDate && tglPengajuan <= endDate && item.Status_Pengajuan == jenisCek).ToList();
                    await helper.AddTableDataToExcel(filePath, sheetName, filteredData, outputPath);
                }
                byte[] fileBytes = System.IO.File.ReadAllBytes(outputPath);
                string fileName = "ReportDocument.xlsx";

                // Mengirimkan file sebagai download


                Response.Cookies["fileDownload"].Value = "true";
                Response.Cookies["fileDownload"].Path = "/";
                Response.Cookies["fileDownload"].Expires = DateTime.Now.AddMinutes(5); // Opsional, masa berlaku cookie

                return File(fileBytes, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", fileName);
            }
            catch (Exception ex)
            {
                ViewBag.Message = $"Terjadi kesalahan: {ex.Message}";
            }

            return View();
        }
        public async Task<ActionResult> GenerateFilePdf([FromBody] List<List<string>> tableData, InvoiceInput dt)
        {
            var xx = Session["DataLog"];
            var log = Session["Name"];
            if (xx == null || xx.ToString() == "")
            {
                return RedirectToAction("auth_login_basic", "Auth");
            }

            ViewBag.User = log;
            ViewBag.Data = xx;
            var DbCek = xx?.ToString();
            GetData nw = new GetData();
            MitsuiPengecekan dataInv = new MitsuiPengecekan();

            try
            {
                string templatePath;
                string outputPath;
                string bookmarkName = "HasilTable";
                string tempPdfPath = Path.Combine(Path.GetTempPath(), "ConvertedDocument.pdf");

                if (DbCek == "MTF-FIN")
                {
                    templatePath = Server.MapPath("~/Content/tasks/template.docx");
                    outputPath = Server.MapPath("~/Temp/UpdatedDocumentMtf.docx");
                }
                else if (DbCek == "MIT-FIN")
                {
                    templatePath = Server.MapPath("~/Content/tasks/templateMitsui.docx");
                    outputPath = Server.MapPath("~/Temp/UpdatedDocumentMitsui.docx");
                }
                else
                {
                    return RedirectToAction("auth_login_basic", "Auth");
                }

                dt.templatePath = templatePath;
                dt.outputPath = outputPath;

                string strconnection = DbCek == "MTF-FIN" ? OwinLibrary.GetAPIMTF() : OwinLibrary.GetAPIMitsu();
                List<Response> rs = await dataInv.GetdataInvoice(strconnection, dt.StartDate, dt.EndDate, dt.cabang);

                var listOfString = rs.Select((r, Index) => new List<string>
                {
                    (Index + 1).ToString(),
                    r.Tgl_Pengajuan,
                    r.NamaPT_Ceking,
                    r.cabang,
                    r.TipeTransaksi_CekingDesc == "Data Terakhir" ? "Rp. 50.000,00" : "Rp. 500.000,00",
                    "Rp. 20.000,00"
                }).ToList();

                GenProfide op = new GenProfide();
                await op.ReplacePlaceholdersAsync(dt, bookmarkName, listOfString);

                byte[] fileBytes = System.IO.File.ReadAllBytes(outputPath);
                string tempWordPath = Path.Combine(Path.GetTempPath(), Path.GetFileName(outputPath));
                using (FileStream fs = new FileStream(tempWordPath, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(fileBytes, 0, fileBytes.Length);
                }
                WaitForFile(tempWordPath);
                // Tunggu jika file terkunci
                //System.Threading.Thread.Sleep(100);

                //if (IsFileLocked(tempWordPath))
                //{
                //    return Content("File sedang digunakan oleh proses lain, coba lagi nanti.");
                //}

                // Proses konversi Word ke PDF
                Application wordApp = new Application();
                Microsoft.Office.Interop.Word.Document wordDoc = null;

                try
                {
                    wordDoc = wordApp.Documents.Open(tempWordPath);
                    wordDoc.ExportAsFixedFormat(tempPdfPath, WdExportFormat.wdExportFormatPDF);

                    // Ambil file PDF untuk dikembalikan
                    byte[] pdfBytes = System.IO.File.ReadAllBytes(tempPdfPath);
                    return File(pdfBytes, "application/pdf", "ConvertedDocument.pdf");
                }
                catch (Exception ex)
                {
                    return Content("Error saat convert ke PDF: " + ex.Message);
                }
                finally
                {
                    if (wordDoc != null)
                    {
                        wordDoc.Close(WdSaveOptions.wdDoNotSaveChanges);
                    }
                    wordApp.Quit();

                    // Hapus file sementara
                    if (System.IO.File.Exists(tempWordPath))
                        System.IO.File.Delete(tempWordPath);
                    if (System.IO.File.Exists(tempPdfPath))
                        System.IO.File.Delete(tempPdfPath);
                }
            }
            catch (Exception ex)
            {
                return Content("Terjadi kesalahan: " + ex.Message);
            }
        }

        public async Task<ActionResult> SaveDBInvoiceData([FromBody] List<List<string>> tableData, InvoiceInput dt, ParamSaveInvoice ds)
        {
            var xx = Session["DataLog"];
            var log = Session["Name"];
            if (string.IsNullOrEmpty(xx?.ToString()))
            {
                return RedirectToAction("auth_login_basic", "Auth");
            }

            ViewBag.User = log;
            ViewBag.Data = xx;

            string dbCheck = xx.ToString();
            GetData dataService = new GetData();
            MitsuiPengecekan invoiceProcessor = new MitsuiPengecekan();

            try
            {
                // Tentukan konfigurasi berdasarkan nilai DbCek
                string templatePath = string.Empty;
                string outputPath = string.Empty;
                string connectionString = string.Empty;

                if (dbCheck == "MTF-FIN")
                {
                    templatePath = Server.MapPath("~/Content/tasks/template.docx");
                    outputPath = Server.MapPath("~/Temp/UpdatedDocumentMtf.docx");
                    connectionString = OwinLibrary.GetAPIMTF();
                }
                else if (dbCheck == "MIT-FIN")
                {
                    templatePath = Server.MapPath("~/Content/tasks/templateMitsui.docx");
                    outputPath = Server.MapPath("~/Temp/UpdatedDocumentMitsui.docx");
                    connectionString = OwinLibrary.GetAPIMitsu();
                }
                else
                {
                    return RedirectToAction("auth_login_basic", "Auth");
                }

                // Ambil data invoice
                dt.templatePath = templatePath;
                dt.outputPath = outputPath;
                List<Response> invoiceData = await invoiceProcessor.GetdataInvoice(connectionString, dt.StartDate, dt.EndDate, dt.cabang);

                // Proses data untuk output
                var tableDataList = invoiceData.Select((r, index) => new List<string>
                {
                    (index + 1).ToString(),
                    r.Tgl_Pengajuan,
                    r.NamaPT_Ceking,
                    r.cabang,
                    r.TipeTransaksi_CekingDesc == "Data Terakhir" ? "Rp. 50.000,00" : "Rp. 500.000,00",
                    "Rp. 20.000,00"
                }).ToList();

                // Siapkan objek untuk pengisian data
                InvoiceInput mod = new InvoiceInput();
                mod.invoice = dt.invoice;
                mod.tglinv = dt.tglinv;
                mod.tgltmp = dt.tgltmp;
                mod.pnbp = dt.pnbp;
                mod.jasatot = dt.jasatot;
                mod.periodetot = dt.periodetot;
                mod.jpsatu = dt.jpsatu;
                mod.jidua = dt.jidua;
                mod.jdpp = dt.jdpp;
                mod.jppn = dt.jppn;
                mod.jpph = dt.jpph;
                mod.jtot = dt.jtot;
                mod.tanggalctts = dt.tanggalctts;
                mod.templatePath = templatePath;
                mod.outputPath = outputPath;
                mod.terbilang = dt.terbilang;
                mod.cabang = dt.cabang;
                mod.NoFaktur = dt.NoFaktur;
                string bookmarkName = "HasilTable";
                GenProfide profideGenerator = new GenProfide();

                // Ganti placeholders dalam template
                await profideGenerator.ReplacePlaceholdersAsync(mod, bookmarkName, tableDataList);

                // Konversi dokumen ke PDF
                byte[] pdfBytes = ConvertWordToPdf(outputPath);
                if (pdfBytes == null)
                {
                    return Content("Error saat convert ke PDF.");
                }

                // Siapkan data untuk penyimpanan
                SavePengajuan saveRequest = new SavePengajuan
                {
                    KodeVoucher = dt.tglinv,
                    PNBP = dt.tgltmp,
                    Keterangan = dt.invoice,
                    filed = Convert.ToBase64String(pdfBytes),
                    Status = "100",
                    data_id = dt.jtot,
                    mxy_no = dt.jpsatu,
                    UserID = dt.jpph,
                    GroupName = dt.jppn,
                    Tglbeli = dt.jdpp,
                    NoTransksi = log.ToString()
                };

                // Update data melalui API
                string apiResponse = await invoiceProcessor.UpdateInvoice(ds, connectionString, dt, saveRequest, invoiceData);

                if (string.IsNullOrEmpty(apiResponse))
                {
                    return Json(new { success = false, message = "Gagal Update Data" });
                }

                return Json(new { success = true, message = "Berhasil Di Simpan" });
            }
            catch (Exception ex)
            {
                // Tambahkan log jika perlu
                return RedirectToAction("auth_login_basic", "Auth");
            }
        }

        private byte[] ConvertWordToPdf(string outputPath)
        {
            // Tentukan path sementara untuk file Word dan PDF
            string tempWordPath = Path.Combine(Path.GetTempPath(), Path.GetFileName(outputPath));
            string tempPdfPath = Path.Combine(Path.GetTempPath(), "ConvertedDocument.pdf");

            try
            {
                byte[] fileBytes = System.IO.File.ReadAllBytes(outputPath);
                using (FileStream fs = new FileStream(tempWordPath, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(fileBytes, 0, fileBytes.Length);
                }
                WaitForFile(tempWordPath);
                // Tunggu jika file terkunci
                System.Threading.Thread.Sleep(100);

                if (IsFileLocked(tempWordPath))
                {
                    return null; // Jika file terkunci, kembalikan null
                }

                // Proses konversi Word ke PDF menggunakan Interop Word
                Application wordApp = new Application();
                Microsoft.Office.Interop.Word.Document wordDoc = null;
                try
                {
                    wordDoc = wordApp.Documents.Open(tempWordPath);
                    wordDoc.ExportAsFixedFormat(tempPdfPath, WdExportFormat.wdExportFormatPDF);

                    // Kembalikan byte[] dari file PDF yang dihasilkan
                    return System.IO.File.ReadAllBytes(tempPdfPath);
                }
                catch (Exception ex)
                {
                    return null; // Jika terjadi error, kembalikan null
                }
                finally
                {
                    if (wordDoc != null)
                    {
                        wordDoc.Close(WdSaveOptions.wdDoNotSaveChanges);
                    }
                    wordApp.Quit();

                    // Hapus file sementara
                    if (System.IO.File.Exists(tempWordPath))
                        System.IO.File.Delete(tempWordPath);
                    if (System.IO.File.Exists(tempPdfPath))
                        System.IO.File.Delete(tempPdfPath);
                }
            }
            catch (Exception)
            {
                return null; // Jika terjadi kesalahan saat menyalin atau mengonversi, kembalikan null
            }
        }
        private static bool IsFileLocked(string filePath)
        {
            try
            {
                using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                {
                    stream.Close();
                }
                return false;
            }
            catch (IOException)
            {
                return true;
            }
        }

        private static void WaitForFile(string filePath)
        {
            while (IsFileLocked(filePath))
            {
                System.Threading.Thread.Sleep(100); // Tunggu 100ms sebelum mengecek lagi
            }
        }

    }

}