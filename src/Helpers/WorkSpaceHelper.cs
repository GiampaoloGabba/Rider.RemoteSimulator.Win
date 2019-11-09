using System.Linq;
using System.Xml.Linq;

namespace RemoteSimulator.Helpers
{
    public static class WorkSpaceHelper
    {
        public static string GetSimulatorName(string workspace)
        {
            var xml = XDocument.Load(workspace);

            //Get the selected configuration from the RunManager component
            var component = xml.Root?.Elements("component").FirstOrDefault(x=>x.Attribute("name")?.Value == "RunManager");
            var selected  = component?.Attribute("selected")?.Value;

            //Calculate the node to read for the current configuration
            var simulatorNode = component?.Descendants()
                .FirstOrDefault(x => selected == x.Attribute("factoryName")?.Value + "." + x.Attribute("name")?.Value);

            //Read the selected simulator
            var simulator = simulatorNode?.Descendants()
                .FirstOrDefault(x => x.Attribute("name")?.Value == "DEFAULT_IPHONE_SIMULATOR");

            return simulator?.Attribute("value")?.Value;
        }

    }
}
