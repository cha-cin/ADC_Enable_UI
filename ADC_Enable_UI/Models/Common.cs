using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Configuration;  // 跟 app.config 連接
using System.Data.SqlClient;  // for sql
using System.IO;
//using HtmlAgilityPack;
using System.Xml;
using Newtonsoft.Json;
using System.Text;

namespace ADC_Enable_UI.Models
{
    public class Group
    {

        public string parent_id { get; set; }
        public string child_id { get; set; }
        public string child_nickname { get; set; }
        public string child_relationship_id { get; set; }
        public string impact { get; set; }
        public Group() { }
        public Group(string parent_id, string child_id, string child_nickname, string child_relationship_id, string impact)//第一個建構函式，帶入一個int參數，初始化時會將參數賦值給Salary
        {
            this.parent_id = parent_id;
            this.child_id = child_id;
            this.child_nickname = child_nickname;
            this.child_relationship_id = child_relationship_id;
            this.impact = impact;
        }
    }
    public class Group_id  //joint cluster to Group for setting
    {

        public int id { get; set; }
        public string cluster_id { get; set; }

        public string parent_id { get; set; }
        public string child_id { get; set; }
        public string child_nickname { get; set; }
        public string child_relationship_id { get; set; }
        public string impact { get; set; }
        public Group_id() { }
        public Group_id(int id, string cluster_id, string parent_id, string child_id, string child_nickname, string child_relationship_id, string impact)//第一個建構函式，帶入一個int參數，初始化時會將參數賦值給Salary
        {
            this.cluster_id = cluster_id;
            this.id = id;
            this.parent_id = parent_id;
            this.child_id = child_id;
            this.child_nickname = child_nickname;
            this.child_relationship_id = child_relationship_id;
            this.impact = impact;
        }
    }
    public class Cluster_Set
    {
        public string only_cluster_id { get; set; }
        public string cluster_id { get; set; }
        public string handler_id { get; set; }
        public string tester_id { get; set; }
        public string equip_type_id { get; set; }
        public string autoshell_process { get; set; }

        public Cluster_Set() { }
        public Cluster_Set(string _only_cluster_id, string _cluster_id, string _handler_id, string _tester_id, string _equip_type_id, string _autoshell_process, Group[] _data)
        {
            only_cluster_id = _only_cluster_id;
            cluster_id = _cluster_id;
            handler_id = _handler_id;
            tester_id = _tester_id;
            equip_type_id = _equip_type_id;
            autoshell_process = _autoshell_process;



        }
    }

    public class Cluster
    {

        public string ToolId { get; set; }
        public string KlarfVersion { get; set; }
        public string SubstrateType { get; set; }
        public string adc_stats { get; set; }



        public Cluster(string _ToolId, string _KlarfVersion, string _SubstrateType, string _adc_stats)
        {

            ToolId = _ToolId;
            KlarfVersion = _KlarfVersion;
            SubstrateType = _SubstrateType;
            adc_stats = _adc_stats;

        }
    }

    public class Cluster2
    {

        public string ToolId { get; set; }
        public string KlarfVersion { get; set; }
        public string SubstrateType { get; set; }
        public Boolean adc_stats { get; set; }

        public Cluster2(string _cluster_id, string _KlarfVersion, string _SubstrateType, Boolean _adc_stats)
        {
            ToolId = _cluster_id;
            KlarfVersion = _KlarfVersion;
            SubstrateType = _SubstrateType;
            adc_stats = _adc_stats;

        }
    }


    public class Common
    {
       

            public static string Change_ADC(string ToolId, string KlarfVersion, string SubstrateType, string ADCEnabled)
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, "http://10.20.125.140:80/PROD/RDAUpStreamProcessor/UpdateAdcEnable");
                var content = new StringContent("{\r\n\r\n \r\n\r\n    \"ToolInfoList\":\r\n\r\n \r\n\r\n    [\r\n\r\n \r\n\r\n        {\r\n\r\n \r\n\r\n            \"ToolId\": \""+ ToolId + "\",\r\n\r\n \r\n\r\n            \"KlarfVersion\": \""+ KlarfVersion + "\",\r\n\r\n \r\n\r\n            \"SubstrateType\": \""+ SubstrateType + "\",\r\n\r\n \r\n\r\n            \"ADCEnabled\": "+ ADCEnabled + ",\r\n\r\n \r\n\r\n            \"DestinationPathList\": []\r\n\r\n \r\n\r\n        }\r\n\r\n \r\n\r\n    ]\r\n\r\n \r\n\r\n}", null, "application/json");
                request.Content = content;
                var response = client.SendAsync(request).Result;
                var jsonString = response.Content.ReadAsStringAsync().Result;
                StreamWriter sw = new StreamWriter("\\\\tbwops01\\e$\\Shawn_source\\repos\\ADC_Enable_UI\\log.txt", true);
                sw.WriteLine(response.ToString());
                sw.Close();
                return response.ToString();
            }


        public static Task<String> Login(string user, string password)
        {
            HttpClientHandler handler = new HttpClientHandler();


            HttpClient client = new HttpClient(handler);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://smlogin.micron.com/siteminderagent/forms/login.fcc?TARGET=-SM-http%3a%2f%2fwebtest.tbtw.micron.com");

            request.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7");
            request.Headers.Add("Accept-Language", "zh-TW,zh;q=0.9,en-US;q=0.8,en;q=0.7");
            request.Headers.Add("Cache-Control", "max-age=0");
            request.Headers.Add("Connection", "keep-alive");
            request.Headers.Add("Cookie", "ASP.NET_SessionId=lggsf4afn3foxu45250y10jb");
            request.Headers.Add("Origin", "https://smlogin.micron.com");
            request.Headers.Add("Referer", "https://smlogin.micron.com/siteminderagent/forms/ntlm/default.aspx?TARGET=-SM-http%3a%2f%2fwebtest.tbtw.micron.com");

            var data = new Dictionary<string, string>
            {
                { "USER", user },
                { "PASSWORD", password },
                { "SMENC", "ISO-8859-1" },
                { "SMLOCALE", "US-EN" }
            };
            var content = new FormUrlEncodedContent(data);
            var response = client.PostAsync("https://smlogin.micron.com/siteminderagent/forms/login.fcc?TARGET=-SM-http%3a%2f%2fwebtest.tbtw.micron.com", content).Result;
            return Task.FromResult(response.Headers.ToString());


        }
        public static void Send_Email(string user, string ToolId, string adc_stats)
        {
            Dictionary<string, string> jsonValues = new Dictionary<string, string>();
            var path = "E:\\Shawn_source\\repos\\ADC_Enable_UI\\Configuration.txt";
            string[] lines = File.ReadAllLines(path, Encoding.UTF8);
            string mailcontract = "";
            foreach (string line in lines)
            {
                mailcontract = line;
            }

            jsonValues.Add("Title", "ADC_STATS change");
            jsonValues.Add("Content", DateTime.Now.ToString("MM-dd-yyyy-hh:mm:ss") + " " + user + " " + "has changed the" + " " + ToolId + " in " + adc_stats + " Successfully!");
            jsonValues.Add("Send_From", "TBMES@micron.com");
            jsonValues.Add("Send_To", user + "@micron.com" + mailcontract);
            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), "http://tbwops01:1003/api/Values/SendEmail"))
                {
                    request.Headers.TryAddWithoutValidation("accept", "*/*");
                    request.Content = new StringContent(JsonConvert.SerializeObject(jsonValues), UnicodeEncoding.UTF8, "application/json");
                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json-patch+json");

                    var response = httpClient.SendAsync(request).Result;


                }
            }
        }



    }
    }
