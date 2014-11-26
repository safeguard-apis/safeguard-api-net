using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SafeGuardServerAPIV2
{
    [DataContract(Name = "flight_group")]
    public class FlightGroup
    {
        public FlightGroup(String Origin, String Destination, DateTime DepartureAt, DateTime ArrivalAt) {
            this.Origin = Origin;
            this.Destination = Destination;
            this.DepartureAt = DepartureAt;
            this.ArrivalAt = ArrivalAt;
        }

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


        [OnSerializing]
        void OnSerializing(StreamingContext context)
        {
            this.DepartureAtSerializable = this.DepartureAt.ToString("yyyy/MM/dd HH:mm:ss zzz");
            this.ArrivalAtSerializable = this.ArrivalAt.ToString("yyyy/MM/dd HH:mm:ss zzz");
        }

    }

        
}
