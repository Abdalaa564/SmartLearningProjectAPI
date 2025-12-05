using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SmartLearning.Application.Services.Paymentservices
{
    public class PaymobOrderResponse
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
    }
}
