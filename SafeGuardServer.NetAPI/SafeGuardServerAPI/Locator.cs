using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SafeGuardServerAPI
{
    [DataContract(Name = "locator")]
    public class Locator
    {
        public Locator(String locator, Boolean isInternational, List<Ticket> tickets, List<Payment> payments)
        {
            this.Loc = locator;
            this.IsInternational = isInternational;
            this.Payments = payments;
            this.Tickets = tickets;
        }

        [DataMember(Name = "loc")]
        String Loc{set; get;}

        [DataMember(Name = "is_international")]
        Boolean IsInternational { set; get; }

        [DataMember(Name = "tickets")]
        List<Ticket> Tickets { set; get; }

        [DataMember(Name = "payments")]
        List<Payment> Payments { set; get; }
    }
}
