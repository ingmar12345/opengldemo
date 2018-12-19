using _3Dengine.Components.Cameras;
using _3Dengine.Components.Vertex_data;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3Dengine.Components.Objects
{
    /// <summary>
    /// Beschrijft een 3D object.
    /// Deze klasse beperkt zich niet tot een mesh van een 3D object, 
    /// maar ook tot de eigenschappen zoals rotatie en positie.
    /// Naast eigenschappen bevat elk Object ook een eigen impementatie voor het renderen.
    /// </summary>
    public abstract class Object
    {
        /// <summary>
        /// Bevat de mesh (vertex data) van dit object.
        /// </summary>
        public Mesh Model
        {
            get
            {
                return model;
            }
        }  
        
        /// <summary>
        /// Bevat de positie van dit object.
        /// </summary>
        public Vector4 Position
        {
            get
            {
                return position;
            }
        }

        /// <summary>
        /// Bevat de richting van dit object.
        /// </summary>
        public Vector4 Direction
        {
            get
            {
                return direction;
            }
        }

        /// <summary>
        /// Bevat de schaal van dit object.
        /// </summary>
        public Vector3 Scale
        {
            get
            {
                return scale;
            }
        }

        protected Mesh model;
        protected Vector4 position;
        protected Vector4 direction;
        protected Vector4 rotation;    
        protected Matrix4 modelView;
        protected Vector3 scale;
        
        /// <summary>
        /// Constructor van dit object.
        /// </summary>
        /// <param name="model">mesh data</param>
        /// <param name="position">positie</param>
        /// <param name="rotation">rotatie</param>
        /// <param name="direction">richting</param>
        public Object(Mesh model, Vector4 position, Vector4 rotation, Vector4 direction)
        {
            this.model = model;
            this.position = position;
            this.rotation = rotation;
            this.direction = direction;
            scale = new Vector3(1);
        }

        /// <summary>
        /// Schaal veranderen.
        /// </summary>
        /// <param name="scale">de gewenste schaal</param>
        public void SetScale(Vector3 scale)
        {
            this.scale = scale;
        }
        
        /// <summary>
        /// Bevat alle acties indien er een nieuw frame wordt gerendert.
        /// </summary>
        /// <param name="time">tijd</param>
        public virtual void Update(double time = 0)
        {
            
        }

        /// <summary>
        /// Bevat alle logica voor het renderen specifiek van dit object.
        /// </summary>
        /// <param name="cam">camera</param>
        public virtual void Render(ICamera cam)
        {
            // mesh koppelen aan huidige programma
            model.Bind();

            // quaternion om gimball lock te voorkomen
            Quaternion quat = Quaternion.Identity;
            quat = Quaternion.FromEulerAngles(rotation.X, rotation.Y, rotation.Z);
            Matrix4 rq = Matrix4.CreateFromQuaternion(quat);

            // translatie
            Matrix4 t = Matrix4.CreateTranslation(position.X, position.Y, position.Z);

            // rotatie in euler angles
            //Matrix4 rmx = Matrix4.CreateRotationX(rotation.X);
            //Matrix4 rmy = Matrix4.CreateRotationY(rotation.Y);
            //Matrix4 rmz = Matrix4.CreateRotationZ(rotation.Z);

            // schalen
            Matrix4 s = Matrix4.CreateScale(scale);

            // gebruiken bij euler rotatie
            //modelView = rmx * rmy * rmz * s * t * cam.LookAtMatrix;

            // gebruiken bij quaternion rotatie
            modelView = rq * s * t * cam.LookAtMatrix;

            /* model view matrix toewijzen in de uniform matrix
            (uniform matrix is globaal, ook in de shaders) */
            GL.UniformMatrix4(21, false, ref modelView);
            model.Render();
        }
    }
}
