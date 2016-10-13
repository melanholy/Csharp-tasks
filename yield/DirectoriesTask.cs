using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace yield
{
	public class FileData
	{
		public string FileExtension;
		public long Size;
	}

	public static class DirectoriesTask
	{
        //public static bool IsAccesible(this DirectoryInfo file)
        //{
        //    try
        //    {
        //        file.GetFiles();
        //    }
        //    catch
        //    {
        //        Console.WriteLine(file.FullName);
        //        return false;
        //    }
        //    return true;
        //}

		public static IEnumerable<FileData> EnumerateAllFiles(DirectoryInfo directoryInfo, Random random)
		{
            //var d = directoryInfo.EnumerateDirectories("App*", SearchOption.AllDirectories).Where(x => x.IsAccesible());
            //foreach (var e in d)
            //{
            //    foreach (var es in e.GetFiles())
            //        yield return new FileData { FileExtension = es.Extension, Size = es.Length };
            //}
            //yield break;
            foreach (var e in directoryInfo.GetFiles().Shuffle(random))
                yield return new FileData { FileExtension = e.Extension, Size = e.Length};
            foreach (var g in directoryInfo.GetDirectories().Shuffle(random))
            {
                try
                {
                    g.GetFiles();
                }
                catch
                {
                    continue;
                }
                foreach (var yu in EnumerateAllFiles(g, random))
                    yield return yu;
            }
            yield break;
		}

		public static IEnumerable<T> Shuffle<T>(this T[] items, Random rand)
		{
            for (var i = 0; i < items.Length; i++)
            {
                var c = rand.Next(items.Length);
                var a = items[c];
                items[c] = items[i];
                items[i] = a;
            }
            return items;
		}
	}
}