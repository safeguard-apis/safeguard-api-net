using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SafeGuardServerAPIV2
{
    public class Risk
    {
        public Risk(String Loc, float RiskScore, List<RiskItem> riskItems){
            this.Loc = Loc;
            this.RiskScore = RiskScore;
            this.riskItems = riskItems;
        }

        public String Loc {set; get;}

        public float RiskScore { set; get; }

        public List<RiskItem> riskItems { set; get; }
    }
}


