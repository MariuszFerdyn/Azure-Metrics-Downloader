using Azure.Monitor.Query.Models;
using Azure.Monitor.Query;
using System.Text;
using Azure.Identity;

namespace AAGMetrics
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var arguments = ParseArguments(args);

            var sb = new StringBuilder();
            var metricsQueryClient = new MetricsQueryClient(new ClientSecretCredential(arguments.TenantId, arguments.ApplicationId, arguments.Secret));
            var resourceId = $"/subscriptions/{arguments.SubscriptionId}/resourceGroups/{arguments.ResourceGroup}/providers/Microsoft.Network/applicationGateways/{arguments.ResourceName}";
            // var resourceId = $"/subscriptions/{arguments.SubscriptionId}/resourceGroups/{arguments.ResourceGroup}/providers/Microsoft.Network/{arguments.ResourceName}";

            var metrics = new[] { arguments.MetricName };


            var results = metricsQueryClient.QueryResourceAsync(
                  resourceId,
                  metrics,
                  new MetricsQueryOptions()
                  {
                      Aggregations = { GetAggregationByType(arguments.MetricTypeField) },
                      //Filter = "EntityName eq '*'",//this is the same as a split in the UI
                      TimeRange = new QueryTimeRange(arguments.StartDate, arguments.EndDate)
                  }
              ).GetAwaiter().GetResult();

            foreach (var metric in results.Value.Metrics)
            {
                foreach (var element in metric.TimeSeries)
                {
                    foreach (var metricValue in element.Values)
                    {
                        sb.AppendLine(
                            $"{metricValue.TimeStamp.ToString("yyyy-MM-dd HH:mm")},{GetValueByFieldName(metricValue, arguments.MetricTypeField) ?? 0}");
                    }
                }
            }

            string filename = arguments.ExportFilename;
            Console.WriteLine($"Exporting to file: {filename}");
            File.WriteAllText(filename, sb.ToString());
        }

        private static object GetValueByFieldName(MetricValue metricValue, string fieldName)
        {
            if (fieldName.ToLower() == "total") return metricValue.Total;

            return metricValue.Total;
        }

        private static MetricAggregationType GetAggregationByType(string metricTypeField)
        {
            if (metricTypeField.ToLower() == "total") return MetricAggregationType.Total;

            return MetricAggregationType.Total;
        }

        private static Arguments ParseArguments(string[] args)
        {
            try
            {
                var now = DateTime.UtcNow;
                Arguments a = new Arguments(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8],
args.Length > 9 ? ParseDateToDateTimeOffset(args[9]) : PreviousHour(now), args.Length > 9 ? ParseDateToDateTimeOffset(args[10]) : PreviousHour(now).AddHours(1));

                Console.WriteLine("Using arguments:");
                Console.WriteLine(a.ToString());
                return a;
            }
            catch
            {
                Console.WriteLine("Could not parse arguments");

                Console.WriteLine($"Sample usage:{System.AppDomain.CurrentDomain.FriendlyName} <applicationId:guid> <secret:string> <tenantId:guid> <subscriptionId:guid> <resouceName:string> <ResourceName:string>  <metricName:string> <exportFileName:string> <startDate:YYYY-MM-DD_HH-mm> <endDate:YYYY-MM-DD_HH-mm>");

                Console.WriteLine("Exiting...");

                Environment.Exit(1);

                return null;
            }
        }

        private static DateTime PreviousHour(DateTime input) => input.AddHours(-1).Date.AddHours(input.AddHours(-1).Hour);

        private static DateTimeOffset ParseDateToDateTimeOffset(string input)
        {
            try
            {
                var items = input.Replace("-", "_").Split("_");

                return new DateTimeOffset(int.Parse(items[0]), int.Parse(items[1]), int.Parse(items[2]), int.Parse(items[3]), int.Parse(items[4]), 0, TimeSpan.Zero);
            }
            catch
            {
                Console.WriteLine($"Could not parse {input}. Expected date format YYYY-MM-DD_HH-mm");
                throw;
            }
        }
        
        public record Arguments(string ApplicationId, string Secret, string TenantId, string SubscriptionId, string ResourceGroup, string ResourceName, string MetricName, string ExportFilename, string MetricTypeField, DateTimeOffset StartDate, DateTimeOffset EndDate);
    }
}
