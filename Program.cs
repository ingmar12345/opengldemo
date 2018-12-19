using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace _3Dengine.Components
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            new Components.MainWindow().Run(60);
        }


    }
}
