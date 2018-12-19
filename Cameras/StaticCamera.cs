using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;

namespace _3Dengine.Components.Cameras
{
    /// <summary>
    /// Statische camera.
    /// </summary>
    public class StaticCamera : ICamera
    {
        /// <summary>
        /// LookAtMatrix
        /// </summary>
        public Matrix4 LookAtMatrix { get; }

        /// <summary>
        /// Constructor van dit object.
        /// </summary>
        public StaticCamera()
        {
            Vector3 position;
            position.X = 0;
            position.Y = 0;
            position.Z = 0;
            LookAtMatrix = Matrix4.LookAt(position, -Vector3.UnitZ, Vector3.UnitY);
        }

        /// <summary>
        /// Overload constructor van dit object.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="target"></param>
        public StaticCamera(Vector3 position, Vector3 target)
        {
            LookAtMatrix = Matrix4.LookAt(position, target, Vector3.UnitY);
        }
        public void Update(double time)
        {

        }
    }
}
