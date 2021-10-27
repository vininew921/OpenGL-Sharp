using GLFW;
using GlmNet;
using PhysicsEngine.RenderCamera;
using PhysicsEngine.Shaders;
using PhysicsEngine.Structs;
using PhysicsEngine.Textures;
using System;
using static OpenGL.GL;

namespace PhysicsEngine
{
    public static class Engine
    {
        private static Window mainWindow;
        private static VAO vao;
        private static VBO vbo;
        private static EBO ebo;
        private static Shader shaderProgram;
        private static readonly int uniScale;
        private static Texture madge;
        private static Camera camera;

        private static readonly float[] vertices =
        {
            -0.5f, 0, 0.5f, 0.83f, 0.7f, 0.44f, 0, 0,
            -0.5f, 0, -0.5f, 0.83f, 0.7f, 0.44f, 5, 0,
            0.5f, 0, -0.5f, 0.83f, 0.7f, 0.44f, 0, 0,
            0.5f, 0, 0.5f, 0.83f, 0.7f, 0.44f, 5, 0,
            0, 0.8f, 0, 0.92f, 0.86f, 0.76f, 2.5f, 5
        };

        private static readonly uint[] indices =
        {
            0, 1, 2,
            0, 2, 3,
            0, 1, 4,
            1, 2, 4,
            2, 3, 4,
            3, 0, 4
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

                shaderProgram.Activate();

                camera.Inputs(mainWindow);
                camera.Matrix(45, 0.1f, 100, shaderProgram, "camMatrix");

                madge.Bind();
                vao.Bind();

                glDrawElements(GL_TRIANGLES, indices.Length, GL_UNSIGNED_INT, IntPtr.Zero.ToPointer());

                Glfw.SwapBuffers(mainWindow);

                Glfw.PollEvents();
            }
        }

        public static void End()
        {
            vao.Delete();
            vbo.Delete();
            ebo.Delete();
            shaderProgram.Delete();

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

            vao = new VAO();
            vbo = new VBO(vertices);
            ebo = new EBO(indices);

            vao.LinkAttrib(vbo, 0, 3, GL_FLOAT, 8 * sizeof(float), (void*)0);
            vao.LinkAttrib(vbo, 1, 3, GL_FLOAT, 8 * sizeof(float), (void*)(3 * sizeof(float)));
            vao.LinkAttrib(vbo, 2, 2, GL_FLOAT, 8 * sizeof(float), (void*)(6 * sizeof(float)));
            vao.Unbind();
            vbo.Unbind();
            ebo.Unbind();

            madge = new Texture("Textures/Images/Madge.png", GL_TEXTURE_2D, GL_TEXTURE0, GL_RGBA);
            madge.TexUnit(shaderProgram, "tex0", 0);
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
