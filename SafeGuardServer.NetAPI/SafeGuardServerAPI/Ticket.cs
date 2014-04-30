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
        public Ticket(String number, DateTime issuedAt, String passenger, Decimal price, String Currency, List<FlightGroup> flightGroups)
        { 
            this.Number = number;
            this.IssuedAt = issuedAt;
            this.Passenger = passenger;
            this.Price = price;
            this.Currency = Currency;
            this.flightGroups = flightGroups;
        }

        public Ticket(String number, DateTime issuedAt, String passenger, Decimal price, String Currency)
        {
            this.Number = number;
            this.IssuedAt = issuedAt;
            this.Passenger = passenger;
            this.Price = price;
            this.Currency = Currency;
            this.flightGroups = new List<FlightGroup>();
        }


        public void AddFlightGroup(FlightGroup group){
            this.flightGroups.Add(group);
        }


        public void AddFlightGroup(String Origin, String Destination, DateTime DepartureAt, DateTime ArrivalAt)
        {
            this.flightGroups.Add(new FlightGroup(Origin, Destination, DepartureAt, ArrivalAt));
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

        [OnSerializing]
        void OnSerializing(StreamingContext context)
        {
            this.IssuedAtSerializable = this.IssuedAt.ToString("yyyy/MM/dd HH:mm:ss zzz");
        }
    }
}
