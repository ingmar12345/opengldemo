using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3Dengine.Components.Cameras
{
    /// <summary>
    /// Interface voor camera.
    /// </summary>
    public interface ICamera
    {
        /// <summary>
        /// LookAtMatrix.
        /// </summary>
        Matrix4 LookAtMatrix { get; }
        
        /// <summary>
        /// Update logica voor camera.
        /// </summary>
        /// <param name="time"></param>
        void Update(double time = 0);
    }
}
