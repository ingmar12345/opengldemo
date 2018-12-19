using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3Dengine.Components
{
    /// <summary>
    /// ShaderProgram regelt het beheren van shaders.
    /// IDisposable biedt de mogelijkheid om garbage collection handmatig aan te passen voor dit object.
    /// </summary>
    class ShaderProgram : IDisposable
    {
        /// <summary>
        /// Programma object.
        /// </summary>
        public int ID { get; }

        /// <summary>
        /// Lijst met shader objecten.
        /// </summary>
        private readonly List<int> shaders = new List<int>();
              
        /// <summary>
        /// Constructor van dit object.
        /// </summary>
        public ShaderProgram()
        {
            ID = GL.CreateProgram();
        }
        
        /// <summary>
        /// Koppelt shader objecten met een programma object.
        /// </summary>
        public void LinkShaders()
        {
            foreach (int shader in shaders)
            {
                // koppelt een shader aan een programma variabele
                GL.AttachShader(ID, shader);
            }

            // linkt alle shader objecten gekoppeld aan een programma met elkaar
            GL.LinkProgram(ID);

            /* verwijder shaders, als de shader eenmaal gelinkt is aan het programma 
              is de shader niet langer nodig omdat het programma de binaire code 
              bevat van de desbetreffende shader */
            foreach (var shader in shaders)
            {
                GL.DetachShader(ID, shader);
                GL.DeleteShader(shader);
            }
        }

        /// <summary>
        /// shader aanmaken van het gewenste type (vertex / fragment) en compilen.
        /// </summary>
        /// <param name="type">type shader</param>
        /// <param name="path">pad naar shader bestand</param>
        public void AddShader(ShaderType type, string path)
        {
            int shader = GL.CreateShader(type);
            string src = File.ReadAllText(path);
            GL.ShaderSource(shader, src);
            GL.CompileShader(shader);
            shaders.Add(shader);
        }

        /// <summary>
        /// Roept de functie aan die het shader programma weggooit en deactiveert garbage collection.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);

            // zet garbage collection uit voor dit object
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Garbage collection voor dit object.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                GL.DeleteProgram(ID);
            }
        }
    }
}
