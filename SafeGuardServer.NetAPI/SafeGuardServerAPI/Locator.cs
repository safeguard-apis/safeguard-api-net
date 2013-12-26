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
        public Locator(String locator, String source, String destination, String origin, DateTime departureAt, DateTime arrivalAt, Boolean isInternational, List<Ticket> tickets)
        {
            this.Loc = locator;
            this.Source = source;
            this.Destination = destination;
            this.Origin = origin;
            this.DepartureAt = departureAt;
            this.ArrivalAt = arrivalAt;
            this.IsInternational = isInternational;
            this.Tickets = tickets;
        }

        [DataMember(Name = "loc")]
        String Loc{set; get;}

        [DataMember(Name = "source")]
        String Source{set; get;}

        [DataMember(Name = "destination")]
        String Destination { set; get; }

        [DataMember(Name = "origin")]
        String Origin { set; get; }

        [DataMember(Name = "departure_at")]
        private String DepartureAtSerializable { set; get; }

        DateTime DepartureAt { set; get; }

        [DataMember(Name = "arrival_at")]
        private String ArrivalAtSerializable { set; get; }

        DateTime ArrivalAt { set; get; }

        [DataMember(Name = "is_international")]
        Boolean IsInternational { set; get; }

        [DataMember(Name = "tickets_attributes")]
        List<Ticket> Tickets { set; get; }

        [OnSerializing]
        void OnSerializing(StreamingContext context)
        {
            this.DepartureAtSerializable = this.DepartureAt.ToString("yyyy/MM/dd HH:mm:ss zzz");
            this.ArrivalAtSerializable = this.ArrivalAt.ToString("yyyy/MM/dd HH:mm:ss zzz");
        }
    }
}
