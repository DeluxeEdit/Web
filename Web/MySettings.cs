using Constants;
namespace Web
{
    public class MySettings
    {
        public AppInfo AppInfo { get; set; } = SystemConstants.GetAppInfo();
        public string CurrentEnvironment { get; } = String.Empty; 
        public MySettings()
        {
            var sel = new SettingsFileSelector();

            var built = WebApplication.CreateBuilder().Configuration.AddJsonFile(sel.WantedFileName).Build();


            CurrentEnvironment = built.GetValue("CurrentEnvironment", "Debug");

                var parsed = Enum.Parse<AppEnvironment>(CurrentEnvironment);
                var appInfo = SystemConstants.GetAppInfo();
                appInfo.Environment = parsed;
                AppInfo = appInfo;

        }

    }
}