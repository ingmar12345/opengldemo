using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenCL;
using Cloo;
using System.Diagnostics;

namespace _3Dengine.Components
{
    /// <summary>
    /// Object voor GPU accleratie met OpenCL.
    /// </summary>
    class Parallel
    {

        public Parallel()
        {
           
        }

        /// <summary>
        /// Rotatie om de X-as.
        /// </summary>
        /// <param name="inputMatrix">de te roteren matrix</param>
        /// <param name="angle">rotatie hoek</param>
        public void CreateRotationX(Matrix4 inputMatrix, float angle)
        {
            Multiply(inputMatrix, RotationX(angle));
        }

        /// <summary>
        /// Rotatie om de Y-as.
        /// </summary>
        /// <param name="inputMatrix">de te roteren matrix</param>
        /// <param name="angle">rotatie hoek</param>
        public void CreateRotationY(Matrix4 inputMatrix, float angle)
        {
            Multiply(inputMatrix, RotationY(angle));
        }

        /// <summary>
        /// Rotatie om de Z-as.
        /// </summary>
        /// <param name="inputMatrix">de te roteren matrix</param>
        /// <param name="angle">rotatie hoek</param>
        public void CreateRotationZ(Matrix4 inputMatrix, float angle)
        {
            Multiply(inputMatrix, RotationZ(angle));
        }

        /// <summary>
        /// Matrix omzetten naar 2 dimensionale array, OpenCL C kent geen matrix typen.
        /// </summary>
        /// <param name="m">de om te zetten matrix</param>
        /// <returns>matrix gerepresenteerd als een 2 dimensionale array</returns>
        private float[][] MatrixToArray(Matrix4 m)
        {
            float[][] result = new float[4][];
            result[0] = new[] { m.Row0.X, m.Row0.Y, m.Row0.Z, m.Row0.W };
            result[1] = new[] { m.Row1.X, m.Row1.Y, m.Row1.Z, m.Row1.W };
            result[2] = new[] { m.Row2.X, m.Row2.Y, m.Row2.Z, m.Row2.W };
            result[3] = new[] { m.Row3.X, m.Row3.Y, m.Row3.Z, m.Row3.W };

            return result;
        }

        /// <summary>
        /// Matrix vermenigvuldiging.
        /// </summary>
        /// <param name="inputMatrix">matrix die bewerkt moet worden</param>
        /// <param name="operationMatrix">matrix met de gewenste operatie</param>
        public void Multiply(Matrix4 inputMatrix, float[][] operationMatrix)
        {
            float[][] a = MatrixToArray(inputMatrix);
            float[][] b = operationMatrix;

            List<float> data = new List<float>(); // 1 dimensionale lijst met data voor OpenCL kernel

            // data platslaan
            data.Add(a.Length);
            for (int i = 0; i < a.Length; i++)
            {
                for (int j = 0; j < a.Length; j++)
                {
                    data.Add(a[i][j]);
                }
            }
            for (int i = 0; i < b.Length; i++)
            {
                for (int j = 0; j < b.Length; j++)
                {
                    data.Add(b[i][j]);
                }
            }
            
            // naar array omzetten (OpenCL kernel moet geschreven zijn in C)
            float[] message = data.ToArray(); 
            
            // parallelliseren over 200 rekenkernen
            int N = 200;

            // MultiCL wil zeggen; alle beschikbare hardware (CPU en GPU) gebruiken voor parallellisatie
            MultiCL cl = new MultiCL(); 
            cl.ProgressChangedEvent += Cl_ProgressChangedEvent1;

            // compileren van de OpenCL kernel
            cl.SetKernel(
                MultiplyCLKernel, // geeft aan in welke functie de kernel te vinden is
                "Multiply");      // geeft aan hoe de OpenCL kernel heet

            // platgeslagen data (beide arrays) naar de kernel sturen
            cl.SetParameter(message); 

            // kernel aanroepen
            cl.Invoke(
                0,              // begin index
                message.Length, // eind index
                N);             // parallelliseren over N rekenkernen
        }


        private static void Cl_ProgressChangedEvent1(object sender, double e)
        {
            Debug.WriteLine(e.ToString("0.00%"));
        }

        /// <summary>
        /// OpenCL kernel.
        /// </summary>
        public static string MultiplyCLKernel
        {
            get
            {
                string result = @"
                kernel void Multiply(global float* message)
                {
                    //int size = (int)get_global_id(0);
                    int size = (int)message[0]; // afmeting van array staat op eerste index, daarna de platgedrukte arrays
                    float M1[4][4]; // eerste matrix declareren met juiste lengte

                    int count = 1; // index bijhouden in de platgedrukte vormalige 2 dimensionale array
                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 0; i < 4; i++)
                        {
                            M1[i][j] = message[count];
                            count++;
                        }
                    }
                    
                    float M2[4][4];
                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 0; i < 4; i++)
                        {
                            M2[i][j] = message[count];
                            count++;
                        }
                    }

                    float resp[4][4];

                    for (int i = 0; i < 4; i++)
                        for (int j = 0; j < 4; j++)
                            for (int k = 0; k < 4; k++)
                                resp[i][j] += M1[i][k] * M2[k][j];

                    return;
                }";

            return result;
            }
        }

        /// <summary>
        /// Rotatie matrix voor een rotatie om de X-as.
        /// </summary>
        /// <param name="angle">hoek</param>
        /// <returns>rotatie matrix</returns>
        public float[][] RotationX(float angle)
        {
            float[][] rotationMatrix = {
                new[] { 1.0f, 0.0f, 0.0f, 0.0f },
                new[] { 0.0f, (float)Math.Cos(angle), (float)Math.Sin(angle), 0.0f },
                new[] { 0.0f, -(float)Math.Sin(angle), (float)Math.Cos(angle), 0.0f },
                new[] { 0.0f, 0.0f, 0.0f, 1.0f }
            };
            return rotationMatrix;
        }

        /// <summary>
        /// Rotatie matrix voor een rotatie om de Y-as.
        /// </summary>
        /// <param name="angle">hoek</param>
        /// <returns>rotatie matrix</returns>
        public float[][] RotationY(float angle)
        {
            float[][] rotationMatrix = {
                new[] { (float)Math.Cos(angle), 0.0f, -(float)Math.Sin(angle), 0.0f },
                new[] { 0.0f, 1.0f, 0.0f, 0.0f },
                new[] { (float)Math.Sin(angle), 0.0f, (float)Math.Cos(angle), 0.0f },
                new[] { 0.0f, 0.0f, 0.0f, 1.0f }
            };
            return rotationMatrix;
        }

        /// <summary>
        /// Rotatie matrix voor een rotatie om de Z-as.
        /// </summary>
        /// <param name="angle">hoek</param>
        /// <returns>rotatie matrix</returns>
        public float[][] RotationZ(float angle)
        {
            float[][] rotationMatrix = {
                new[] { (float)Math.Cos(angle), -(float)Math.Sin(angle), 0.0f, 0.0f },
                new[] { (float)Math.Sin(angle), (float)Math.Cos(angle), 0.0f, 0.0f },
                new[] { 0.0f, 0.0f, 1.0f, 0.0f },
                new[] { 0.0f, 0.0f, 0.0f, 1.0f }
            };
            return rotationMatrix;
        }
    }
}
