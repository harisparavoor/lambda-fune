using Amazon.Lambda.Core;

// Tells Lambda how to serialize input/output
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace EnvVarLambda;

public class Function
{
    // Handler: LambdaTest::EnvVarLambda.Function::FunctionHandler
    public string FunctionHandler(object input, ILambdaContext context)
    {
        string personnelBatchSize    = Environment.GetEnvironmentVariable("PERSONNEL_BATCH_SIZE")    ?? "(not set)";
        string processorFunctionName = Environment.GetEnvironmentVariable("PROCESSOR_FUNCTION_NAME") ?? "(not set)";
        string purgeCutoffDays       = Environment.GetEnvironmentVariable("PURGE_CUTOFF_DAYS")       ?? "(not set)";

        var result =
            $"PERSONNEL_BATCH_SIZE    = {personnelBatchSize}{Environment.NewLine}" +
            $"PROCESSOR_FUNCTION_NAME = {processorFunctionName}{Environment.NewLine}" +
            $"PURGE_CUTOFF_DAYS       = {purgeCutoffDays}";

        // Shows up in CloudWatch Logs
        context.Logger.LogInformation(result);

        return result;
    }
}