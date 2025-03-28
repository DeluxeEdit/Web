using VSConfiguratonHelper;

namespace Web
{
    public class SettingsFileSelector
    {
        public string StandardFileName { get; set; } = "appsettings.json";
        public string ProductionFileName { get; set; } = "appsettings.Production.json";


        public string WantedFileName { get; set; } = String.Empty;
        public SettingsFileSelector()
        {
            var helper = new ConfiguratonHelper();
            helper.SolutionBasePath = "C:\\gitroot\\personal\\Web";
            string active=helper.ActiveConfiguration;
            if (active.StartsWith("Production"))
                WantedFileName = ProductionFileName;
            else 
                WantedFileName = StandardFileName;


            }
        }
}
