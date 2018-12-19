using OpenTK;
using OpenTK.Graphics;

namespace _3Dengine.Components.Vertex_data
{
    /// <summary>
    /// Struct om een vertex met een kleur in op te slaan.
    /// </summary>
    public struct Vertex
    {
        /// <summary>
        /// Grootte van het Vertex object in bytes.
        /// Dit hebben we later nodig om buffer te reserveren in het geheugen.
        /// </summary>
        public const int Size = (4 + 4) * 4; 

        /// <summary>
        /// Positie van een vertex.
        /// </summary>
        private readonly Vector4 position;

        /// <summary>
        /// Kleur van een vertex.
        /// </summary>
        private readonly Color4 color;

        /// <summary>
        /// Constructor van Vertex.
        /// </summary>
        /// <param name="position">Positie van een vertex</param>
        /// <param name="color">Kleur van een vertex</param>
        public Vertex(Vector4 position, Color4 color)
        {
            this.position = position;
            this.color = color;
        }
    }
}
