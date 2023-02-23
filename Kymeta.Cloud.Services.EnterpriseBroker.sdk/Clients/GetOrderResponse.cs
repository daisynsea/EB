﻿using Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle.REST;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Clients
{
    public class GetOrderResponse : IOracleResponsePayload
    {
        public long HeaderId { get; set; }
        public string OrderNumber { get; set; }
        public string SourceTransactionNumber { get; set; }
        public string SourceTransactionSystem { get; set; }
        public string SourceTransactionId { get; set; }
        public long BusinessUnitId { get; set; }
        public string BusinessUnitName { get; set; }

        public bool IsSuccessfulResponse()
        {
            return !(HeaderId == 0 && OrderNumber is null && SourceTransactionNumber is null && SourceTransactionSystem is null && SourceTransactionId is null
                && BusinessUnitId == 0 && BusinessUnitName is null);
        }
    }

}