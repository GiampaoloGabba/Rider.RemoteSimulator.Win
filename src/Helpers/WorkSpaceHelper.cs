using System.Linq;
using System.Xml.Linq;

namespace Rider.RemoteSimulator.Win.Helpers
{
    public static class WorkSpaceHelper
    {
        public static string GetSimulatorName(string workspace)
        {
            var xml = XDocument.Load(workspace);

            //CODE FOR NEW RIDER VERSION (2019.3)
            var component = xml.Root?.Elements("component").FirstOrDefault(x=>x.Attribute("name")?.Value == "PropertiesComponent");
            var simulator = component?.Elements("property").FirstOrDefault(x=>x.Attribute("name")?.Value.Contains("XamarinIOSSimulatorKind") == true);
            return simulator?.Attribute("value")?.Value;


            //CODE FOR OLD RIDER VERSION  (2019.2):

            //Get the selected configuration from the RunManager component
            /*var component = xml.Root?.Elements("component").FirstOrDefault(x=>x.Attribute("name")?.Value == "RunManager");
            var selected  = component?.Attribute("selected")?.Value;

            //Calculate the node to read for the current configuration
            var simulatorNode = component?.Descendants()
                .FirstOrDefault(x => selected == x.Attribute("factoryName")?.Value + "." + x.Attribute("name")?.Value);

            //Read the selected simulator
            var simulator = simulatorNode?.Descendants()
                .FirstOrDefault(x => x.Attribute("name")?.Value == "DEFAULT_IPHONE_SIMULATOR");

            return simulator?.Attribute("value")?.Value;*/
        }

    }
}
