using ProfideSedayuOp.Models.Mitsui;
using ProfideSedayuOp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data;
using DocumentFormat.OpenXml.Presentation;
using ProfideSedayuOp.Models.Helper;
using Newtonsoft.Json;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using System.IO;
using DocumentFormat.OpenXml.Office2019.Drawing.SVG;
using DocumentFormat.OpenXml.Wordprocessing;

namespace ProfideSedayuOp.Controllers
{
    public class AuthController : Controller
    {

        // GET: Auth
        //public ActionResult auth_forgot_password_basic()
        //{
        //    return View();
        //}
        //public ActionResult auth_forgot_password_cover()
        //{
        //    return View();
        //}
        public ActionResult auth_login_basic()
        {
            return View();
        }
        //public ActionResult auth_login_basic()
        //{
        //    return View();
        //}
        //public ActionResult auth_register_basic()
        //{
        //    return View();
        //}
        //public ActionResult auth_register_cover()
        //{
        //    return View();
        //}
        //public ActionResult auth_register_multisteps()
        //{
        //    return View();
        //}
        //public ActionResult auth_reset_password_basic()
        //{
        //    return View();
        //}
        //public ActionResult auth_reset_password_cover()
        //{
        //    return View();
        //}
        //public ActionResult auth_two_steps_basic()
        //{
        //    return View();
        //}
        //public ActionResult auth_two_steps_cover()
        //{
        //    return View();
        //}
        //public ActionResult auth_verify_email_basic()
        //{
        //    return View();
        //}
        //public ActionResult auth_verify_email_cover()
        //{
        //    return View();
        //}
        public async Task<ActionResult> AccountLoginAPI(string username, string password)
        {
            try
            {
                MitsuiPengecekan gd = new MitsuiPengecekan();
                string strconnection = OwinLibrary.GetAPIMitsu();

                var getToken = await gd.GetAccessTokenAsync(strconnection);
                if (string.IsNullOrEmpty(getToken))
                {
                    ViewBag.Error = "Gagal Autentifikasi End Point";
                    return View("auth_login_basic");
                }
                var AuthGetData = await gd.Login(password, username, getToken, strconnection);
                if (string.IsNullOrEmpty(AuthGetData)) 
                {
                    ViewBag.Error = "Data Yang Di Inputkan Tidak Vali";
                    return View("auth_login_basic");
                }
                var responseList = JsonConvert.DeserializeObject<List<LginRespon>>(AuthGetData);

                if (responseList != null && responseList.Count > 0)
                {
                    GetData nw = new GetData();
                    ParamGetPengajuan dbs = new ParamGetPengajuan();
                    var Acces = responseList[0].accestipe;
                    if (Acces != null && responseList.Count > 0)
                    {
                        Session["DataLog"] = Acces;
                        Session["Name"] = responseList[0].Nama;
                        Session["Admin"] = responseList[0].accesenum;
                        if (Acces == "MTF-FIN")
                        {
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
                            if (Acces.ToString() == "MTF-FIN")
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
                                if (Acces.ToString() == "MTF-FIN")
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

                        }
                        else if (Acces == "MIT-FIN")
                        {
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
                            if (Acces.ToString() == "MIT-FIN")
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
                                if (Acces.ToString() == "MIT-FIN")
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

                        }
                        else if (Acces == "ADMIN")
                        {
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
                            if (Acces.ToString() == "ADMIN")
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
                                if (Acces.ToString() == "ADMIN")
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
                    } else
                    {
                        ViewBag.Error = "Anda Tidak Memiliki Akses";
                        return View("auth_login_basic");
                    }
                } else
                {
                    ViewBag.Error = "Anda Tidak Memiliki Akses";
                    return View("auth_login_basic");
                }
                ViewBag.Error = "Data Yang Di Inputkan Tidak Valid";
                return View("auth_login_basic");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("auth_login_basic");
            }
        }
        public class LginRespon
        {
            public string Status { get; set; }
            public string Role { get; set; }
            public string Nama { get; set; }
            public string email { get; set; }
            public string updated_at { get; set; }
            public string acceslink { get; set; }
            public string accestipe { get; set; }
            public string accesenum { get; set; }
        }
        public async Task<ActionResult> AccountLoginAppNew(ParamLogin param)
        {
            try
            {
                // Inisialisasi data dan helper
                AuthGetDt data = new AuthGetDt();
                ExcdataHelper to = new ExcdataHelper();
                DataTable dt = await data.dbGetDataLogin(param);

                // Validasi jika data tidak ditemukan
                if (dt == null || dt.Rows.Count <= 0)
                {
                    ViewBag.Error = "Data tidak ditemukan. Periksa kembali data Anda.";
                    return View("auth_login_basic"); // Ganti dengan nama View Auth Login Anda
                }

                // Set session data
                var Acces = dt.Rows[0][0];
                var Name = dt.Rows[0][1];
                Session["DataLog"] = Acces;
                Session["Name"] = Name;

                // Inisialisasi parameter pengajuan dan koneksi API
                ParamGetPengajuan dbs = new ParamGetPengajuan();
                GetData nw = new GetData();
                string strconnection = Acces.ToString() == "MTF-FIN" ? OwinLibrary.GetAPIMTF() : OwinLibrary.GetAPIMitsu();

                // Proses data untuk masing-masing akses
                if (Acces.ToString() == "MTF-FIN" || Acces.ToString() == "MIT-FIN")
                {
                    // Path JSON
                    string jsonFilePath = Server.MapPath("~/Content/assets/json/BackData.json");

                    // Ambil data dari API
                    var apiResponse = await nw.GetDataPengecekanAPI(dbs, strconnection);
                    if (string.IsNullOrEmpty(apiResponse))
                    {
                        return Json(new { success = false, message = "Response kosong dari API" });
                    }

                    // Serialize dan simpan data ke file JSON
                    var responseObject = JsonConvert.DeserializeObject<object>(apiResponse);
                    string jsonContent = JsonConvert.SerializeObject(responseObject, Formatting.Indented);
                    System.IO.File.WriteAllText(jsonFilePath, jsonContent);

                    // Proses tambahan jika status = "40"
                     await ProcessAdditionalData(nw, strconnection, Acces.ToString());
                }

                // Jika akses tidak valid
                ViewBag.Error = "Data tidak ditemukan. Periksa kembali data Anda.";
                return View("auth_login_basic");
            }
            catch (Exception ex)
            {
                // Tangani kesalahan umum
                ViewBag.Error = "Terjadi Kesalahan Coba Lagi";
                return View("auth_login_basic");
            }
        }

        private async Task ProcessAdditionalData(GetData nw, string strconnection, string accesType)
        {
            // Path JSON
            string jsonFilePath2 = Server.MapPath("~/Content/assets/json/BackDataDone.json");

            // Ambil data tambahan untuk status "40"
            ParamGetPengajuan dbo = new ParamGetPengajuan { Status = "40" };
            var apiResponse2 = await nw.GetDataPengecekanAPI(dbo, strconnection);
            if (string.IsNullOrEmpty(apiResponse2))
            {
                throw new Exception("Response kosong dari API");
            }

            // Serialize dan simpan data ke file JSON
            var responseObject2 = JsonConvert.DeserializeObject<object>(apiResponse2);
            string jsonContent2 = JsonConvert.SerializeObject(responseObject2, Formatting.Indented);
            System.IO.File.WriteAllText(jsonFilePath2, jsonContent2);

            // Proses tambahan untuk tipe "002"
            if (accesType == "MTF-FIN" || accesType == "MIT-FIN")
            {
                string jsonFilePathInvoice = Server.MapPath("~/Content/assets/json/BackDataInv.json");
                ParamGetPengajuan NewForINVdbo = new ParamGetPengajuan { Tipe = "002" };
                var responseDataInv = await nw.GetDataPengecekanAPI(NewForINVdbo, strconnection);

                if (string.IsNullOrEmpty(responseDataInv))
                {
                    throw new Exception("Response kosong dari API");
                }

                var responseDataInvObject = JsonConvert.DeserializeObject<object>(responseDataInv);
                string responseDataInvContent = JsonConvert.SerializeObject(responseDataInvObject, Formatting.Indented);
                System.IO.File.WriteAllText(jsonFilePathInvoice, responseDataInvContent);
            }
        }


        public async Task<ActionResult> AccountLoginApp(ParamLogin param)
        {

            try
            {
                AuthGetDt data = new AuthGetDt();
                ExcdataHelper to = new ExcdataHelper();
                var dt = new DataTable();
                dt = await data.dbGetDataLogin(param);
                if (dt == null || dt.Rows.Count <= 0)
                {
                    // Jika data kosong, kirim pesan error
                    ViewBag.Error = "Data tidak ditemukan. Periksa kembali data Anda.";
                    return View("auth_login_basic"); // Ganti dengan nama View Auth Login Anda
                }
                
                var jsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(dt);
                var Acces = dt.Rows[0][0];
                var Name = dt.Rows[0][1];
                var type = dt.Rows[0][2];
                Session["DataLog"] = Acces;
                Session["Name"] = Name;

                ParamGetPengajuan dbs = new ParamGetPengajuan();
                GetData nw = new GetData();
                if (Acces.ToString() == "MTF-FIN")
                {
                    string jsonFilePath = Server.MapPath("~/Content/assets/json/BackData.json");
                    string strconnection = OwinLibrary.GetAPIMTF();
                    var apiResponse = await nw.GetDataPengecekanAPI(dbs, strconnection);
                    if (string.IsNullOrEmpty(apiResponse))
                    {
                        return Json(new { success = false, message = "Response kosong dari API" });
                    }
                    var responseObject = JsonConvert.DeserializeObject<object>(apiResponse);
                    string jsonContent = JsonConvert.SerializeObject(responseObject, Formatting.Indented);
                    System.IO.File.WriteAllText(jsonFilePath, jsonContent);
                    if (Acces.ToString() == "MTF-FIN")
                    {
                        string jsonFilePath2 = Server.MapPath("~/Content/assets/json/BackDataDone.json");
                        ParamGetPengajuan dbo = new ParamGetPengajuan();
                        dbo.Status = "40";
                        var apiResponse2 = await nw.GetDataPengecekanAPI(dbo, strconnection);
                        if (string.IsNullOrEmpty(apiResponse))
                        {
                            return Json(new { success = false, message = "Response kosong dari API" });
                        }
                        var responseObject2 = JsonConvert.DeserializeObject<object>(apiResponse2);
                        string jsonContent2 = JsonConvert.SerializeObject(responseObject2, Formatting.Indented);
                        System.IO.File.WriteAllText(jsonFilePath2, jsonContent2);
                        if (Acces.ToString() == "MTF-FIN")
                        {
                            string jsonFilePathInvoice = Server.MapPath("~/Content/assets/json/BackDataInv.json");
                            ParamGetPengajuan NewForINVdbo = new ParamGetPengajuan();
                            NewForINVdbo.Tipe = "002";
                            var ResponseDataInv = await nw.GetDataPengecekanAPI(NewForINVdbo, strconnection);
                            if (string.IsNullOrEmpty(ResponseDataInv))
                            {
                                return Json(new { success = false, message = "Response kosong dari API" });
                            }
                            var ResponseDataInvObject = JsonConvert.DeserializeObject<object>(ResponseDataInv);
                            string ResponseDataInvContent = JsonConvert.SerializeObject(ResponseDataInvObject, Formatting.Indented);
                            System.IO.File.WriteAllText(jsonFilePathInvoice, ResponseDataInvContent);
                            return RedirectToAction("Index", "Home");
                        }
                    }
                }
                else if (Acces.ToString() == "MIT-FIN")
                {
                    string jsonFilePath = Server.MapPath("~/Content/assets/json/BackData.json");
                    string strconnection = OwinLibrary.GetAPIMitsu();
                    var apiResponse = await nw.GetDataPengecekanAPI(dbs, strconnection);
                    if (string.IsNullOrEmpty(apiResponse))
                    {
                        return Json(new { success = false, message = "Response kosong dari API" });
                    }
                    var responseObject = JsonConvert.DeserializeObject<object>(apiResponse);
                    string jsonContent = JsonConvert.SerializeObject(responseObject, Formatting.Indented);
                    System.IO.File.WriteAllText(jsonFilePath, jsonContent);
                    if (Acces.ToString() == "MIT-FIN")
                    {
                        string jsonFilePath2 = Server.MapPath("~/Content/assets/json/BackDataDone.json");
                        ParamGetPengajuan dbo = new ParamGetPengajuan();
                        dbo.Status = "40";
                        var apiResponse2 = await nw.GetDataPengecekanAPI(dbo, strconnection);
                        if (string.IsNullOrEmpty(apiResponse2))
                        {
                            return Json(new { success = false, message = "Response kosong dari API" });
                        }
                        var responseObject2 = JsonConvert.DeserializeObject<object>(apiResponse2);
                        string jsonContent2 = JsonConvert.SerializeObject(responseObject2, Formatting.Indented);
                        System.IO.File.WriteAllText(jsonFilePath2, jsonContent2);
                        if (Acces.ToString() == "MIT-FIN")
                        {
                            string jsonFilePathInvoice = Server.MapPath("~/Content/assets/json/BackDataInv.json");
                            ParamGetPengajuan NewForINVdbo = new ParamGetPengajuan();
                            NewForINVdbo.Tipe = "002";
                            var ResponseDataInv = await nw.GetDataPengecekanAPI(NewForINVdbo, strconnection);
                            if (string.IsNullOrEmpty(ResponseDataInv))
                            {
                                return Json(new { success = false, message = "Response kosong dari API" });
                            }
                            var ResponseDataInvObject = JsonConvert.DeserializeObject<object>(ResponseDataInv);
                            string ResponseDataInvContent = JsonConvert.SerializeObject(ResponseDataInvObject, Formatting.Indented);
                            System.IO.File.WriteAllText(jsonFilePathInvoice, ResponseDataInvContent);
                            return RedirectToAction("Index", "Home");
                        }
                    }
                }
                ViewBag.Error = "Data tidak ditemukan. Periksa kembali data Anda.";
                return View("auth_login_basic");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Terjadi Kesalahan Coba Lagi";
                return View("auth_login_basic");
            }
        }
    }
    public class dbgetDateLo
    {
        public string Name { get; set; }
        public string Acces { get; set; }
    }
}