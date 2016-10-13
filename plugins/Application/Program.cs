using System;
using System.Linq;
using System.IO;
using Framework;
using System.Reflection;
using System.Collections.Generic; 

namespace Application
{
	public class MainClass
	{
		public static List<IPlugin> GetPlugins(string[] dllfiles)
		{
			var plugins = new List<IPlugin> ();
		    if (!dllfiles.All(File.Exists))
                throw new ArgumentException();
			foreach (var myAssembly in dllfiles.Select(Assembly.LoadFile))
			    plugins.AddRange(
			        myAssembly.GetTypes()
			            .Where(x => x.GetInterface(nameof(IPlugin)) != null)
			            .Select(Activator.CreateInstance).Cast<IPlugin>()
			    );
			return plugins;
		}

		public static string[] GetDllFiles(string directory)
		{
		    if (directory == "")
				directory = ".";
		    return Directory.Exists(directory) 
                ? Directory.GetFiles (directory, "*.dll").Select(Path.GetFullPath).ToArray()
                : new string[0];
		}

	    public static void Main (string[] args)
		{
			var files = GetDllFiles ("Solution");
			var plugins = GetPlugins (files);
			foreach (var e in plugins)
				Console.WriteLine(e.Name);
		}
	}
}
