﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SafeGuardServerAPI
{
    public class RiskItem
    {
        public RiskItem(String Type, int Score) {
            this.Score = Score;
            this.Type = Type;
        }
        String Type { set; get; }
        int Score {set; get;}
    }
}