using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3Dengine.Components
{
    /// <summary>
    /// Klasse om materiaal eigenschappen van een object in op te slaan.
    /// Wordt momenteel nog niet gebruikt.
    /// </summary>
    class Material
    {
        public Vector3 AmbientColor = new Vector3();
        public Vector3 DiffuseColor = new Vector3();
        public Vector3 SpecularColor = new Vector3();
        public float SpecularExponent = 1;
        public float Opacity = 1.0f;

        public String AmbientMap = "";
        public String DiffuseMap = "";
        public String SpecularMap = "";
        public String OpacityMap = "";
        public String NormalMap = "";

        public Material()
        {
        }

        /// <summary>
        /// Constuctor van dit object.
        /// </summary>
        /// <param name="ambient"></param>
        /// <param name="diffuse"></param>
        /// <param name="specular"></param>
        /// <param name="specexponent"></param>
        /// <param name="opacity"></param>
        public Material(Vector3 ambient, Vector3 diffuse, Vector3 specular, float specexponent = 1.0f, float opacity = 1.0f)
        {
            AmbientColor = ambient;
            DiffuseColor = diffuse;
            SpecularColor = specular;
            SpecularExponent = specexponent;
            Opacity = opacity;
        }
    }
}
