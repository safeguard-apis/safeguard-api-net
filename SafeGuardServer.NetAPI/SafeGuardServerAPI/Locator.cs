using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SafeGuardServerAPIV2
{
    [DataContract(Name = "locator")]
    public class Locator
    {
        public Locator(String locator, Boolean isInternational, List<Ticket> tickets)
        {
            this.Loc = locator;
            this.IsInternational = isInternational;
            this.Tickets = tickets;
        }

        public Locator(String locator, Boolean isInternational)
        {
            this.Loc = locator;
            this.IsInternational = isInternational;
            this.Tickets = new List<Ticket>();
        }

        public void AddTicket(Ticket ticket) {
            this.Tickets.Add(ticket);
        }

        [DataMember(Name = "loc")]
        String Loc{set; get;}

        [DataMember(Name = "is_international")]
        Boolean IsInternational { set; get; }

        [DataMember(Name = "tickets")]
        List<Ticket> Tickets { set; get; }

    }
}
