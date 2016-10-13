using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            var level = File.ReadAllLines("level0.txt").ToList();
            var levelsfiles = new List<string> { "level1.txt", "level2.txt", "level3.txt" };
            var levels = levelsfiles.Select(e => File.ReadAllLines(e).ToList()).ToList();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var game = new Game(level, levels);
            var menu = new MenuForm();
            Application.Run(menu);
            if (menu.ReturnValue == 1) Application.Run(new GameForm(game));
        }
    }
}