# Azure Metrics Downloader
Azure Resources Metrics are generated automatically and for free stored for 30-90 days. Using this small software, you can download them and store them in Storage Account or in One Drive. Later you can analyze it using Excel or PowerBI. Metrics can be processor, disk utilization etc.

## Usage Scenario 1
```
AzureMetricsDownloader.exe <<ApplicationId>> <<Secret>> <<TenantId>> <<FullResourceID>> <<MetricName>> <<ExportFilename>> <<Agregation>> <<StartDateTime>> <<StopDateTime>>
```
e.g.
```
AzureMetricsDownloader.exe e9xxx49 Uqxxxxm 42xxxef 0axxx041 /subscriptions/xxx/resourceGroups/xxx/providers/Microsoft.Network/applicationGateways/xxx  TotalRequests export.csv Totals 2024-10-23_11-00 2024-10-23_15-00
```

This export TotalRequests metric from resource id /subscriptions/xxx/resourceGroups/xxx/providers/Microsoft.Network/applicationGateways/xxx. Time Range 2024-10-23 11:00 to 2024-10-23 15:00. e9xxx49 it is Application ID, with Uqxxxxm Secret in 42xxxef Tenant. Application must have Reader permission on resource that you want export the Metrics. TotalRequests is the metric to gather and Totals is aggregation.

## Usage Scenario 2

If you skip StartDataTime and StopDataTime it will export last full hour Metrics. If we run it at 11:10 it will export Metric from 10:00:00 to 10:59:59. If we run it 11:59 it will export Metric from 10:00:00 to 10:59:59. It could be useful to continuously export the metric.
```
AzureMetricsDownloader.exe <<ApplicationId>> <<Secret>> <<TenantId>> <<FullResourceID>> <<MetricName>> <<ExportFilename>> <<Agregation>>
```
e.g.
```
AzureMetricsDownloader.exe e9xxx49 Uqxxxxm 42xxxef 0axxx041 /subscriptions/xxx/resourceGroups/xxx/providers/Microsoft.Network/applicationGateways/xxx  TotalRequests export.csv Totals
```
## Notes
Do not export more than 10h in one shot, you can lose some logs.

## Download
You can Download the Software from Artefact from last successful build: https://github.com/MariuszFerdyn/Azure-Metrics-Downloader/actions/workflows/01-build.yml Just select last green successful build and at the bottom of screen you have artefacts.

## Build yourself
You can download the source code and open Solution in Visual Studio.

You can also use this GitHub action: https://github.com/MariuszFerdyn/Azure-Metrics-Downloader/blob/master/.github/workflows/01-build.yml

## Run in pipeline
Here is how you can run it automatically in pipeline (GitHub action): https://github.com/MariuszFerdyn/Azure-Metrics-Downloader/blob/master/.github/workflows/02-run.yml <br>
It can be useful to automatically run it every hour and export metrics in additional tasks.

## Future
If you want to export all Metrics from provided resource, please see my other repository named **Azure-Metrics-Downloader-PowerShell**
