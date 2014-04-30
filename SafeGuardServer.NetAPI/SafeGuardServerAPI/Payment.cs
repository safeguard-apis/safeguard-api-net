using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SafeGuardServerAPI
{
    [DataContract(Name = "payment")]
    public class Payment
    {
        
        public Payment(String Type) {
            this.Type = Type;
        }

        public Payment(CreditCardInfo info)
        {
            this.Type = "credit_card";
            this.CreditCardInfo = info;
        }

        [DataMember(Name = "type")]
        String Type { set; get; }

        [DataMember(Name = "info")]
        CreditCardInfo CreditCardInfo { set; get; }
    }
}
