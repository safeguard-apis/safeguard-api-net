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

namespace SafeGuardServerAPIV2
{
    public class SafeGuardClient
    {
        public enum Failures {PAYMENT = 1, EXPIRED = 2}
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
                return executePost("/v2.0/context_must_use_hw_token.json", json);
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public Boolean ValidateOtp(string transactionToken, string otp, string device_type)
        {
            try
            {
                string json = "{\"transaction_token\": \"" + transactionToken + "\", \"otp\": \"" + otp + "\", \"device_type\": \"" + device_type + "\"}";
                return executePost("/v2.0/validate_otp.json", json);
            }
            catch (Exception e)
            {
                if (e.GetType() == typeof(SafeGuardException))
                {
                    SafeGuardException sgException = (SafeGuardException)e;
                    if (sgException.ErrorCode == -6)
                        return false;
                }
                throw e;
            }
        }

        public Boolean NotifyFailure(string transactionToken, List<LocatorFailure> failures)
        {
            try
            {
                String json = "";
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(failures.GetType());
                using (MemoryStream ms = new MemoryStream())
                {
                    serializer.WriteObject(ms, failures);
                    json = Encoding.Default.GetString(ms.ToArray());
                }
                json = "{\"transaction_token\":\"" + transactionToken + "\", \"errors\":" + json + "}";

                return executePost("/v2.0/notify_failure.json", json);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public Boolean ValidateTransactionToken(string transactionToken)
        {
            try
            {
                string json = "{\"transaction_token\": \"" + transactionToken + "\"}";
                return executePost("/v2.0/validate_transaction_token.json", json);
            }
            catch (Exception e)
            {
                if (e.GetType() == typeof(SafeGuardException))
                {
                    SafeGuardException sgException = (SafeGuardException)e;
                    if (sgException.ErrorCode == -7)
                        return false;
                }
                throw e;
            }
        }

        public List<Risk> AnalyzeRisk(List<Locator> locators, String transactionToken)
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
                json = "{\"transaction_token\":\"" + transactionToken + "\", \"locators\":" + json + "}";
                
                             
                return executePostToRisk("/v2.0/analyze_risk.json", json);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<Risk> IssueLocators(List<Locator> locators, String transactionToken)
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
                json = "{\"transaction_token\":\"" + transactionToken + "\", \"locators\":" + json + "}";


                return executePostToRisk("/v2.0/issue_locators.json", json);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        
        private List<Risk> executePostToRisk(String relativeURL, String body)
        {
            var parser = new SafeGuardJsonParser();
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
 
                if (webResponse.StatusCode == HttpStatusCode.OK)
                    return parser.parseAnalyzeSuccessResponse(resp);
                throw new SafeGuardException("Invalid Response", -99);
                
            }
            catch (WebException we)
            {
                HttpWebResponse webResponse = ((HttpWebResponse)we.Response);
                StreamReader reader = reader = new StreamReader(webResponse.GetResponseStream());
                string resp = reader.ReadToEnd().Trim();
                throw parser.buildSafeGuardException(resp);
            }

        }

        private Boolean executePost(String relativeURL, String body)
        {
            var parser = new SafeGuardJsonParser();
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
                HttpWebResponse webResponse = ((HttpWebResponse)we.Response);
                StreamReader reader = reader = new StreamReader(webResponse.GetResponseStream());
                string resp = reader.ReadToEnd().Trim();
                throw parser.buildSafeGuardException(resp);
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
