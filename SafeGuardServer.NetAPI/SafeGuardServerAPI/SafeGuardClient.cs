using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Web.Script.Serialization;
using System.IO;
using System.Configuration;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json;

namespace SafeGuardServerAPI
{
    public class SafeGuardClient
    {
        private String ApplicationToken;
        private String Url;

        private void SetParams(String applicationToken, String serverURL)
        {
            if (String.IsNullOrEmpty(applicationToken))
                throw new Exception("It's manadtory to pass a valid application name, and application token");
            this.ApplicationToken = applicationToken;
            this.Url = serverURL;
        }

        public SafeGuardClient(String applicationToken, String serverURL)
        {
            SetParams(applicationToken, serverURL);
        }

        public SafeGuardClient(String applicationToken)
        {
            string url = ConfigurationManager.AppSettings["SafeGuardURL"];
            SetParams(applicationToken, url);
        }
        
        public Boolean ContextMustUsehardwareToken(string context, string user) 
        {
            try
            {
                string json = "{\"context\": \"" + context + "\", \"user\": \"" + user + "\"}";
                return executePost("/v1.2/context_must_use_hw_token.json", json);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public Boolean ValidateOtp(string transactionToken, string otp, string context, string user)
        {
            try
            {
                string json = "{\"token\": \"" + transactionToken + "\", \"otp\": \"" + otp + "\", \"context\": \"" + context + "\", \"user\": \"" + user + "\"}";
                return executePost("/v1.2/validate_otp.json", json);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public Boolean ValidateTransactionToken(string token, string context, string user)
        {
            try
            {
                string json = "{\"token\": \"" + token + "\", \"context\": \"" + context + "\", \"user\": \"" + user + "\"}";
                return executePost("/v1.2/validate_transaction_token.json", json);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<Risk> AnalyzeRisk(List<Locator> locators, String token)
        {
            string json = "";
            try
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(locators.GetType());
                using (MemoryStream ms = new MemoryStream())
                {
                    serializer.WriteObject(ms, locators);
                    json = Encoding.Default.GetString(ms.ToArray());
                }
                json = "{\"transaction_token\":\"" + token + "\", \"locators\":" + json + "}";
                
                             
                return executePostToRisk("/v2.0/analyze_risk.json", json);
            }
            catch (Exception e)
            {
                throw e;
            }
        }




        public Boolean LogTicketsIssued(List<Locator> locators, String token)
        {
            string json = "";
            try
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(locators.GetType());
                using (MemoryStream ms = new MemoryStream())
                {
                    serializer.WriteObject(ms, locators);
                    json = Encoding.Default.GetString(ms.ToArray());
                }
                json = "{\"token\":\"" + token + "\", \"locators\":" + json + "}";
                return executePost("/v1.2/insert_issue_log_in_transaction_token.json", json);
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        private List<Risk> executePostToRisk(String relativeURL, String body)
        {
            var webRequest = GenerateWebRequest(relativeURL);
            webRequest.Method = "POST";
            webRequest.ContentType = "application/json";
            webRequest.Accept = "application/json";
            using (var writer = new StreamWriter(webRequest.GetRequestStream()))
            {
                writer.Write(body);
            }

            try
            {
                var webResponse = (HttpWebResponse)webRequest.GetResponse();
                
                StreamReader reader = reader = new StreamReader(webResponse.GetResponseStream());
                string resp = reader.ReadToEnd().Trim();
 
                Dictionary<string, dynamic> parsedResp = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(resp);
                if (webResponse.StatusCode == HttpStatusCode.OK)
                {
                    List l = parsedResp["locs_risk"]   
                }

                return null;
            }
            catch (WebException we)
            {
                HttpWebResponse webResponse = ((HttpWebResponse)we.Response);
                StreamReader reader = reader = new StreamReader(webResponse.GetResponseStream());
                string resp = reader.ReadToEnd().Trim();
                Dictionary<string, dynamic> parsedResp = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(resp);
                
                if(parsedResp["message"] != null)
                    throw new SafeGuardException(parsedResp["message"], parsedResp["error_code"]);
                throw we;
            }

        }

        private Boolean executePost(String relativeURL, String body)
        {
            var webRequest = GenerateWebRequest(relativeURL);
            webRequest.Method = "POST";
            webRequest.ContentType = "application/json";
            using (var writer = new StreamWriter(webRequest.GetRequestStream()))
            {
                writer.Write(body);
            }

            try
            {
                var webResponse = (HttpWebResponse)webRequest.GetResponse();

                if (webResponse.StatusCode == HttpStatusCode.OK)
                    return true;
                else
                    return false;
            }
            catch (WebException we)
            {
                if (((HttpWebResponse)we.Response).StatusCode == HttpStatusCode.NotFound)
                    return false;
                else if (((HttpWebResponse)we.Response).StatusCode == HttpStatusCode.Forbidden)
                    return false;
                else
                    throw we;
            }
        
        }

        private Boolean executeGet(String relativeURL)
        {
            var webRequest = GenerateWebRequest(relativeURL);

            webRequest.Method = "GET";
            try
            {
                var webResponse = (HttpWebResponse)webRequest.GetResponse();

                if (webResponse.StatusCode == HttpStatusCode.OK)
                    return true;
                else
                    return false;
            }
            catch (WebException we)
            {
                if (((HttpWebResponse)we.Response).StatusCode == HttpStatusCode.NotFound)
                    return false;
                else if (((HttpWebResponse)we.Response).StatusCode == HttpStatusCode.Forbidden)
                    return false;
                else
                    throw we;
            }
        }

        private HttpWebRequest GenerateWebRequest(String relativeURL)
        {
            System.Net.ServicePointManager.Expect100Continue = false;
            var webRequest = (HttpWebRequest)WebRequest.Create(Url + relativeURL);
            webRequest.Accept = "*/*";
            webRequest.Headers.Add("Accept-Encoding", "gzip,deflate,sdch");
            webRequest.Headers.Add("Accept-Language", "pt-BR,pt;q=0.8,en-US;q=0.6,en;q=0.4");
            return webRequest;
        }

        
    }
}
