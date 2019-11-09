## Using JetBrains Rider with the iOS Remote Simulator 
Enable JetBrains Rider to work with the Remote iOS Simulator for windows, installed with Visual Studio.

### Usage:
Create a new **External tool** in JetBrains Rider:

```Settings --> Tools --> External tools```

- Make sure to have working, established a connection to your mac with the Xamarin Mac Agent 
- Edit the `Rider.RemoteSimulator.Win.exe.config` file with the connection details for your mac (host name, username and password)
- Open JetBrains Rider
- In **Tools Settings** select the executable `Rider.RemoteSimulator.Win.exe`
- In **Arguments** insert this: `$ModuleFileDir$`
- Now in your run configuration for iOS simulator, you can add this external tool in the **Before Launch** settings
- Debug and enjoy the Remote Simulator :)

### How it works

When started, this tool execute these step:

1) Read the `workspace.xml` (for current solution) to retrieve the selected simulator in your run configuration
2) Connects to your mac host with the settings provided in `Rider.RemoteSimulator.Win.exe.config`
3) Retrieve the simulators list from xcode to get the right UDID for the selected simulator
4) Launch the Windows Remote Simulator from command line, passing the right UDID
