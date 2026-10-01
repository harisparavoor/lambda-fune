using System.Text.Json;
using Amazon.Lambda.Core;

// Tells Lambda how to serialize input/output
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace EnvVarLambda;

public class Function
{
    // Handler: LambdaTest::EnvVarLambda.Function::FunctionHandler
    //
    // Test event can be either:
    //   "PERSONNEL_BATCH_SIZE PROCESSOR_FUNCTION_NAME, PURGE_CUTOFF_DAYS"
    //   {"vars": "PERSONNEL_BATCH_SIZE PROCESSOR_FUNCTION_NAME, PURGE_CUTOFF_DAYS"}
    public string FunctionHandler(JsonElement input, ILambdaContext context)
    {
        string? text = null;

        if (input.ValueKind == JsonValueKind.String)
        {
            text = input.GetString();
        }
        else if (input.ValueKind == JsonValueKind.Object &&
                 input.TryGetProperty("vars", out var vars) &&
                 vars.ValueKind == JsonValueKind.String)
        {
            text = vars.GetString();
        }

        if (string.IsNullOrWhiteSpace(text))
        {
            return "Pass variable names in the test event, e.g. \"NAME1 NAME2, NAME3\" " +
                   "or {\"vars\": \"NAME1 NAME2, NAME3\"}";
        }

        var names = text.Split(new[] { ' ', ',', ';', '\t', '\n', '\r' },
                               StringSplitOptions.RemoveEmptyEntries);

        var lines = names.Select(name =>
            $"{name} = {Environment.GetEnvironmentVariable(name) ?? "(not set)"}");

        var result = string.Join(Environment.NewLine, lines);

        // Shows up in CloudWatch Logs
        context.Logger.LogInformation(result);

        return result;
    }
}