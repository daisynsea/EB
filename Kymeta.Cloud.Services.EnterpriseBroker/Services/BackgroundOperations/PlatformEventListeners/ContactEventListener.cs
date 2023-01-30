using CometD.NetCore.Bayeux.Client;
using CometD.NetCore.Bayeux;
using Newtonsoft.Json;
using Kymeta.Cloud.Services.EnterpriseBroker.Models.Salesforce.External.PlatformEvents;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Services.BackgroundOperations.PlatformEventListeners
{
    class ContactEventListener : IMessageListener
    {
        /// <summary>
        /// Listen for messages from Salesforce Platform Events for Contacts
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="message"></param>
        public void OnMessage(IClientSessionChannel channel, IMessage message)
        {
            // fetch the JSON
            var convertedJson = message.Json;
            // deserialize JSON into C# model
            var obj = JsonConvert.DeserializeObject<ContactEventRoot>(convertedJson);
            // write to console for demonstration purposes
            Console.WriteLine(convertedJson);
            Console.WriteLine($"Message received ({obj?.Data.Payload.KCS_Contact_Id__c}) - Name: {obj?.Data.Payload.KCS_Contact_FirstName__c} {obj?.Data.Payload.KCS_Contact_LastName__c} [{obj?.Data.Payload.KCS_Contact_Email__c}]");
        }
    }
}
