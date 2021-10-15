namespace Kymeta.Cloud.Services.EnterpriseBroker.Services;

public static class Helpers
{
    public static TV GetValue<TK, TV>(this IDictionary<TK, TV> dict, TK key, TV defaultValue = default(TV))
    {
        return dict.TryGetValue(key, out TV value) ? value : defaultValue;
    }
}