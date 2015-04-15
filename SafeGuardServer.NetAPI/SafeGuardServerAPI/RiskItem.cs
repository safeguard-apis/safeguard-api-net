using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SafeGuardServerAPIV2
{
    public class RiskItem
    {
        public RiskItem(String Type, int Score) {
            this.Score = Score;
            this.Type = Type;
        }
        public String Type { set; get; }
        public int Score {set; get;}
    }
}
