using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ADC_Enable_UI.Models;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web.Script.Serialization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Configuration;  // 跟 app.config 連接
using System.Data.SqlClient;  // for sql
using System.IO;
using System.Security.Policy;

namespace ADC_Enable_UI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(FormCollection obj)
        {
            //Session.Remove["UserName"];
            Session.Abandon();
            if (TempData["ErrorMessage"] != null)
            {
                ViewBag.ErrorMessage = TempData["ErrorMessage"];
            }
            return View();

        }
   



        public ActionResult Search()
        {
          

            return View();

        }


        public ActionResult Disable_adc_Set(string ToolId, string KlarfVersion, string SubstrateType, string adc_stats,string submit)
        {
            var user = Session["UserName"];
            //bool ADC_stats = bool.Parse(adc_stats);
            if (adc_stats == "True")
            {
                var result=Common.Change_ADC(ToolId, KlarfVersion, SubstrateType, "false");
                ViewData["ToolId"] = ToolId;
                ViewData["KlarfVersion"] = KlarfVersion;
                ViewData["SubstrateType"] = SubstrateType;
                ViewData["adc_stats"] = "false";
                StreamWriter sw = new StreamWriter("\\\\tbwops01\\e$\\Shawn_source\\repos\\ADC_Enable_UI\\log.txt", true);
                sw.WriteLine(DateTime.Now.ToLocalTime());
                sw.WriteLine(user.ToString());
                sw.WriteLine(DateTime.Now.ToLocalTime());
                sw.WriteLine("change the tool "+ ToolId + " ADC_STATES to false");
                sw.Close();
                Common.Send_Email(user.ToString(), ToolId, "false");

            }
            else
            {
                var result = Common.Change_ADC(ToolId, KlarfVersion, SubstrateType, "true");
                ViewData["ToolId"] = ToolId;
                ViewData["KlarfVersion"] = KlarfVersion;
                ViewData["SubstrateType"] = SubstrateType;
                ViewData["adc_stats"] = "true";
                StreamWriter sw = new StreamWriter("\\\\tbwops01\\e$\\Shawn_source\\repos\\ADC_Enable_UI\\log.txt", true);
                sw.WriteLine(DateTime.Now.ToLocalTime());
                sw.WriteLine(user.ToString());
                sw.WriteLine(DateTime.Now.ToLocalTime());
                sw.WriteLine("change the tool " + ToolId + " ADC_STATES to true");
                sw.Close();
                Common.Send_Email(user.ToString(), ToolId, "true");
            }

            return View();
        }
        // forward the async type of the comman function.
        public static void send_mail_forward(string user, string ToolId, string adc_stats)
        {
            Common.Send_Email(user, ToolId, adc_stats);
        }


        public ActionResult Check()
        {


            return View();
        }

        public async Task<ActionResult> Verify(FormCollection obj)
        {
            string account = obj["username"].ToString();
            string password = obj["password"].ToString();
            byte[] data = Convert.FromBase64String(password);
            string decodedPassword = Encoding.UTF8.GetString(data);
            ViewBag.Account = obj["username"];
            ViewBag.Password = decodedPassword;

            string result = await Common.Login(account, decodedPassword);

            if (result.Contains("Set-Cookie"))
            {
                Session["UserName"] = account;
                // 登入成功，轉址到 /home/search
                return RedirectToAction("setting", "Home");
            }
            else
            {
                TempData["ErrorMessage"] = "Login Failed, Please Enter Correct Username and Password";
                return RedirectToAction("Index", "Home");

            }

        }

        public ActionResult Setting(string cluster_id)
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Relogin", "Home");
            }
            try
            {
                string connetionString;
                SqlConnection cnn;
                connetionString = @"Data Source=TBMSSPROD38;Initial Catalog=RDASUITEBE;User ID=cats;Password=Micron123";
                cnn = new SqlConnection(connetionString);
                cnn.Open();
                SqlCommand scom = new SqlCommand("", cnn);
                scom.CommandText = $@"SELECT TOP (1000) [tool_ID]
                                      ,[klarf_version]
                                      ,[substrate_type]
                                      ,[adc_Enable]
                                      ,[destination_path]
                                      ,[insert_date_time]
                                  FROM [RDASUITEBE].[dbo].[inspection_tool_config_info]
                                  where tool_ID = '" + cluster_id + "'";
                SqlDataReader sread = scom.ExecuteReader();
                if (scom != null)
                {
                    while (sread.Read())
                    {
                        int i = sread.FieldCount;
                        Cluster2 b = new Cluster2(cluster_id, sread.GetValue(1).ToString(), sread.GetValue(2).ToString(), (bool)sread.GetValue(3));
                        return View(b);

                    }
                }
                sread.Close();
                cnn.Close();
                return RedirectToAction("Setting_Home", "Home");
            }
            catch (Exception e)
            {

                TempData["ErrorMessage"] = e.Message;
                return RedirectToAction("Setting_Home", "Home");

            }
        }


        public ActionResult Setting_Home()
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Relogin", "Home");
            }
            if (TempData["ErrorMessage"] != null)
            {
                ViewBag.ErrorMessage = TempData["ErrorMessage"];
            }


            return View();
        }
        public ActionResult Relogin(FormCollection obj)
        {

            return View();

        }
        public ActionResult Logout(FormCollection obj)
        {
            Session.Clear();
            return View();

        }



    }
}