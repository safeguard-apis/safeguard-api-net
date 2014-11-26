using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SafeGuardServerAPIV2
{
    [DataContract(Name = "ticket")]
    public class Ticket
    {
        public Ticket(String number, DateTime issuedAt, String passenger, Decimal price, String Currency, List<FlightGroup> flightGroups, List<Payment> payments)
        { 
            this.Number = number;
            this.IssuedAt = issuedAt;
            this.Passenger = passenger;
            this.Price = price;
            this.Currency = Currency;
            this.flightGroups = flightGroups;
            this.Payments = payments;
        }

        public Ticket(String number, DateTime issuedAt, String passenger, Decimal price, String Currency)
        {
            this.Number = number;
            this.IssuedAt = issuedAt;
            this.Passenger = passenger;
            this.Price = price;
            this.Currency = Currency;
            this.flightGroups = new List<FlightGroup>();
            this.Payments = new List<Payment>();
        }


        public void AddFlightGroup(FlightGroup group){
            this.flightGroups.Add(group);
        }


        public void AddFlightGroup(String Origin, String Destination, DateTime DepartureAt, DateTime ArrivalAt)
        {
            this.flightGroups.Add(new FlightGroup(Origin, Destination, DepartureAt, ArrivalAt));
        }

        
        public void AddPayment(String type)
        {
            this.Payments.Add(new Payment(type));
        }

        public void AddPayment(String type, Decimal price)
        {
            this.Payments.Add(new Payment(type, price));
        }

        public void AddPayment(CreditCardInfo card)
        {
            this.Payments.Add(new Payment(card));
        }

        public void AddPayment(CreditCardInfo card, Decimal price)
        {
            this.Payments.Add(new Payment(card, price));
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

        [DataMember(Name = "flight_groups")]
        List<FlightGroup> flightGroups { set; get; }
        
        [DataMember(Name = "payments")]
        List<Payment> Payments { set; get; }

        [OnSerializing]
        void OnSerializing(StreamingContext context)
        {
            this.IssuedAtSerializable = this.IssuedAt.ToString("yyyy/MM/dd HH:mm:ss zzz");
        }
    }
}
