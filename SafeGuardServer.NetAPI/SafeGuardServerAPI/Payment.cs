using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SafeGuardServerAPIV2
{
    [DataContract(Name = "payment")]
    public class Payment
    {
        
        public Payment(String Type) {
            this.Type = Type;
        }

        public Payment(String Type, Decimal price)
        {
            this.Type = Type;
            this.price = price;
        }

        public Payment(CreditCardInfo info)
        {
            this.Type = "credit_card";
            this.CreditCardInfo = info;
        }

        public Payment(CreditCardInfo info, Decimal price)
        {
            this.Type = "credit_card";
            this.CreditCardInfo = info;
            this.price = price;
        }

        [DataMember(Name = "type")]
        String Type { set; get; }

        [DataMember(Name = "info")]
        CreditCardInfo CreditCardInfo { set; get; }

        [DataMember(Name = "price")]
        Decimal price { set; get; }
    }
}
