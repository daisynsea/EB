using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Pages
{
    public class IndexModel : PageModel
    {
        private IActionsRepository _actionsRepository;
        public IEnumerable<SalesforceActionTransaction> ActionRecords = new List<SalesforceActionTransaction>();

        public IndexModel(IActionsRepository actionsRepository)
        {
            _actionsRepository = actionsRepository;
        }

        public async Task OnGet()
        {
            ActionRecords = await _actionsRepository.GetActionRecords();
        }
    }
}
