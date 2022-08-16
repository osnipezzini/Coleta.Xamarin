using Android.App;

using SOCore;

using System.Reflection;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
#if TKT
[assembly: AssemblyTitle("SOColeta - TKT")]
#else
[assembly: AssemblyTitle("SOColeta")]
#endif
[assembly: AssemblyDescription("Sistema para inventario de produtos")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("SOTech Sistemas")]
[assembly: AssemblyProduct("SOColeta.Android")]
[assembly: AssemblyCopyright("Copyright ©  2020")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: ComVisible(false)]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
[assembly: AssemblyVersion("1.1.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]
[assembly: Application(UsesCleartextTraffic = true)]

// Add some common permissions, these can be removed if not needed
[assembly: UsesPermission(Android.Manifest.Permission.Internet)]
[assembly: UsesPermission(Android.Manifest.Permission.WriteExternalStorage)]
[assembly: UsesPermission(Android.Manifest.Permission.Flashlight)]

[assembly: SOApplication(SOColeta.Module.AppId, AppName = SOColeta.Module.AppName)]