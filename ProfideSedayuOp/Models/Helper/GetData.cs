using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ProfideSedayuOp.Models.Helper
{
    public class GetData
    {

        public async Task<string> GetDataPengecekanAPI(ParamGetPengajuan dt,string strconnection)
        {
            MitsuiPengecekan data = new MitsuiPengecekan();
            var token = await data.GetAccessTokenAsync(strconnection);

            ParamGetPengajuan newdt = new ParamGetPengajuan();

            newdt.Status = dt.Status;
            newdt.UserID = dt.UserID;
            newdt.GroupName = dt.GroupName;
            newdt.Tipe = dt.Tipe;
            newdt.endDate = dt.endDate;
            newdt.startDate = dt.startDate;
            newdt.accesToken = token;


            var apiResponse = await data.getPengajuan(newdt, strconnection);
            List<string> list = new List<string>();
            if (apiResponse != null)
            {
                return apiResponse;
            }
            else
            {
                string err = "Gagal Memproses Data";
                return err;
            }
        }
        public async Task<string> SaveDataPengecekanAPI(SavePengajuan dt, string strconnection)
        {
            MitsuiPengecekan data = new MitsuiPengecekan();
            var token = await data.GetAccessTokenAsync(strconnection);

            SavePengajuan newdt = new SavePengajuan();
            newdt.Status=dt.Status;
            newdt.NoTransksi = dt.NoTransksi;
            newdt.Token = token;
            var apiResponse = await data.SavePengajuanDB(newdt, strconnection);
            List<string> list = new List<string>();
            if (apiResponse != null)
            {
                return apiResponse;
            }
            else
            {
                string err = "Gagal Memproses Data";
                return err;
            }
        }


        public async Task<List<Response>> GetDataPengecekanIn(ParamGetPengajuan dt,string strconnection)
        {
            MitsuiPengecekan data = new MitsuiPengecekan();

            var token = await data.GetAccessTokenAsync(strconnection);

            // Buat parameter baru dengan data token
            ParamGetPengajuan newdt = new ParamGetPengajuan
            {
                Status = dt.Status,
                UserID = dt.UserID,
                GroupName = dt.GroupName,
                Tipe = dt.Tipe,
                endDate = dt.endDate,
                startDate = dt.startDate,
                accesToken = token
            };

            string apiResponse = await data.getPengajuan(newdt, strconnection);

            // Deserialize JSON menjadi List<Response>
            List<Response> responseList = JsonConvert.DeserializeObject<List<Response>>(apiResponse);

            if (responseList != null && responseList.Count > 0)
            {
                List<Response> updatedResponseList = new List<Response>();

                foreach (var item in responseList)
                {
                    // Buat objek Response baru berdasarkan item
                    Response newItem = new Response
                    {
                        NoTransaksi = item.NoTransaksi,
                        cabang = item.cabang,
                        Nama_PT = item.Nama_PT,
                        Nama = item.Nama,
                        Alamat = item.Alamat,
                        Email = item.Email,
                        NoTelp = item.NoTelp,
                        NamaPT_Ceking = item.NamaPT_Ceking,
                        NIK_Ceking = item.NIK_Ceking,
                        TglLahir_Ceking = item.TglLahir_Ceking,
                        Status_Pengajuan = item.Status_Pengajuan,
                        Status_PengajuanDesc = item.Status_PengajuanDesc,
                        Tgl_Pengajuan = item.Tgl_Pengajuan,
                        Tgl_Update = item.Tgl_Update,
                        Tgl_Voucher = item.Tgl_Voucher,
                        SLA = item.SLA,
                        TipeTransaksi_Ceking = item.TipeTransaksi_Ceking,
                        Jenis_Badan = item.Jenis_Badan,
                        Wilayah_PT = item.Wilayah_PT,
                        Kode_Voucher = item.Kode_Voucher,
                        PNBP = item.PNBP,
                        TipeTransaksi_CekingDesc = item.TipeTransaksi_CekingDesc,
                        TipeTransaksi_Par = item.TipeTransaksi_Par,
                        TipeTransaksi_Val = item.TipeTransaksi_Val,
                        File_pdf = item.File_pdf,
                        User_Pengaju = item.User_Pengaju,
                        data_id = item.data_id,
                        mxy_no = item.mxy_no,
                        Keterangan = item.Keterangan
                    };

                    updatedResponseList.Add(newItem);
                }

                return updatedResponseList;
            }
            else
            {
                return new List<Response>
                {
                    new Response { Keterangan = "Data Kosong" }
                };
            }
        }


    }

}