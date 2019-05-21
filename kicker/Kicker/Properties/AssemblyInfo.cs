using System;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
[assembly: CLSCompliant(false)]

// Durch Festlegen von ComVisible auf "false" werden die Typen in dieser Assembly unsichtbar 
// für COM-Komponenten. Wenn Sie auf einen Typ in dieser Assembly von 
// COM zugreifen müssen, legen Sie das ComVisible-Attribut für diesen Typ auf "true" fest.
[assembly: ComVisible(false)]

// Die folgende GUID bestimmt die ID der Typbibliothek, wenn dieses Projekt für COM verfügbar gemacht wird
[assembly: Guid("a0bc0444-175e-46d2-b252-24061326e387")]

// Configure log4net using the .config file
// This will cause log4net to look for a configuration file
// called Kicker.exe.config in the application base
// directory (i.e. the directory containing Kicker.exe)
[assembly: log4net.Config.XmlConfigurator(Watch = true)]

