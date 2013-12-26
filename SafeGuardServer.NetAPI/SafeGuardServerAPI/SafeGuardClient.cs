using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Web.Script.Serialization;
using System.IO;
using System.Configuration;
using System.Runtime.Serialization.Json;

namespace SafeGuardServerAPI
{
    public class SafeGuardClient
    {
        private String Application;
        private String ApplicationToken;
        private String Url;

        private void SetParams(String application, String applicationToken, String serverURL)
        {
            if (String.IsNullOrEmpty(application) || String.IsNullOrEmpty(applicationToken))
                throw new Exception("It's manadtory to pass a valid application name, and application token");
            this.Application = application;
            this.ApplicationToken = applicationToken;
            this.Url = serverURL;
        }

        public SafeGuardClient(String application, String applicationToken, String serverURL)
        {
            SetParams(application, applicationToken, serverURL);
        }

        public SafeGuardClient(String application, String applicationToken)
        {
            string url = ConfigurationManager.AppSettings["SafeGuardURL"];
            SetParams(application, applicationToken, url);
        }
        
        public Boolean ContextMustUsehardwareToken(string context, string user) 
        {
            try
            {
                string json = "{\"context\": \"" + context + "\", \"user\": \"" + user + "\"}";
                return executePost("/context_must_use_hw_token.json", json);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public Boolean ValidateTransactionToken(string token, string otp, string context, string user)
        {
            try
            {
                string json = "{\"token\": \"" + token + "\", \"otp\": \"" + otp + "\", \"context\": \"" + context + "\", \"user\": \"" + user + "\"}";
                return executePost("/validate_transaction_token.json", json);
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
                return executePost("/insert_issue_log_in_transaction_token.json", json);
            }
            catch (Exception e)
            {
                throw e;
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
            webRequest.UserAgent = this.Application + "(" + this.ApplicationToken + ")";
            return webRequest;
        }
    }
}
