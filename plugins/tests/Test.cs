using NUnit.Framework;
using System;
using Application;
using System.Linq;

namespace tests
{
	[TestFixture]
	public class Test
	{
		[Test]
		public void TestGetFiles ()
		{
			CollectionAssert.AreEquivalent(new string[0], MainClass.GetDllFiles ("dhdgfdfg"));
			CollectionAssert.AreEquivalent (
				new [] { "./tests.dll", "./nunit.framework.dll" },
				MainClass.GetDllFiles (".")
			);
		}

		[Test]
		public void TestGetPlugins()
		{
			Assert.Throws<ArgumentException>(() => MainClass.GetPlugins (new [] { "." }));
			Assert.AreEqual (1, MainClass.GetPlugins (new [] { "Plugin1.dll" }).Count());
		}
	}
}
