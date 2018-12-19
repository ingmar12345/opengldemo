using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace _3Dengine.Components.Cameras
{
    /// <summary>
    /// Third person camera.
    /// </summary>
    public class TrackCamera : ICamera
    {
        /// <summary>
        /// LookAtMatrix.
        /// </summary>
        public Matrix4 LookAtMatrix { get; private set; }

        /// <summary>
        /// Het te volgen object.
        /// </summary>
        private readonly Objects.Object target;

        /// <summary>
        /// Volg afstand (camera)
        /// </summary>
        private readonly Vector3 offset;

        /// <summary>
        /// Constructor van dit object.
        /// </summary>
        /// <param name="target"></param>
        public TrackCamera(Objects.Object target)
            : this(target, Vector3.Zero)
        { }

        /// <summary>
        /// Overload constructor van dit object.
        /// </summary>
        /// <param name="target">te volgen object</param>
        /// <param name="offset">volg afstand</param>
        public TrackCamera(Objects.Object target, Vector3 offset)
        {
            this.target = target;
            this.offset = offset;
            Update(0);
        }

        /// <summary>
        /// Houdt bij wanneer 10 seconden zijn verstreken.
        /// </summary>
        private double secrule = 0;

        /// <summary>
        /// Update logica van dit object.
        /// </summary>
        /// <param name="time">tijd</param>
        public void Update(double time)
        {
            Vector3 eye = new Vector3(target.Position) + (offset * new Vector3(target.Direction));
            Vector3 lookAtTarget = new Vector3(target.Position);

            if (((int)(time - secrule)) > 2)
            {
                Debug.WriteLine("cam positie :" + eye.X + " " + eye.Y + " " + eye.Z);
                secrule = time;
            }

            LookAtMatrix = Matrix4.LookAt(eye, lookAtTarget, Vector3.UnitY);
        }
    }
}
