using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SafeGuardServerAPI
{
    public class Risk
    {
        public Risk(String Loc, float RiskScore, List<RiskItem> riskItems){
            this.Loc = Loc;
            this.RiskScore = RiskScore;
            this.riskItems = riskItems;
        }

        String Loc {set; get;}

        float RiskScore { set; get; }

        List<RiskItem> riskItems { set; get; }
    }
}


