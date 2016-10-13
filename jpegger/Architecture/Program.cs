using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Digger
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            var menu = new JpeggerMenu();
            Application.Run(menu);
            var type = menu.Type;
            if (type == 1) Application.Run(new JpeggerGame());
            else 
            {
                var size = "12 12";
                var constructor = new JpeggerConstructor(int.Parse(size.Split(' ')[0]), int.Parse(size.Split(' ')[1]));
                Application.Run(constructor);
            }
        }
    }
}
