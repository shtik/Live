namespace ShtikLive.Options
{
    public class ServiceOptions
    {
        public ServiceInfo Shows { get; set; }
        public ServiceInfo Notes { get; set; }
        public ServiceInfo Questions { get; set; }
        public ServiceInfo Slides { get; set; }
    }

    public class ServiceInfo
    {
        public string BaseUrl { get; set; }
    }
}