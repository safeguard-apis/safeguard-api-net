using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SafeGuardServerAPI
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

            String Message { set; get; }
            decimal ErrorCode { set; get; }

    }
}
