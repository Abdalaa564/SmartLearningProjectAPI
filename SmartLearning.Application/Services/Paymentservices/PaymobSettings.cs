using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLearning.Application.Services.Paymentservices
{
    public class PaymobSettings
    {
        public string ApiKey { get; set; }
        public string IntegrationId { get; set; }
        public string IframeId { get; set; }
        public string HmacSecret { get; set; }
        public string ApiUrl { get; set; } = "https://accept.paymob.com/api";
    }
}
