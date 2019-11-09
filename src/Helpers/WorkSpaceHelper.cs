using System.Linq;
using System.Xml.Linq;

namespace RemoteSimulator.Helpers
{
    public static class WorkSpaceHelper
    {
        public static string GetSimulatorName(string workspace)
        {
            var xml = XDocument.Load(workspace);

            var component = xml.Root?.Elements("component").FirstOrDefault(x=>x.Attribute("name")?.Value == "RunManager");
            var selected  = component?.Attribute("selected")?.Value;

            var simulatorNode = component?.Descendants()
                .FirstOrDefault(x => selected == x.Attribute("factoryName")?.Value + "." + x.Attribute("name")?.Value);

            var simulator = simulatorNode?.Descendants()
                .FirstOrDefault(x => x.Attribute("name")?.Value == "DEFAULT_IPHONE_SIMULATOR");

            return simulator?.Attribute("value")?.Value;
        }

    }
}
