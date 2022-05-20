namespace MeeyPage.Data.AppSetting
{
    public class ConfigServiceResult
    {
        public int code { get; set; }
        public bool success { get; set; }
        public string message { get; set; }
        public AppSettingsConfig data { get; set; }
    }

    public class GetConfigResult
    {
        public int code { get; set; }
        public bool success { get; set; }
        public string message { get; set; }
        public object data { get; set; }
    }

    /// <summary>
    /// app setting from config service
    /// </summary>
    public class AppSettingsConfig
    {
        public ConnectionStringsConfig ConnectionStrings { get; set; }
        public RedisCacheConfig RedisCache { get; set; }
        public ElasticApmConfig ElasticApm { get; set; }
        public SwaggerConfig Swagger { get; set; }
        public GatewayServiceConfig GatewayService { get; set; }
        public AppEnvConfig AppEnv { get; set; }
        public ReportConfig Report { get; set; }
        public FileServerConfig FileServer { get; set; }
        public MailServiceConfig MailService { get; set; }
        public PushServerConfig PushServer { get; set; }
        public EInvoiceConfig EInvoice { get; set; }
        public AdmissionWebConfig AdmissionWeb { get; set; }
    }

    public class PushServerConfig
    {
        public string Push { get; set; }
        public string Subcribe { get; set; }
        public string Notification { get; set; }
        public string ApiKey { get; set; }
    }
    public class EInvoiceConfig
    {
        public string Url { get; set; }
    }

    public class RedisCacheConfig
    {
        public bool UseRedisCache { get; set; }
        public string Address { get; set; }
    }

    public class ConnectionStringsConfig
    {
        public string DbConnection { get; set; }    
        public string DefaultSchema { get; set; }
    }

    public class ElasticApmConfig
    {
        public bool UseApm { get; set; }
        public string ServerUrls { get; set; }
        public decimal TransactionSampleRate { get; set; }
        public string CaptureBodyContentTypes { get; set; }
        public string ServiceName { get; set; }
    }

    public class SwaggerConfig
    {
        public bool UseSwagger { get; set; }
    }

    public class AppEnvConfig
    {
        public string Env { get; set; }
    }

    public class GatewayServiceConfig
    {
        public bool UseGateway { get; set; }
    }

    public class ReportConfig
    {
        public int CacheTimeInHour { get; set; }
    }

    public class FileServerConfig
    {
        public string UploadUrl { get; set; }
        public string DownloadUrl { get; set; }
        public int FileSize { get; set; }
    }

    public class MailServiceConfig
    {
        public string Url { get; set; }
        public int DelayInSecond { get; set; }
        public bool LogTrace { get; set; }
        public int MaxRetryTime { get; set; }
        public int MailsPerInteval { get; set; }
        //public List<HostConfig> HostConfigList { get; set; }
    }

    public class AdmissionWebConfig
    {
        public string AppName { get; set; }
        public string ApiUrl { get; set; }
        public string Province { get; set; }
        public string ProvinceName { get; set; }
        public string HotLine { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public bool CheckCaptcha { get; set; }
        public string StudentHealthUrl { get; set; }
    }
}
