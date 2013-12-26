using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SafeGuardServerAPI
{
    [DataContract(Name = "ticket")]
    public class Ticket
    {
        public Ticket(String number, DateTime issuedAt, String passenger, Decimal price, String Currency)
        { 
            this.Number = number;
            this.IssuedAt = issuedAt;
            this.Passenger = passenger;
            this.Price = price;
            this.Currency = Currency;
        }

        [DataMember(Name = "number")]
        String Number { set; get; }

        [DataMember(Name = "issued_at")]
        private String IssuedAtSerializable { set; get; }

        DateTime IssuedAt { set; get; }

        [DataMember(Name = "currency")]
        String Currency { set; get; }

        [DataMember(Name = "passenger")]
        String Passenger { set; get; }

        [DataMember(Name = "price")]
        Decimal Price { set; get; }

        [OnSerializing]
        void OnSerializing(StreamingContext context)
        {
            this.IssuedAtSerializable = this.IssuedAt.ToString("yyyy/MM/dd HH:mm:ss zzz");
        }
    }
}
