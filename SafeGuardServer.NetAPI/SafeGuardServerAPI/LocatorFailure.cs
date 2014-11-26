using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SafeGuardServerAPIV2
{
    [DataContract(Name = "errors")]
    public class LocatorFailure
    {
        public enum Failures { PAYMENT = 1, EXPIRED = 2, UNKNOWN = 99}

        public LocatorFailure(String locId, String errorMessage) {
            this.Loc = locId;
            this.ErrorMessage = errorMessage;
            this.ErrorCode = Failures.UNKNOWN;
        }

        public LocatorFailure(String locId, String errorMessage, Failures failureCode)
        {
            this.Loc = locId;
            this.ErrorMessage = errorMessage;
            this.ErrorCode = failureCode;
        }

        public LocatorFailure(Locator locator, String errorMessage)
        {
            this.locator = locator;
            this.ErrorMessage = errorMessage;
            this.ErrorCode = Failures.UNKNOWN;
        }

        public LocatorFailure(Locator locator, String errorMessage, Failures failureCode)
        {
            this.locator = locator;
            this.ErrorMessage = errorMessage;
            this.ErrorCode = failureCode;
        }

        [DataMember(Name = "loc")]
        String Loc { set; get; }

        [DataMember(Name = "locator")]
        Locator locator { set; get; }

        [DataMember(Name = "error_message")]
        String ErrorMessage { set; get; }


        [DataMember(Name = "error_code")]
        Failures ErrorCode { set; get; }
    }
}
