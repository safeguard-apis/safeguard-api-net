using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SafeGuardServerAPIV2
{
    public class SafeGuardException: Exception
    {
            public SafeGuardException(String message){
                this.Message = message;
            }

            public SafeGuardException(String message, decimal errorCode)
            {
                this.Message = message;
                this.ErrorCode = errorCode;
            }

            public String Message { set; get; }
            public decimal ErrorCode { set; get; }

    }
}
