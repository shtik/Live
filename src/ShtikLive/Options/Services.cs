namespace ShtikLive.Options
{
    public class Services
    {
        public ServiceInfo Shows { get; set; }
        public ServiceInfo Notes { get; set; }
        public ServiceInfo Questions { get; set; }
    }

    public class ServiceInfo
    {
        public string BaseUrl { get; set; }
    }
}