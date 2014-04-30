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

        public Locator(String locator, Boolean isInternational)
        {
            this.Loc = locator;
            this.IsInternational = isInternational;
            this.Payments = new List<Payment>();
            this.Tickets = new List<Ticket>();
        }

        public void AddPayment(String type) {
            this.Payments.Add(new Payment(type)); 
        }

        public void AddPayment(CreditCardInfo card)
        {
            this.Payments.Add(new Payment(card));
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

        [DataMember(Name = "payments")]
        List<Payment> Payments { set; get; }
    }
}
