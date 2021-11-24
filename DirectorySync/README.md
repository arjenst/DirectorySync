New-Service -Name "YourServiceName" -BinaryPathName <yourproject>.exe# DirectorySync

Simple service for Windows that synchronizes the content of configured directories.

## Building

Build the solution in Visual Studio.

## Installation

Run a PowerShell terminal as Administrator and run the following command:

```powershell
New-Service -Name "DirectorySync" -BinaryPathName "<yourproject>/DirectorySync.exe" -StartupType Automatic
```
## Usage

Open the DirectorySync.exe.config in <yourproject> in your preferred editor and configure the paths to synchronize.

```xml
<directorySync>
  <directories>
    <add name="<name>" source="<path to source directory>" destination="<path to destination directory>" />
    <!-- Repeat the line above to synchronize multiple directories. -->
  </directories>
</directorySync>
```

Once configured the service, you can start the service.
```powershell
Start-Service DirectorySync
```
Or restart it if it was already running.
```powershell
Restart-Service DirectorySync
```

## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

Please make sure to update tests as appropriate.

## License
[MIT](https://choosealicense.com/licenses/mit/)