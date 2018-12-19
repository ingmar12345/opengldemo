using _3Dengine.Components.Vertex_data;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3Dengine.Components.Objects
{
    /// <summary>
    /// Bevat de implementatie van een landschap object.
    /// </summary>
    public class Scenery : Object
    {
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
        public Scenery(Mesh model, Vector4 position, Vector4 direction, Vector4 rotation, float speed)
            : base(model, position, rotation, direction)
        {
            original = model;
        }
    }
}
