using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ObjLoader.Loader.Data.Elements;
using ObjLoader.Loader.Loaders;
using OpenTK;
using OpenTK.Graphics;

namespace _3Dengine.Components.Vertex_data
{
    /// <summary>
    /// Factory voor het aanmaken van een vertex array.
    /// Hier worden de obj bestanden geparsed en uitgelezen.
    /// </summary>
    class MeshFactory
    {
        /// <summary>
        /// Creëer een 3D model uit een obj bestand op basis van het gewenste model.
        /// </summary>
        /// <param name="model">enum met het gewenste model</param>
        /// <returns>vertex array van alle vertices in het model (obj bestand)</returns>
        public static Vertex[] CreateModel(Models model)
        {
            // vliegtuig
            if (model == Models.plane)
            {
                return CreateMesh(model, "Components/OBJ_files/rcplane2.obj");
            }
            // stadion
            else if (model == Models.stadium)
            {
                return CreateMesh(model, "Components/OBJ_files/stadion.obj");
            }
            else
            {
                return new Vertex[0];
            }
        }

        /// <summary>
        /// Haalt de relevante vertices uit een obj bestand en wijst een kleur toe aan bepaalde groepen vertices.
        /// </summary>
        /// <param name="model">gewenste model</param>
        /// <param name="path">pad naar de obj file</param>
        /// <returns>vertex array van het gewenste model</returns>
        private static Vertex[] CreateMesh(Models model, string path)
        {
            ObjLoaderFactory factory = new ObjLoaderFactory();
            IObjLoader objLoader = factory.Create(new ObjMtlFileLoader(path));
            LoadResult result;
            using (var fileStream = File.OpenRead(path))
            {
                // parse de data uit het bestand naar het object result (vertices en faces)
                result = objLoader.Load(fileStream);
            }

            // lijst met alle individuele vertexen
            List<Vector4> tempVertices = new List<Vector4>();

            // de uiteindelijke lijst met alle driehoekjes en vlakken
            List<Vertex> vertices = new List<Vertex>();

            // vertex data uit de object file in een vector laden
            foreach (ObjLoader.Loader.Data.VertexData.Vertex r in result.Vertices)
            {
                tempVertices.Add(new Vector4(r.X, r.Y, r.Z, 1.0f));
            }

            // alle faces en de bijbehorende vertices uitlezen
            IList<Group> groups = result.Groups;

            for (int i = 0; i < groups.Count(); i++)
            {
                // op basis van het groepnummer wordt een kleur toegewezen aan een vertex
                string[] grouparr = groups[i].Name.Split(' ');
                string groupnr = "-1";
                if (grouparr.Length > 2)
                {
                    groupnr = grouparr[1].Last().ToString();
                }      

                Color4 color = new Color4();
                IList<Face> faces = result.Groups[i].Faces;
                foreach (Face f in faces)
                {
                    for (int j = 0; j < f.Count; j++)
                    {
                        FaceVertex indexFJ = f[j];
                        int v = indexFJ.VertexIndex;

                        Vector4 vertex = tempVertices[v - 1];

                        // kleuren voor vliegtuig
                        if (model == Models.plane)
                        {
                            // kleur bepalen van de verschillende componenten (groepen zoals dat heet)
                            switch (Convert.ToInt32(groupnr))
                            {
                                // ruiten
                                case 1:
                                    color = Color4.Blue;
                                    break;
                                // vleugels
                                case 2:
                                    color = Color4.Green;
                                    break;
                                // fuselage (romp)
                                case 3:
                                    color = Color4.Red;
                                    break;
                                default:
                                    color = Color4.Yellow;
                                    break;
                            }
                        }
                        // kleuren voor stadion
                        else if (model == Models.stadium)
                        {
                            // kleur bepalen van de verschillende componenten (groepen zoals dat heet)
                            switch (Convert.ToInt32(groupnr))
                            {
                                // voetbal veld
                                case 0:
                                    color = Color4.Green;
                                    break;
                                // de rest van het stadion
                                default:
                                    color = Color4.Black;
                                    break;
                            }
                        }
                        

                        vertices.Add(new Vertex(vertex, color));
                    }
                }
            }

            // lijst met vertices omzetten naar een array en retourneren
            return vertices.ToArray();
        }
    }
}
