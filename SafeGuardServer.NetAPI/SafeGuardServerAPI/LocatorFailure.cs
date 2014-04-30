using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SafeGuardServerAPI
{
    [DataContract(Name = "errors")]
    public class LocatorFailure
    {
        public enum Failures { PAYMENT = 1, EXPIRED = 2, UNKNOWN = 99}

        public LocatorFailure(String loc, String errorMessage) {
            this.Loc = loc;
            this.ErrorMessage = errorMessage;
            this.ErrorCode = Failures.UNKNOWN;
        }

        public LocatorFailure(String loc, String errorMessage, Failures failureCode)
        {
            this.Loc = loc;
            this.ErrorMessage = errorMessage;
            this.ErrorCode = failureCode;
        }

        [DataMember(Name = "loc")]
        String Loc { set; get; }

        [DataMember(Name = "error_message")]
        String ErrorMessage { set; get; }


        [DataMember(Name = "error_code")]
        Failures ErrorCode { set; get; }
    }
}
