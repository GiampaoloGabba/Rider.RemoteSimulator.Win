using System.Linq;
using System.Xml.Linq;

namespace Rider.RemoteSimulator.Win.Helpers
{
    public static class WorkSpaceHelper
    {
        public static string GetSimulatorName(string workspace)
        {
            var xml = XDocument.Load(workspace);
            var component = xml.Root?.Elements("component").FirstOrDefault(x=>x.Attribute("name")?.Value == "PropertiesComponent");
            var simulator = component?.Elements("property").LastOrDefault(x=>x.Attribute("name")?.Value.Contains("XamarinIOSSimulatorKind") == true);
            return simulator?.Attribute("value")?.Value;
        }

    }
}
