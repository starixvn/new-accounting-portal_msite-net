namespace Maintenance.Letstalk.Params;

using System;
using System.Collections.Generic;
using Model;

public class HttpResendMultipleParams
{
    public DateTime FromDate { get; set; }

    public DateTime ToDate { get; set; }

    public IEnumerable<ResendLetstalkModel> Data { get; set; }
}