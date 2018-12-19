using System;
using _3Dengine.Components;
using OpenTK.Graphics.OpenGL4;

namespace _3Dengine.Components.Vertex_data
{
    /// <summary>
    /// Regelt het daadwerkelijke bufferen van een gekleurde Vertex.
    /// </summary>
    public class ColorMesh : Mesh
    {
        /// <summary>
        /// Constructor van Mesh.
        /// </summary>
        /// <param name="vertices">vertex array</param>
        /// <param name="program">programma variabele</param>
        public ColorMesh(Vertex[] vertices, int program) : base(program, vertices.Length)
        {
            /* voordat een buffer object gebruikt kan worden moet eerst de data op de
             * correcte manier toegewezen worden */
            GL.NamedBufferStorage(
                buffer,                             // startadres van de buffer in het geheugen
                Vertex.Size * vertices.Length,      // bepaalt hoe groot de opslag is in bytes
                vertices,                           // de data waarmee de buffer geinitialiseerd wordt
                BufferStorageFlags.MapWriteBit);    // buffer data wordt toegewezen voor schrijven

            /* koppelt de vertex array aan de buffer en genereert er een attribuut voor
             * als een vertex array object is gekoppelt, moet het attribuut op de index die
             gespecificeerd is in het tweede argument (attribuut index) de data ophalen uit de buffer (binding index, derde argument) */
            GL.VertexArrayAttribBinding(VAO, 0, 0);

            // sta toe dat vertex attribute op index 0 automatisch toegewezen mag worden
            GL.EnableVertexArrayAttrib(VAO, 0);

            // Beschrijf de opmaak en formaat van de data
            GL.VertexArrayAttribFormat(
                VAO,                    // vertex array object
                0,                      // index van de vertex attribuut, shader locatie 0
                4,                      // aantal waarden in de buffer voor elke vertex (Vector4 dus 4)
                VertexAttribType.Float, // type van de data, Vector4 bestaat uit floats dus spreekt voor zich
                false,                  // data hoeft niet genormaliseerd te worden (schalen tussen 0 en 1), kan genegeerd worden voor zwevende kommagetallen
                0);                     // geeft aan hoeveel bytes er zitten tussen het begin van een vertex tot het volgende begin van een vertex, kan op 0 gezet worden om OpenGL het zelf te laten berekenen

            /* koppelt de vertex array aan de buffer en genereert er een attribuut voor
             * als een vertex array object is gekoppelt, moet het attribuut op de index die
             gespecificeerd is in het tweede argument (attribuut index) de data ophalen uit de buffer (binding index, derde argument) */
            GL.VertexArrayAttribBinding(VAO, 1, 0);

            // sta toe dat vertex attribute op index 1 automatisch toegewezen mag worden
            GL.EnableVertexArrayAttrib(VAO, 1);

            // Beschrijf de opmaak en formaat van de data
            GL.VertexArrayAttribFormat(
                VAO,                    // vertex array object
                1,                      // index van de vertex attribuut, shader locatie 1
                4,                      // aantal waarden in de buffer voor elke vertex (Vector4 dus 4)
                VertexAttribType.Float, // type van de data, Vector4 bestaat uit floats dus spreekt voor zich
                false,                  // data hoeft niet genormaliseerd te worden (schalen tussen 0 en 1), kan genegeerd worden voor zwevende kommagetallen
                16);                    // geeft aan hoeveel bytes er zitten tussen het begin van een vertex tot het volgende begin van een vertex, kan op 0 gezet worden om OpenGL het zelf te laten berekenen

            /* geeft aan in welk buffer object onze data opgeslagen is 
             * en waar in die desbetreffende buffer de specifieke data zit */
            GL.VertexArrayVertexBuffer(
                VAO,          // vertex array object
                0,            // index van de vertex buffer (komt overeen met derde argument in de functie VertexArrayAttribBinding)
                buffer,       // naam van het buffer object dat gekoppelt wordt
                IntPtr.Zero,  // geeft aan waar de eerste data van de vertex begint (in bytes)
                Vertex.Size); // geeft aan hoever elke vertex is (in bytes)
        }
    }
}