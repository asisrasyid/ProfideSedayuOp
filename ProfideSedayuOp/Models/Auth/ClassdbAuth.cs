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

}