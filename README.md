# Azure Metrics Downloader
Azure Resources Metrics are generated automaticlly and for free stored for 30-90 days. Using this small software you can download them and store in Storage Account or in One Drive. Latter you can analise it using Excel or PowerBI. Metrics can be processor , disk utylisation etc.

## Usage Scenario 1
```
AzureMetricsDownloader.exe <<ApplicationId>> <<Secret>> <<TenantId>> <<SubscryptionID>> <<ResourceGroup>> <<ResourceName>> <<MetricName>> <<ExportFilename>> <<StartDateTime>> <<StopDateTime>>
```
e.g.
```
AzureMetricsDownloader.exe e9xxx49 Uqxxxxm 42xxxef 0axxx041 sample-app-gateway lpm-t-agw-mf01 TotalRequests export.csv 2024-10-23_11-00 2024-10-23_15-00
```

This export TotalRequests metric from resource lpm-t-agw-mf01 in sample-app-gateway resourcegroup in 0axxx041 subscryption. Time Range 2024-10-23 11:00 to 2024-10-23 15:00. e9xxx49 it is Application ID, with Uqxxxxm Secret in 42xxxef Tenant. Application must have Reader permission on resource that you want export the Metrics.

## Usage Scenario 2

If you skip STartDataTime and StopData Time it will export last full hour Metrics. If we run it 11:10 it will export Metric from 10:00:00 to 10:59:59. If we run it 11:59 it will export Metric from 10:00:00 to 10:59:59. It could be usefull to continiously export the metric.
```
AzureMetricsDownloader.exe <<ApplicationId>> <<Secret>> <<TenantId>> <<SubscryptionID>> <<ResourceGroup>> <<ResourceName>> <<MetricName>> <<ExportFilename>> <<StartDateTime>> <<StopDateTime>>
```
e.g.
```
AzureMetricsDownloader.exe e9xxx49 Uqxxxxm 42xxxef 0axxx041 sample-app-gateway lpm-t-agw-mf01 TotalRequests export.csv
```

## Download
You can Download the Software from Artefact from last succesfull build: https://github.com/MariuszFerdyn/Azure-Metrics-Downloader/actions/workflows/01-build.yml Just select last green successful build and at the bottom of screen you have artefacts.

## Build yourself
You can download the sourcecode and open Solution in Visual Studio.

You can also use this github actioin: https://github.com/MariuszFerdyn/Azure-Metrics-Downloader/blob/master/.github/workflows/01-build.yml

## Run in pipelinee
Here is how you can run it automatically in pipeline (github action): https://github.com/MariuszFerdyn/Azure-Metrics-Downloader/blob/master/.github/workflows/02-run.yml <br>
It can be usefull for automatically run it every hour and export metrics.
