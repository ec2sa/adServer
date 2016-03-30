using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebServiceADContentProvider
{
    public class GetMultimediaObject_Request
    {
			public int ID { get; set; }

			public string Data0 { get; set; }

			public string Data1 { get; set; }

			public string Data2 { get; set; }

			public string Data3 { get; set; }

			public string SessionId { get; set; }

			public int RequestSource { get; set; }

			public DateTime RequestDate { get; set; }
		
    }
}