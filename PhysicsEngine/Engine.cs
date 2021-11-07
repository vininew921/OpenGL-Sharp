using GLFW;
using GlmNet;
using PhysicsEngine.Meshes;
using PhysicsEngine.RenderCamera;
using PhysicsEngine.Shaders;
using PhysicsEngine.Structs;
using PhysicsEngine.Textures;
using static OpenGL.GL;
using static PhysicsEngine.Meshes.Mesh;

namespace PhysicsEngine
{
    public static class Engine
    {
        private static Window mainWindow;
        private static Shader shaderProgram;
        private static Shader lightShader;
        private static Camera camera;
        private static Mesh plane;
        private static Mesh light;

        private static readonly float[] vertices =
        {
            -1.0f, 0.0f,  1.0f,     0.0f, 1.0f, 0.0f,       1.0f, 1.0f, 1.0f,       0.0f, 0.0f,     
            -1.0f, 0.0f, -1.0f,     0.0f, 1.0f, 0.0f,       1.0f, 1.0f, 1.0f,       0.0f, 1.0f,     
             1.0f, 0.0f, -1.0f,     0.0f, 1.0f, 0.0f,       1.0f, 1.0f, 1.0f,       1.0f, 1.0f,
             1.0f, 0.0f,  1.0f,     0.0f, 1.0f, 0.0f,       1.0f, 1.0f, 1.0f,       1.0f, 0.0f
        };

        private static readonly uint[] indices =
        {
            0, 1, 2,
            0, 2, 3
        };

        private static readonly float[] lightVertices =
        {
            -0.1f, -0.1f,  0.1f,
            -0.1f, -0.1f, -0.1f,
             0.1f, -0.1f, -0.1f,
             0.1f, -0.1f,  0.1f,
            -0.1f,  0.1f,  0.1f,
            -0.1f,  0.1f, -0.1f,
             0.1f,  0.1f, -0.1f,
             0.1f,  0.1f,  0.1f
        };

        private static readonly uint[] lightIndices =
        {
            0, 1, 2,
            0, 2, 3,
            0, 4, 7,
            0, 7, 3,
            3, 7, 6,
            3, 6, 2,
            2, 6, 5,
            2, 5, 1,
            1, 5, 4,
            1, 4, 0,
            4, 5, 6,
            4, 6, 7
        };

        private const int WINDOW_WIDTH = 800;
        private const int WINDOW_HEIGHT = 800;
        private const string WINDOW_TITLE = "3D Physics Engine";

        public static void Start()
        {
            InitializeGl();
        }

        public static unsafe void Run()
        {
            glEnable(GL_DEPTH_TEST);

            camera = new Camera(WINDOW_WIDTH, WINDOW_HEIGHT, new vec3(0, 0, 2));

            while (!Glfw.WindowShouldClose(mainWindow))
            {
                SetBackgroundColor(Color.VeryDarkBlue);

                camera.Inputs(mainWindow);
                camera.UpdateMatrix(45, 0.1f, 100);

                plane.Draw(shaderProgram, camera);
                light.Draw(lightShader, camera);

                Glfw.SwapBuffers(mainWindow);

                Glfw.PollEvents();
            }
        }

        public static void End()
        {
            shaderProgram.Delete();
            lightShader.Delete();

            Glfw.DestroyWindow(mainWindow);
            Glfw.Terminate();
        }

        private static void InitializeGl()
        {
            Glfw.Init();

            SetupHints();

            mainWindow = Glfw.CreateWindow(WINDOW_WIDTH, WINDOW_HEIGHT, WINDOW_TITLE, Monitor.None, Window.None);
            Glfw.MakeContextCurrent(mainWindow);

            Import(Glfw.GetProcAddress);
            glViewport(0, 0, WINDOW_WIDTH, WINDOW_HEIGHT);

            SetupShaders();
            SetBackgroundColor(Color.VeryDarkBlue);

            Glfw.SwapBuffers(mainWindow);
        }

        private static unsafe void SetupShaders()
        {
            shaderProgram = new Shader("Shaders/Defaults/default.vert", "Shaders/Defaults/default.frag");

            lightShader = new Shader("Shaders/Defaults/light.vert", "Shaders/Defaults/light.frag");

            Texture[] textures =
            {
                new Texture("Textures/Images/planks.png", "diffuse", 0, GL_RGBA),
                new Texture("Textures/Images/planksSpec.png", "specular", 1, GL_RED)
            };

            plane = new Mesh(vertices, indices, textures);
            light = new Mesh(lightVertices, lightIndices, textures, MeshType.Light);

            vec4 lightColor = new vec4(1f, 1f, 1f, 1f);
            vec3 lightPos = new vec3(0.5f, 0.5f, 0.5f);
            mat4 lightModel = new mat4(1f);
            lightModel = glm.translate(lightModel, lightPos);

            vec3 objectPos = new vec3(0f, 0f, 0f);
            mat4 objectModel = new mat4(1f);
            objectModel = glm.translate(objectModel, objectPos);

            lightShader.Activate();
            glUniformMatrix4fv(glGetUniformLocation(lightShader.ID, "model"), 1, false, lightModel.to_array());
            glUniform4f(glGetUniformLocation(lightShader.ID, "lightColor"), lightColor.x, lightColor.y, lightColor.z, lightColor.w);
            shaderProgram.Activate();
            glUniformMatrix4fv(glGetUniformLocation(shaderProgram.ID, "model"), 1, false, objectModel.to_array());
            glUniform4f(glGetUniformLocation(shaderProgram.ID, "lightColor"), lightColor.x, lightColor.y, lightColor.z, lightColor.w);
            glUniform3f(glGetUniformLocation(shaderProgram.ID, "lightPos"), lightPos.x, lightPos.y, lightPos.z);
        }

        private static void SetupHints()
        {
            Glfw.WindowHint(Hint.ContextVersionMajor, 3);
            Glfw.WindowHint(Hint.ContextVersionMinor, 3);
            Glfw.WindowHint(Hint.OpenglProfile, Profile.Core);
        }

        private static void SetBackgroundColor(Color color)
        {
            glClearColor(color.RNorm, color.GNorm, color.BNorm, color.ANorm);
            glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
        }
    }
}
