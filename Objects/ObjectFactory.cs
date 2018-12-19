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
    /// Fabriek voor het aanmaken van objecten zoals een vliegtuig of een voetbalstadion.
    /// IDisposable biedt de mogelijkheid om garbage collection handmatig aan te passen voor dit object.
    /// </summary>
    public class ObjectFactory : IDisposable
    {
        /// <summary>
        /// Bibliotheek om modellen in op te slaan.
        /// Key: string met beschrijving van model.
        /// Value: mesh.
        /// </summary>
        private readonly Dictionary<string, Mesh> models;

        /// <summary>
        /// Constructor van dit object
        /// </summary>
        /// <param name="models">bibliotheek met meshes</param>
        public ObjectFactory(Dictionary<string, Mesh> models)
        {
            this.models = models;
        }

        /// <summary>
        /// Maakt een instantie van een vliegtuig object aan.
        /// </summary>
        /// <returns>Instantie van een vliegtuig</returns>
        public Plane CreatePlane()
        {
            var plane = new Plane(
                models["plane"],          // 3D mesh
                new Vector4(-10,20,20,0), // begin postitie
                new Vector4(0,0,1,0),     // richting (vliegtuig wijst parallel met Z-as)
                new Vector4(0), 0);       // rotatie / oriëntatie
            
            // vliegtuig is erg groot, dus even schalen naar kleiner vliegtuig
            plane.SetScale(new Vector3(0.05f));
            return plane;
        }

        /// <summary>
        /// Maakt een instantie van een landschap aan; in dit geval een voetbalstadion.
        /// </summary>
        /// <returns>Instantie van een landschap</returns>
        public Scenery CreateStadium()
        {
            var stadium = new Scenery(models["stadium"], new Vector4(0,0,0,0), Vector4.Zero, Vector4.Zero, 0.2f);
            stadium.SetScale(new Vector3(1.0f));
            return stadium;
        }

        /// <summary>
        /// Garbage collection van dit object.
        /// </summary>
        public void Dispose()
        {
            foreach (var obj in models)
                obj.Value.Dispose();
        }
    }
}
