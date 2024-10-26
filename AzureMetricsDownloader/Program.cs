// See https://aka.ms/new-console-template for more information
using System.Text;
using Azure.Identity;
using Azure.Monitor.Query;
using Azure.Monitor.Query.Models;

Console.WriteLine("Starting....");
var sb = new StringBuilder();
var metricsQueryClient = new MetricsQueryClient(new DefaultAzureCredential());
var resourceId = $"/subscriptions/0af06968-920a-43d9-9931-cfe593e9a041/resourceGroups/sample-app-gateway/providers/Microsoft.Network/applicationGateways/lpm-t-agw-mf01";
var metrics = new[] { "TotalRequests" };
var results = await metricsQueryClient.QueryResourceAsync(
      resourceId,
      metrics,
      new MetricsQueryOptions()
      {
          Aggregations = { MetricAggregationType.Total },
          // Filter = "EntityName eq '*'",//this is the same as a split in the UI
          TimeRange = new QueryTimeRange(
                    new DateTimeOffset(2024, 10, 23, 11, 0, 0, TimeSpan.Zero),
                    new DateTimeOffset(2024, 10, 23, 15, 0, 0, TimeSpan.Zero))
      }
  );

foreach (var metric in results.Value.Metrics)
{
    foreach (var element in metric.TimeSeries)
    {
        foreach (var metricValue in element.Values)
        {
            sb.AppendLine(
                $"{metricValue.TimeStamp.ToString("yyyy-MM-dd HH:mm:ss.fff")},{metric.Name},{metricValue.Total}");
        }
    }
}

// Instead of returning, print the result
Console.WriteLine(sb.ToString());