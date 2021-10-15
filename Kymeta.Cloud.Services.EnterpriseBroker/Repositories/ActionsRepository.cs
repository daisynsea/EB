using Dapper;
using Kymeta.Cloud.Services.EnterpriseBroker.Models.Salesforce;
using System.Data;
using System.Data.SqlClient;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Repositories;

public interface IActionsRepository
{
    Task<bool> InsertActionRecord(SalesforceActionRecord model);
    Task<IEnumerable<SalesforceActionRecord>> GetActionRecords();
    Task<bool> UpdateActionRecord(SalesforceActionRecord model);
}

public class ActionsRepository : IActionsRepository
{
    private readonly string _connString;

    public ActionsRepository(IConfiguration configuration)
    {
        _connString = configuration.GetConnectionString("ServicesConnection");
    }

    public async Task<IEnumerable<SalesforceActionRecord>> GetActionRecords()
    {
        using var db = new SqlConnection(_connString);
        var result = await db.QueryAsync<SalesforceActionRecord>("EnterpriseActionsGet", commandType: CommandType.StoredProcedure);
        return result;
    }

    public async Task<bool> InsertActionRecord(SalesforceActionRecord model)
    {
        using var db = new SqlConnection(_connString);
        var result = await db.ExecuteAsync("EnterpriseActionsAdd", model, commandType: CommandType.StoredProcedure);
        return result > 0;
    }

    public async Task<bool> UpdateActionRecord(SalesforceActionRecord model)
    {
        using var db = new SqlConnection(_connString);
        var result = await db.ExecuteAsync("EnterpriseActionsUpdate", new
        {
            model.Id,
            model.OssStatus,
            model.OracleStatus,
            model.LastUpdatedOn,
            model.OracleErrorMessage,
            model.OssErrorMessage
        }, commandType: CommandType.StoredProcedure);
        return result > 0;
    }
}
