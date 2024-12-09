using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using ProfideSedayuOp.Models.Helper;
using ProfideSedayuOp.Models.Mitsui;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net;
using System.Text;
using static System.Net.WebRequestMethods;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using DocumentFormat.OpenXml.Drawing;
using System.Web.Mvc;
using System.Windows.Interop;

namespace ProfideSedayuOp.Models
{
    public class MitsuiPengecekan
    {
        public async Task<DataTable> dbGetDataPengecekan(ParamGetData param)
        {

            DataTable dt;
            ExcdataHelper dbaccess = new ExcdataHelper();
            string strconnection = OwinLibrary.GetDB();

            SqlParameter[] sqlParam =
             {
                new SqlParameter ("@User_Pengaju", param.User_Pengaju?? ""),
                new SqlParameter ("@User_Group", param.User_Group ?? ""),
                new SqlParameter ("@tipe", param.tipe ?? ""),
                new SqlParameter ("@status", param.status ?? ""),
                new SqlParameter ("@startDate", param.startDate ?? ""),
                new SqlParameter ("@endDate", param.endDate ?? ""),
            };

            dt = await dbaccess.ExecuteDataTable(strconnection, "udp_trx_pengajuanpt_get", sqlParam);

            return dt;
        }


        public async Task<string> GetAccessTokenAsync(string strconnection)
        {
            string grantType = OwinLibrary.APIParam();
            string username = OwinLibrary.APIKey();
            try
            {
                using (var client = new HttpClient())
                {
                    var requestBody = new Dictionary<string, string>
                    {
                        { "grant_type", grantType },
                        { "username", username }
                    };
                    var content = new FormUrlEncodedContent(requestBody);
                    var response = await client.PostAsync(strconnection + "Token", content);

                    if (response.IsSuccessStatusCode)
                    {
                        var jsonResponse = await response.Content.ReadAsStringAsync();
                        var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(jsonResponse);
                        return tokenResponse?.access_token;
                    }
                    else
                    {
                        // Logging error response
                        var errorResponse = await response.Content.ReadAsStringAsync();
                        LogError("Error while getting token: " + errorResponse);
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                LogError("Exception in GetAccessTokenAsync: " + ex.Message);
                return null;
            }
        }

        public async Task<string> getPengajuan(ParamGetPengajuan dt, string strconnection)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", dt.accesToken);

                    var formData = new MultipartFormDataContent
                    {
                        { new StringContent(dt.UserID ?? ""), "UserID" },
                        { new StringContent(dt.GroupName?? ""), "GroupName" },
                        { new StringContent(dt.Status ?? ""), "Status" },
                        { new StringContent(dt.Tipe ?? ""), "Tipe" },
                        { new StringContent(dt.startDate ?? ""), "startDate" },
                        { new StringContent(dt.endDate ?? ""), "endDate" }
                    };

                    var response = await client.PostAsync(strconnection + "api/AHU/GetPengajuanChecking", formData);

                    if (response.IsSuccessStatusCode)
                    {

                        var jsonResponse = await response.Content.ReadAsStringAsync();
                        var formattedJson = JToken.Parse(jsonResponse).ToString(Formatting.Indented); // Format JSON
                        return formattedJson;
                    }
                    else
                    {
                        // Logging error response
                        var errorResponse = await response.Content.ReadAsStringAsync();
                        LogError("Error in CallSecondApiAsync: " + errorResponse);
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                LogError("Exception in CallSecondApiAsync: " + ex.Message);
                return null;
            }
        }



        public async Task<string> Login(string Password,string Email, string token,  string strconnection)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                    var formData = new MultipartFormDataContent
                    {
                        { new StringContent(Email), "Email" },
                        { new StringContent(Password), "Password" },
                    };

                    var response = await client.PostAsync(strconnection + "api/AHU/CheckAUTH", formData);

                    if (response.IsSuccessStatusCode)
                    {

                        var jsonResponse = await response.Content.ReadAsStringAsync();
                        var formattedJson = JToken.Parse(jsonResponse).ToString(Formatting.Indented); // Format JSON
                        return formattedJson;
                    }
                    else
                    {
                        // Logging error response
                        var errorResponse = await response.Content.ReadAsStringAsync();
                        LogError("Error in CallSecondApiAsync: " + errorResponse);
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                LogError("Exception in CallSecondApiAsync: " + ex.Message);
                return null;
            }
        }

        public async Task<string> SavePengajuanDB(SavePengajuan dt, string strconnection)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", dt.Token);

                    var formData = new MultipartFormDataContent
                    {
                        { new StringContent(dt.UserID ?? ""), "UserID" },
                        { new StringContent(dt.GroupName?? ""), "GroupName" },
                        { new StringContent(dt.Status ?? ""), "Status" },
                        { new StringContent(dt.NoTransksi ?? ""), "NoTransksi" },
                        { new StringContent(dt.KodeVoucher ?? ""), "KodeVoucher" },
                        { new StringContent(dt.Tglbeli ?? ""), "Tglbeli" },
                        { new StringContent(dt.PNBP ?? ""), "PNBP" },
                        { new StringContent(dt.Keterangan ?? ""), "Keterangan" },
                        { new StringContent(dt.filed ?? ""), "filed" },
                        { new StringContent(dt.Wilayah_PT ?? ""), "Wilayah_PT" },
                        { new StringContent(dt.data_id ?? ""), "data_id" },
                        { new StringContent(dt.mxy_no ?? ""), "mxy_no" },

                    };

                    var response = await client.PostAsync(strconnection + "api/AHU/SaveVoucherAHU", formData);

                    if (response.IsSuccessStatusCode)
                    {

                        var jsonResponse = await response.Content.ReadAsStringAsync();
                        //var formattedJson = JToken.Parse(jsonResponse).ToString(Formatting.Indented); // Format JSON
                        return jsonResponse;
                    }
                    else
                    {
                        // Logging error response
                        var errorResponse = await response.Content.ReadAsStringAsync();
                        LogError("Error in CallSecondApiAsync: " + errorResponse);
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                LogError("Exception in CallSecondApiAsync: " + ex.Message);
                return null;
            }
        }

        public async Task<List<Response>> GetdataInvoice(string strconnection, string Startdate, string EndDate, string cabang)
        {

            try
            {
                if (string.IsNullOrEmpty(strconnection))
                {
                    throw new ArgumentException("String koneksi tidak boleh kosong.");
                }

                ParamGetPengajuan mod = new ParamGetPengajuan
                {
                    Status = "",
                    UserID = "",
                    GroupName = "",
                    Tipe = "001",
                    endDate = "",
                    startDate = ""
                };
                GetData op = new GetData();
                List<Response> dt = await op.GetDataPengecekanIn(mod, strconnection);

                if (string.IsNullOrEmpty(Startdate) && string.IsNullOrEmpty(EndDate) && string.IsNullOrEmpty(cabang))
                {
                    return dt;
                }
                else if (string.IsNullOrEmpty(cabang))
                {
                    DateTime startDate = DateTime.Parse(Startdate).Date;
                    DateTime endDate = DateTime.Parse(EndDate).Date;

                    var filteredData = dt.Where(item =>
                    DateTime.TryParse(item.Tgl_Pengajuan, out DateTime tglPengajuan) && // Memastikan Tgl_Pengajuan valid
                    tglPengajuan.Date >= startDate && tglPengajuan.Date <= endDate).ToList();
                    return filteredData;

                }
                else if (string.IsNullOrEmpty(Startdate) && string.IsNullOrEmpty(EndDate))
                {
                    var filteredData = dt.Where(item =>
                    item.cabang == cabang
                    ).ToList();
                    return filteredData;
                }
                else
                {
                    DateTime startDate = DateTime.Parse(Startdate).Date;
                    DateTime endDate = DateTime.Parse(EndDate).Date;

                    var filteredData = dt.Where(item =>
                    DateTime.TryParse(item.Tgl_Pengajuan, out DateTime tglPengajuan) && // Memastikan Tgl_Pengajuan valid
                    tglPengajuan.Date >= startDate && tglPengajuan.Date <= endDate && item.cabang == cabang).ToList();
                    return filteredData;
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Terjadi kesalahan saat mengambil data invoice.", ex);
            }
        }

        public async Task<string> UpdateInvoice(ParamSaveInvoice sve, string strconnection, InvoiceInput mod, SavePengajuan ob, List<Response> rs)
        {
            try
            {

                var token = await GetAccessTokenAsync(strconnection);

                ParamSaveInvoice newdt = new ParamSaveInvoice();
                newdt.NoTransaksi = sve.NoTransaksi;
                newdt.NoInvoice = sve.NoInvoice;
                newdt.accesToken = token;
                ob.Token = token;
                var resSave = await SavePengajuanDB(ob, strconnection);
                if (resSave == null || resSave.Length < 1)
                {
                    var msg = "Gagal menyimpan Data";
                    return msg;
                } else
                {
                    using (var client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                        var noTransaksiList = rs.Select(item => item.NoTransaksi).ToList();
                        var requestData = new
                        {
                            NoInvoice = mod.invoice,
                            NoTransaksi = noTransaksiList // Pastikan 'rs' adalah koleksi yang kompatibel seperti List<string> atau array
                        };
                        // Serialisasi objek ke JSON
                        var jsonContent = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");

                        // Kirim POST request dengan JSON
                        var response = await client.PostAsync(strconnection + "api/AHU/InvoiceUpdateData", jsonContent);

                        //var formData = new MultipartFormDataContent();
                        //formData.Add(new StringContent(mod.invoice), "NoInvoice");

                        //foreach (var NoTransaksi in rs)
                        //{
                        //    formData.Add(new StringContent(NoTransaksi.ToString()), "NoTranskasi[]");
                        //}

                        //var response = await client.PostAsync(strconnection + "api/AHU/InvoiceUpdateData", formData);

                        if (response.IsSuccessStatusCode)
                        {
                            var jsonResponse = "Save Succesfully";
                            return jsonResponse;
                        }
                        else
                        {
                            // Logging error response
                            var errorResponse = await response.Content.ReadAsStringAsync();
                            LogError("Error in CallSecondApiAsync: " + errorResponse);
                            return null;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                LogError("Exception in CallSecondApiAsync: " + ex.Message);
                return null;
            }
        }


        private void LogError(string message)
        {
            Console.WriteLine(message);
        }

    }
    public class ParamSaveInvoice
    {
        public string NoInvoice { get; set; }
        public List<string> NoTransaksi { get; set; }
        public string accesToken { get; set; }
        public string TglMulai { get; set; }
        public string TglTempo { get; set; }
    }

    public class ParamGetPengajuan
    {
        public string UserID { get; set; }
        public string GroupName { get; set; }
        public string Status { get; set; }
        public string Tipe { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
        public string accesToken { get; set; }
    }

    public class TokenResponse
    {
        [JsonProperty("access_token")]
        public string access_token { get; set; }
    }
    public class SavePengajuan
    {
        public string NoTransksi { get; set; }
        public string KodeVoucher { get; set; }
        public string Tglbeli { get; set; }
        public string PNBP { get; set; }
        public string Status { get; set; }
        public string UserID { get; set; }
        public string GroupName { get; set; }
        public string Keterangan { get; set; }
        public string filed { get; set; }
        public string Wilayah_PT { get; set; }
        public string data_id { get; set; }
        public string mxy_no { get; set; }
        public string Token { get; set; }
    }


}