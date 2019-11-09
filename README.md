## Connect JetBrains Rider with Xamarin Remoted iOS Simulator 
Enable JetBrains Rider to work with [Xamarin Remoted iOS Simulator for windows](https://docs.microsoft.com/en-us/xamarin/tools/ios-simulator/), installed with Microsoft Visual Studio.

### Usage:

- Make sure to have working, established a connection to your mac with the Xamarin Mac Agent 
- Edit the `Rider.RemoteSimulator.Win.exe.config` file with the connection details for your mac (host name, username and password)
- Open JetBrains Rider
- Create a new **External tool**: `Settings --> Tools --> External tools`
- In **Tools Settings** select the executable `Rider.RemoteSimulator.Win.exe`
- In **Arguments** insert this: `$ModuleFileDir$`
- Now in your run configuration for iOS simulator, you can add this external tool in **Before Launch** settings
- Debug and enjoy the Remote Simulator :)

### How it works

When started, this tool execute these step:

1) Read the `workspace.xml` (for current solution) to retrieve the selected simulator in your run configuration
2) Connects to your mac host with the settings provided in `Rider.RemoteSimulator.Win.exe.config`
3) Retrieve the simulators list from xcode to get the right UDID for the selected simulator
4) Launch the Windows Remote Simulator from command line, passing the right UDID

### FAQ:

1) *Why an external tool and not a plugin?*

Well...  currently i dont have time to learn how to develop a plugin for Rider, so i made a simple console application to use as an external tool :)

2) *Whaaat??? A plain password in the .config file?*

Yeah, i know... i know... But see the previous FAQ: i dont have much time right now so i had to do things super-fast. If you dont like this auth method, a pull request will be super-welcome!


