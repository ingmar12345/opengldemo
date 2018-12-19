using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _3Dengine.Components;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using System.Diagnostics;
using _3Dengine.Components.Objects;
using _3Dengine.Components.Vertex_data;
using _3Dengine.Components.Cameras;

namespace _3Dengine.Components
{
    /// <summary>
    /// MainWindow erft van GameWindow.
    /// GameWindow is een blauwdruk voor een OpenGL applicatie met OpenTK.
    /// </summary>
    public sealed class MainWindow : GameWindow
    {
        /// <summary>
        /// Tijd
        /// </summary>
        private double time;

        /// <summary>
        /// Fabriek die objecten aanmaakt.
        /// </summary>
        private ObjectFactory objectFactory;

        /// <summary>
        /// Lijst met objecten.
        /// </summary>
        private readonly List<Objects.Object> objects = new List<Objects.Object>();

        /// <summary>
        /// Bibliotheek om modellen in op te slaan.
        /// Key: string met beschrijving van model.
        /// Value: mesh.
        /// </summary>
        private Dictionary<string, Mesh> models = new Dictionary<string, Mesh>();

        /// <summary>
        /// Vliegtuig van de gebruiker.
        /// </summary>
        private Plane pilot;

        /// <summary>
        /// Shader programma.
        /// </summary>
        private ShaderProgram renderingProgram;

        /// <summary>
        /// Achtergrond kleur.
        /// </summary>
        private readonly Color4 backColor = new Color4(0.7f, 240f, 250f, 255f);

        /// <summary>
        /// Projectie matrix.
        /// </summary>
        private Matrix4 projectionMatrix;

        /// <summary>
        /// Instantie van het camera object.
        /// </summary>
        private ICamera cam;

        /// <summary>
        /// Object voor parallelle berekeningen aanmaken.
        /// </summary>
        private Parallel p = new Parallel();

        /// <summary>
        /// Aanmaken van een venster.
        /// </summary>
        public MainWindow(): base(
            640, // breedte
            360, // hoogte
            GraphicsMode.Default,
            "Flight Simulator Y",  // titel
            GameWindowFlags.Default,
            DisplayDevice.Default,
            4, // OpenGL hoofd versie
            0, // OpenGL minor versie
            GraphicsContextFlags.ForwardCompatible)
            {
                Title += ": OpenGL Versie: " + GL.GetString(StringName.Version);
            }

        /// <summary>
        /// Deze functie word automatisch aangeroepen als de venster grootte veranderd.
        /// Applicatie op volledig scherm roept deze functie 1 keer aan.
        /// Gevensterde applicaties kunnen deze functie vaker aanroepen indien het venster van grootte wordt veranderd.
        /// </summary>
        /// <param name="e">Event informatie</param>
        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            CreateProjectionMatrix();
        }

        /// <summary>
        /// Startup functie die alle informatie inlaad, zoals shaders en geometrie uit obj bestanden.
        /// </summary>
        /// <param name="e">event informatie</param>
        protected override void OnLoad(EventArgs e) 
        {
            Debug.WriteLine("OnLoad");

            // verticale synchronisatie uitzetten
            VSync = VSyncMode.Off;

            // projectie matrix aanmaken
            CreateProjectionMatrix();

            // shader programma aanmaken
            renderingProgram = new ShaderProgram();

            // shaders toevoegen aan het programma
            renderingProgram.AddShader(ShaderType.VertexShader, @"Components\Shaders\vertexShader.glsl");
            renderingProgram.AddShader(ShaderType.FragmentShader, @"Components\Shaders\fragmentShader.glsl");

            // shaders linken
            renderingProgram.LinkShaders();

            // ambachtelijk gemaakte 3D objecten
            Vertex[] customStadium = MeshFactory.CreateModel(Models.stadium);
            Vertex[] customPlane = MeshFactory.CreateModel(Models.plane);

            // model aanmaken en koppelen aan juiste shader programma
            models.Add("stadium", new ColorMesh(customStadium, renderingProgram.ID));
            models.Add("plane", new ColorMesh(customPlane, renderingProgram.ID));
            objectFactory = new ObjectFactory(models);

            pilot = objectFactory.CreatePlane();
            objects.Add(pilot);
            objects.Add(objectFactory.CreateStadium());

            // third person camera die zich richt op het vliegtuig van de piloot
            cam = new TrackCamera(pilot, new Vector3(0,0,-40));
            //cam = new StaticCamera(new Vector3(0,20,40), new Vector3(pilot.Position));

            CursorVisible = true;

            // polygon mode kan men aanpassen voor ander soort weergave (lijnen, punten of vlakken)
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            GL.PatchParameter(PatchParameterInt.PatchVertices, 3);
            GL.PointSize(3);
            GL.Enable(EnableCap.DepthTest); // laat alleen objecten zien die zich niet achter een ander object bevinden
            GL.Enable(EnableCap.CullFace); // laat alleen de voorkant zien van een vlak
            Closed += OnClosed;
            Debug.WriteLine("Inladen klaar");         
        }

        /// <summary>
        /// Als de gebruiker het programma afsluit word het programma schoongemaakt.
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="eventArgs">event informatie</param>
        private void OnClosed(object sender, EventArgs eventArgs)
        {
            Exit();
        }

        /// <summary>
        /// Bevat alle logica voor het afsluiten van het programma.
        /// </summary>
        public override void Exit()
        {
            // garbage collection
            objectFactory.Dispose();
            renderingProgram.Dispose();
            base.Exit();
        }

        /// <summary>
        /// Projectie matrix aanmaken.
        /// </summary>
        private void CreateProjectionMatrix()
        {
            float aspectRatio = (float)Width / Height;
            projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(
                60 * ((float)Math.PI / 180f), // kijkhoek in radialen
                aspectRatio,                  // beeldverhouding
                0.1f,                         // objecten die dichterbij zijn dan deze waarde worden niet gerendert
                4000f);                       // objecten die verderweg zijn dan deze waarde worden niet gerendert
        }

        /// <summary>
        /// Wordt uitgevoerd nadat een frame is geupdate.
        /// Praktisch om render fase gescheiden gehouden van bijvoorbeeld keybindings, physics en andere niet-graphics gerelateerde zaken.
        /// </summary>
        /// <param name="e">event informatie</param>
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            //p.Multiply(Matrix4.Identity, p.RotationX(0.05f));

            time += e.Time;
            foreach (var obj in objects)
            {
                obj.Update(time);
            }
            cam.Update(time);
            UserInput();
        }

        /// <summary>
        /// Regelt gebruikersinvoer.
        /// </summary>
        private void UserInput() 
        {
            KeyboardState state = Keyboard.GetState();

            // het programma kan worden gesloten met escape knop
            if (state.IsKeyDown(Key.Escape)) 
            {
                Exit();
            }

            // roll / aillerons (roteren om Z-as)
            if (state.IsKeyDown(Key.Right))
            {
                pilot.ControlRoll(Heading.positive);
            }
            if (state.IsKeyDown(Key.Left))
            {
                pilot.ControlRoll(Heading.negative);
            }

            // pitch / elevator (roteren om X-as)
            if (state.IsKeyDown(Key.Up))
            {
                pilot.ControlPitch(Heading.positive);
            }
            if (state.IsKeyDown(Key.Down))
            {
                pilot.ControlPitch(Heading.negative);
            }

            // yaw / rudder (roteren om Y-as)
            if (state.IsKeyDown(Key.BracketLeft))
            {
                pilot.ControlYaw(Heading.positive);
            }
            if (state.IsKeyDown(Key.BracketRight))
            {
                pilot.ControlYaw(Heading.negative);
            }

            // snelheid
            if (state.IsKeyDown(Key.W))
            {
                pilot.ControlSpeed(Heading.positive);
            }        
            if (state.IsKeyDown(Key.S))
            {
                pilot.ControlSpeed(Heading.negative);
            }

            // alles neutraliseren
            if (state.IsKeyDown(Key.Space))
            {
                pilot.ControlSpeed(Heading.neutral);
                pilot.ControlRoll(Heading.neutral);
                pilot.ControlPitch(Heading.neutral);
                pilot.ControlYaw(Heading.neutral);
            }
        }

        /// <summary>
        /// Render functie; hier wordt alles op het frame getekend.
        /// </summary>
        /// <param name="e">event informatie</param>
        protected override void OnRenderFrame(FrameEventArgs e) 
        {
            // totale tijd
            time += e.Time;

            // Vsync indicator en FPS teller
            Title = $"(Vsync: {VSync}) FPS: {1f / e.Time:0}";

            // achtergrond kleur
            GL.ClearColor(backColor);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            int lastProgram = -1;

            foreach (var obj in objects)
            {
                // geef aan welk programma op dit moment wordt gebruikt
                var program = obj.Model.program;

                if (lastProgram != program)
                {
                    /* projectie matrix toewijzen in de uniform matrix
                    (uniform matrix is globaal, ook in de shaders) */
                    GL.UniformMatrix4(20, false, ref projectionMatrix);
                }
                    
                lastProgram = obj.Model.program;

                /* per object verschilt de implementatie
                 * een stadion moet op zijn plek blijven terwijl
                 * een vliegtuig zich moet verplaatsen over de spelwereld */
                obj.Render(cam);

            }
            SwapBuffers();
        }   
    }
}
