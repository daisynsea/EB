using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Pages
{
    [Authorize]
    [ExcludeFromCodeCoverage]
    public class DetailModel : PageModel
    {
        private JsonSerializerOptions _serializerOptions = new JsonSerializerOptions { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };
        private IActionsRepository _actionsRepository;
        public SalesforceActionTransaction SalesforceActionTransaction { get; set; }
        public SalesforceActionObject? SalesforceActionObject { get; set; }
        public string TimePreference = "utc";
        public string SerializedTransaction
        {
            get
            {
                string transaction = String.Empty;
                switch (SalesforceActionTransaction.Object)
                {
                    case ActionObjectType.Account:
                        transaction = JsonSerializer.Serialize(JsonSerializer.Deserialize<SalesforceAccountModel>(SalesforceActionTransaction.SerializedObjectValues), _serializerOptions);
                        break;
                    case ActionObjectType.Address:
                        transaction = JsonSerializer.Serialize(JsonSerializer.Deserialize<SalesforceAddressModel>(SalesforceActionTransaction.SerializedObjectValues), _serializerOptions);
                        break;
                    case ActionObjectType.Contact:
                        transaction = JsonSerializer.Serialize(JsonSerializer.Deserialize<SalesforceContactModel>(SalesforceActionTransaction.SerializedObjectValues), _serializerOptions);
                        break;
                    default:
                        break;
                }
                return transaction;
            }
        }
        public string SerializedResponse => JsonSerializer.Serialize(SalesforceActionTransaction.Response, _serializerOptions);
        public string SalesforceUrl { get; set; }

        public DetailModel(IActionsRepository actionsRepository)
        {
            _serializerOptions.Converters.Add(new JsonStringEnumConverter());
            _actionsRepository = actionsRepository;
        }

        public async Task OnGet(Guid id, string objectType)
        {
            TimePreference = HttpContext.Request.Cookies["timePreference"] ?? "utc";

            SalesforceActionTransaction = await _actionsRepository.GetActionRecord(id, objectType); // Get the entire transaction record
            // transform timestamps depending on preference
            if (TimePreference == "local")
            {
                SalesforceActionTransaction.CreatedOn = SalesforceActionTransaction.CreatedOn.ToLocalTime();
                foreach (var ac in SalesforceActionTransaction.TransactionLog)
                {
                    ac.Timestamp = ac.Timestamp.GetValueOrDefault().ToLocalTime();
                }
            }

            SalesforceActionObject = JsonSerializer.Deserialize<SalesforceActionObject>(SalesforceActionTransaction.SerializedObjectValues); // Parse out just the incoming payload generic fields
            SalesforceUrl = $"{SalesforceActionObject.EnterpriseOriginUri}/lightning/r/{SalesforceActionTransaction.Object}/{SalesforceActionTransaction.ObjectId}/view"; // Build the url
        }
    }
}
