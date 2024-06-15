using Dapr.Client;
using System.Text;
using System.Text.Json;

namespace Common;

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

    public static Either<Error, T> Match<T>(
        this BindingResponse response, Func<JsonDocument[][], T> some)
    {
        JsonDocument[][] docs = [];

        if (response.Request.Operation == "query")
        {
            try
            {
                var content = Encoding.UTF8.GetString(response.Data.Span);
                docs = JsonSerializer.Deserialize<JsonDocument[][]>(content) ?? [];
            }
            catch (Exception ex)
            {
                return Error.New(ex);
            }
        }

        return some(docs);
    }
}
