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

    public class ParamLogin
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
    public class dbgetDateLo
    {
        public string Name { get; set; }
        public string Acces { get; set; }
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

    [Serializable]
    public class SeassonKey
    {
        public const string DataLog = "DataLog";
        public const string Name = "Name";
        public const string Admin = "Admin";
        public const string Caption = "Caption";
    }

}