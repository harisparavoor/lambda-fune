using Amazon.Lambda.Core;

// Tells Lambda how to serialize input/output
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace EnvVarLambda;

public class Function
{
    // Handler: LambdaTest::EnvVarLambda.Function::FunctionHandler
    //
    // Test event = one JSON string with variable names separated by space and/or comma:
    //   "PERSONNEL_BATCH_SIZE PROCESSOR_FUNCTION_NAME, PURGE_CUTOFF_DAYS"
    public string FunctionHandler(string input, ILambdaContext context)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return "Pass variable names in the test event, e.g. \"hari NAME2, NAME3\"";
        }

        var names = input.Split(new[] { ' ', ',', ';', '\t', '\n', '\r' },
                                StringSplitOptions.RemoveEmptyEntries);

        var lines = names.Select(name =>
            $"{name} = {Environment.GetEnvironmentVariable(name) ?? "(not set)"}");

        var result = string.Join(Environment.NewLine, lines);

        // Shows up in CloudWatch Logs
        context.Logger.LogInformation(result);

        return result;
    }
}