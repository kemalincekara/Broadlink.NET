## C# .NET Control for Broadlink RF/IR Controllers

.Net library for Broadlink devices.

Currently only supports RM devices (Tested with Broadlink RM Pro)

### Build requirements
 * Visual Studio 2017
 * .NET Framework 4.6.1

### Features
 * Asynchronous
 * Setup Wifi in AP Mode
 * Scan devices on the local network
 * Get the temperature
 * IR or RF learning mode
 * OneClick, send commands with ID parameters from the desktop shortcut.
 * Import data from Broadlink eControl App

Usage OneClick
```
Broadlink.OneClick.exe salon-avize-lamba televizyon-vol+ ...
```
### Dependencies
 * [Newtonsoft.Json](https://www.nuget.org/packages/Newtonsoft.Json)
 * [System.ValueTuple](https://www.nuget.org/packages/System.ValueTuple)

### Thanks
 * https://github.com/wind-rider/broadlink-dotnet
 * https://github.com/mjg59/python-broadlink
 * https://github.com/lprhodes/broadlinkjs-rm/
 * https://github.com/rdavisau/sockets-for-pcl