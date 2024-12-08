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

namespace ProfideSedayuOp.Models
{
    public class AuthGetDt
    {
        public async Task<DataTable> dbGetDataLogin(ParamLogin param)
        {

            DataTable dt;
            ExcdataHelper dbaccess = new ExcdataHelper();
            string strconnection = OwinLibrary.GetDBP();

            SqlParameter[] sqlParam =
                {
                new SqlParameter ("@UserName", param.Username),
                new SqlParameter ("@PassWord", param.Password),
            };

            dt = await dbaccess.ExecuteDataTable(strconnection, "app_dbget_CekAksesLogin", sqlParam);
            return dt;
        }
    }
}