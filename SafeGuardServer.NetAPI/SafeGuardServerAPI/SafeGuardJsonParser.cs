using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SafeGuardServerAPI
{
    public class SafeGuardJsonParser
    {
        public List<Risk> parseAnalyzeSuccessResponse(String json) {

            try
            {
                Dictionary<string, dynamic> htmlAttributes = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(json);

                List<Risk> risks = new List<Risk>();
                foreach (JToken locRisk in ((JArray)htmlAttributes["locs_risk"]))
                {

                    float risk_value = locRisk["risk"].Value<float>();
                    string loc = locRisk["loc"].Value<string>();
                    List<RiskItem> riskItems = new List<RiskItem>();
                    foreach (JToken itemRisk in ((JArray)locRisk["summarized_risk"]))
                    {
                        RiskItem item = new RiskItem(itemRisk["type"].Value<string>(), itemRisk["score"].Value<int>());
                        riskItems.Add(item);
                    }

                    Risk risk = new Risk(loc, risk_value, riskItems);
                    risks.Add(risk);
                }

                return risks;
            }
            catch (Exception)
            {
                throw new SafeGuardException("Error occurs in parsing response", -90);
            }
        }

        public SafeGuardException buildSafeGuardException(String json) { 
            Dictionary<string, dynamic> parsedResp = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(json);

            if (parsedResp["message"] != null)
                return new SafeGuardException(parsedResp["message"], parsedResp["error_code"]);
            return new SafeGuardException("Erro desconhecido", -99);
        }
     
    }
}
