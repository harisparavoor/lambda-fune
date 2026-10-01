using Amazon.Lambda.Core;

// Tells Lambda how to serialize input/output
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace EnvVarLambda;

public class Function
{
    // Handler: EnvVarLambda::EnvVarLambda.Function::FunctionHandler
    public string FunctionHandler(object input, ILambdaContext context)
    {
        string pdfBucketName    = Environment.GetEnvironmentVariable("PDF_BUCKET_NAME")    ?? "(not set)";
        string reportDayOffset  = Environment.GetEnvironmentVariable("REPORT_DAY_OFFSET")  ?? "(not set)";
        string reportPeriodMode = Environment.GetEnvironmentVariable("REPORT_PERIOD_MODE") ?? "(not set)";
        string snsTopicArn      = Environment.GetEnvironmentVariable("SNS_TOPIC_ARN")      ?? "(not set)";

        var result =
            $"PDF_BUCKET_NAME    = {pdfBucketName}{Environment.NewLine}" +
            $"REPORT_DAY_OFFSET  = {reportDayOffset}{Environment.NewLine}" +
            $"REPORT_PERIOD_MODE = {reportPeriodMode}{Environment.NewLine}" +
            $"SNS_TOPIC_ARN      = {snsTopicArn}";

        // Shows up in CloudWatch Logs
        context.Logger.LogInformation(result);

        return result;
    }
}
