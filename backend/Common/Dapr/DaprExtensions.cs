using Dapr.Client;
using System.Text;
using System.Text.Json;

namespace Common.Dapr;

public static class DaprExtensions
{
    public static BindingRequest AddSqlText(this BindingRequest request, string sqlText)
    {
        request.Metadata.Add("sql", sqlText);
        return request;
    }


    public static BindingRequest AddSqlParams(this BindingRequest request, params object?[] @params)
    {
        request.Metadata.Add("params", JsonSerializer.Serialize(@params));
        return request;
    }

    public static int GetAffectedRows(this BindingResponse response)
    {
        var rowAffected = response.Metadata.GetValueOrDefault("rows-affected", null);
        if (int.TryParse(rowAffected, out var n))
        {
            return n;
        }
        return 0;
    }

    public static T Match<T>(
        this BindingResponse response, Func<JsonDocument[][], T> mapping)
    {
        if (response is null)
        {
            throw new ArgumentNullException(nameof(response));
        }

        if (mapping is null)
        {
            throw new ArgumentNullException(nameof(mapping));
        }

        JsonDocument[][] docs = [];

        if (response.Request.Operation == "query")
        {
            var content = Encoding.UTF8.GetString(response.Data.Span);
            docs = JsonSerializer.Deserialize<JsonDocument[][]>(content) ?? [];
        }

        return mapping(docs);
    }
}
