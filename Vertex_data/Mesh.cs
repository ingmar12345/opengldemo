using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3Dengine.Components.Vertex_data
{
    /// <summary>
    /// Regelt het correct toewijzen van geheugen (buffering) voor een vertex array.
    /// </summary>
    public abstract class Mesh
    {
        /// <summary>
        /// Het programma ID.
        /// </summary>
        public readonly int program;

        /// <summary>
        /// Index die refereert naar de specifieke vertexArray.
        /// </summary>
        protected readonly int VAO;

        /// <summary>
        /// Index die refereert naar de specifieke buffer.
        /// </summary>
        protected readonly int buffer;

        /// <summary>
        /// Aantal vertices in een vertexArray.
        /// </summary>
        protected readonly int verticeCount;

        /// <summary>
        /// Constructor van dit object.
        /// </summary>
        /// <param name="program">programma variabele</param>
        /// <param name="vertexCount">aantal vertices</param>
        protected Mesh(int program, int vertexCount)
        {
            this.program = program;
            verticeCount = vertexCount;
            VAO = GL.GenVertexArray();
            buffer = GL.GenBuffer();
            GL.BindVertexArray(VAO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, buffer);
        }

        /// <summary>
        /// Koppelt een vertex array object aan een programma.
        /// </summary>
        public virtual void Bind()
        {
            GL.UseProgram(program);
            GL.BindVertexArray(VAO);
        }

        /// <summary>
        /// Beschrijft uit welke primitives (punt, lijn of driehoek) een vertex array wordt opgebouwd.
        /// </summary>
        public virtual void Render()
        {
            GL.DrawArrays(PrimitiveType.Triangles, 0, verticeCount);
        }

        /// <summary>
        /// Roept de functie aan die de VAO en VBO weggooit en deactiveert garbage collection.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);

            // zet garbage collection uit voor dit object
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Regelt het weggooien van VAO's (Vertex Array Object) en VBO's (Vertex Buffer Object).
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // verwijder de vertex array op de juiste index
                GL.DeleteVertexArray(VAO);

                // maak het geheugen vrij op de juiste locatie
                GL.DeleteBuffer(buffer);
            }
        }
    }
}
