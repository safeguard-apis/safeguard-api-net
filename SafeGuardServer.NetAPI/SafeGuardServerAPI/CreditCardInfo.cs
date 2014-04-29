using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SafeGuardServerAPI
{
    [DataContract(Name = "info")]
    public class CreditCardInfo
    {
        public CreditCardInfo(String Name, String number) {
            this.Name = Name;
            this.Number = Number;
        }

        [DataMember(Name = "name")]
        String Name { set; get; }

        [DataMember(Name = "number")]
        String Number { set; get; }

    }
}
