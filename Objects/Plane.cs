using _3Dengine.Components.Cameras;
using _3Dengine.Components.Vertex_data;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3Dengine.Components.Objects
{
    /// <summary>
    /// Bevat de implementatie van een vliegtuig object.
    /// </summary>
    public class Plane : Object
    {
        /// <summary>
        /// Snelheid van het vliegtuig.
        /// </summary>
        private float speed;

        /// <summary>
        /// Originele model.
        /// </summary>
        private Mesh original;

        /// <summary>
        /// Constructor van dit object.
        /// </summary>
        /// <param name="model">mesh</param>
        /// <param name="position">positie</param>
        /// <param name="direction">richting</param>
        /// <param name="rotation">rotatie</param>
        /// <param name="speed">snelheid</param>
        public Plane(Mesh model, Vector4 position, Vector4 direction, Vector4 rotation, float speed)
            : base(model, position, rotation, direction)
        {
            this.speed = speed;
            original = model;          
        }

        /// <summary>
        /// Houdt bij wanneer er 10 seconden zijn verstreken.
        /// </summary>
        private double secrule = 0;

        /// <summary>
        /// Update de positie en richting van het vliegtuig.
        /// </summary>
        /// <param name="time">tijd</param>
        public override void Update(double time)
        {

            // voor waarde W geldt: 0 is een richting en 1 is een locatie
            //Vector4 d = new Vector4(rotation.X, rotation.Y, rotation.Z, 0);
            //Vector4 d = new Vector4(new Vector3(0, 2, 0), 0);

            Quaternion q = Quaternion.Identity;
            q = Quaternion.FromEulerAngles(new Vector3(rotation.X, rotation.Y, rotation.Z));
            Matrix4 rot = Matrix4.CreateFromQuaternion(q);

            //Vector4 d = Matrix4.CreateRotationX(rotation.X) * Matrix4.CreateRotationY(rotation.Y) * Matrix4.CreateRotationZ(rotation.Z) * position;

            Vector4 d = rot * position;

            d.Normalize();

            //Vector4 n = new Vector4(0);

            //if (d != new Vector4(0))
            //{
            //    n = Vector4.Normalize(d);
            //}

            direction = d;

            position += direction * speed;
            

            if (((int)(time - secrule)) > 2)
            {
                Debug.WriteLine("direction: " + d.X + " " + d.Y + " " + d.Z);
                Debug.WriteLine("rotation: " + rotation.X + " " + rotation.Y + " " + rotation.Z);
                Debug.WriteLine("position: " + position.X + " " + position.Y + " " + position.Z);
                Debug.WriteLine("speed: " + speed);
                //Debug.WriteLine("lengte D: " + d.Length);
                //Debug.WriteLine("lengte N: " + n.Length);
                Debug.WriteLine("----------------------");
                secrule = time;
            }

            base.Update();
        }

        /// <summary>
        /// Berekent het kruisproduct van 2 vectoren.
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        private Vector4 CrossProduct(Vector4 v1, Vector4 v2)
        {
            return new Vector4(v1.Y * v2.Z - v1.Z * v2.Y,
                 v1.Z * v2.X - v1.X * v2.Z,
                 v1.X * v2.Y - v1.Y  * v2.X, 1);
        }

        /// <summary>
        /// Render functie van dit object.
        /// </summary>
        /// <param name="cam">camera</param>
        public override void Render(ICamera cam)
        {
            base.Render(cam);
        }

        /// <summary>
        /// Oude rotatie van dit object.
        /// </summary>
        private Vector3 oldRot = new Vector3(0);

        /// <summary>
        /// Pitch aanpassen.
        /// </summary>
        /// <param name="h">richting</param>
        public void ControlPitch(Heading h)
        {
            switch (h)
            {
                case Heading.negative:
                    //rotationAngle.X = oldRot.X - 0.05f; // voor quaternion               
                    rotation.X -= 0.05f; // voor euler rotatie
                    break;
                case Heading.positive:
                    //rotationAngle.X = oldRot.X + 0.05f; // voor quaternion
                    rotation.X += 0.05f; // voor euler rotatie
                    break;
                case Heading.neutral:
                    //rotationAngle.X = 0; // voor quaternion
                    rotation.X = 0; // voor euler rotatie
                    break;
                default:
                    break;
            }
            //oldRot.X = rotationAngle.X;
        }

        /// <summary>
        /// Roll aanpassen.
        /// </summary>
        /// <param name="h">richting</param>
        public void ControlRoll(Heading h)
        {
            switch (h)
            {
                case Heading.negative:
                    //rotationAngle.Z = oldRot.Z - 0.05f; // voor quaternion
                    rotation.Z -= 0.05f; // voor euler rotatie
                    break;
                case Heading.positive:
                    //rotationAngle.Z = oldRot.Z + 0.05f; // voor quaternion
                    rotation.Z += 0.05f; // voor euler rotation
                    break;
                case Heading.neutral:
                    //rotationAngle.Z = 0; // voor quaternion
                    rotation.X = 0; // voor euler rotatie
                    break;
                default:
                    break;
            }
            //oldRot.Z = RotationAngle.Z;
        }

        /// <summary>
        /// Yaw aanpassen.
        /// </summary>
        /// <param name="h">richting</param>
        public void ControlYaw(Heading h)
        {
            switch (h)
            {
                case Heading.negative:
                    //rotationAngle.Y = oldRot.Y - 0.05f; // voor quaternion
                    rotation.Y -= 0.05f; // voor euler rotatie
                    break;
                case Heading.positive:
                    //rotationAngle.Y = oldRot.Y + 0.05f; // voor quaternion
                    rotation.Y += 0.05f; // voor euler rotatie
                    break;
                case Heading.neutral:
                    //rotationAngle.Y = 0; // voor quaternion
                    rotation.Y = 0; // voor euler rotatie
                    break;
                default:
                    break;
            }
            //oldRot.Y = rotationAngle.Y;
        }

        /// <summary>
        /// Snelheid aanpassen.
        /// </summary>
        /// <param name="h">richting</param>
        public void ControlSpeed(Heading h)
        {        
                switch (h)
                {
                    case Heading.negative:
                        if (speed > -0.15f)
                        {
                            speed -= 0.002f;
                        }     
                        break;
                    case Heading.positive:
                        if (speed < 0.15f)
                        {
                            speed += 0.002f;
                        }
                        break;
                    case Heading.neutral:
                        speed = 0;
                        break;
                    default:
                        break;
                
            }
        }
    }
}
