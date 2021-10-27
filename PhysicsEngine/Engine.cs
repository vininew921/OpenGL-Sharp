using GLFW;
using GlmNet;
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
        private static int uniScale;
        private static Texture madge;

        private static float[] vertices =
        {
            -0.5f, 0, 0.5f, 0.83f, 0.7f, 0.44f, 0, 0,
            -0.5f, 0, -0.5f, 0.83f, 0.7f, 0.44f, 5, 0,
            0.5f, 0, -0.5f, 0.83f, 0.7f, 0.44f, 0, 0,
            0.5f, 0, 0.5f, 0.83f, 0.7f, 0.44f, 5, 0,
            0, 0.8f, 0, 0.92f, 0.86f, 0.76f, 2.5f, 5
        };

        private static uint[] indices =
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
            float rotation = 0;
            double prevTime = Glfw.Time;

            glEnable(GL_DEPTH_TEST);

            while (!Glfw.WindowShouldClose(mainWindow))
            {
                SetBackgroundColor(Color.VeryDarkBlue);

                shaderProgram.Activate();

                double currentTime = Glfw.Time;
                if(currentTime - prevTime >= 1 / 60)
                {
                    rotation += 0.5f;
                    prevTime = currentTime;
                }

                mat4 model = new mat4(1);
                mat4 view = new mat4(1);
                mat4 proj = new mat4(1);

                model = glm.rotate(model, glm.radians(rotation), new vec3(0, 1, 0));
                view = glm.translate(view, new vec3(0, -0.5f, -2));
                proj = glm.perspective(glm.radians(45), WINDOW_WIDTH / WINDOW_HEIGHT, 0.1f, 100);

                int modelLoc = glGetUniformLocation(shaderProgram.ID, "model");
                glUniformMatrix4fv(modelLoc, 1, false, model.to_array());

                int viewLoc = glGetUniformLocation(shaderProgram.ID, "view");
                glUniformMatrix4fv(viewLoc, 1, false, view.to_array());

                int projLoc = glGetUniformLocation(shaderProgram.ID, "proj");
                glUniformMatrix4fv(projLoc, 1, false, proj.to_array());

                glUniform1f(uniScale, 0.5f);
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

            uniScale = glGetUniformLocation(shaderProgram.ID, "scale");

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
