using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Pages
{
    public class DetailModel : PageModel
    {
        private JsonSerializerOptions _serializerOptions = new JsonSerializerOptions { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        private IActionsRepository _actionsRepository;
        public SalesforceActionTransaction SalesforceActionTransaction { get; set; }
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

        public DetailModel(IActionsRepository actionsRepository)
        {
            _serializerOptions.Converters.Add(new JsonStringEnumConverter());
            _actionsRepository = actionsRepository;
        }

        public async Task OnGet(Guid id, string objectType)
        {
            SalesforceActionTransaction = await _actionsRepository.GetActionRecord(id, objectType);
        }
    }
}
