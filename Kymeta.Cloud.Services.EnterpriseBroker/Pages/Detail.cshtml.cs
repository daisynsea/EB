using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Pages
{
    public class DetailModel : PageModel
    {
        private IActionsRepository _actionsRepository;
        public SalesforceActionTransaction SalesforceActionTransaction { get; set; }

        public DetailModel(IActionsRepository actionsRepository)
        {
            _actionsRepository = actionsRepository;
        }

        public async Task OnGet(Guid id, string action)
        {
            SalesforceActionTransaction = await _actionsRepository.GetActionRecord(id, action);
        }
    }
}
