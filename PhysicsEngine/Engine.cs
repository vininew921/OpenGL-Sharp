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
        private static VAO renderVao;
        private static VBO renderVbo;
        private static EBO renderEbo;
        private static VAO lightVao;
        private static VBO lightVbo;
        private static EBO lightEbo;
        private static Shader shaderProgram;
        private static Shader lightShader;
        private static Texture planeTex;
        private static Texture planeSpec;
        private static Camera camera;

        private static readonly float[] vertices =
        {
            -1.0f, 0.0f,  1.0f,     0.0f, 0.0f, 0.0f,       0.0f, 0.0f,     0.0f, 1.0f, 0.0f,
            -1.0f, 0.0f, -1.0f,     0.0f, 0.0f, 0.0f,       0.0f, 1.0f,     0.0f, 1.0f, 0.0f,
             1.0f, 0.0f, -1.0f,     0.0f, 0.0f, 0.0f,       1.0f, 1.0f,     0.0f, 1.0f, 0.0f,
             1.0f, 0.0f,  1.0f,     0.0f, 0.0f, 0.0f,       1.0f, 0.0f,     0.0f, 1.0f, 0.0f
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

                shaderProgram.Activate();
                glUniform3f(glGetUniformLocation(shaderProgram.ID, "camPos"), camera.Position.x, camera.Position.y, camera.Position.z);

                camera.Matrix(shaderProgram, "camMatrix");

                planeTex.Bind();
                planeSpec.Bind();
                renderVao.Bind();

                glDrawElements(GL_TRIANGLES, indices.Length, GL_UNSIGNED_INT, IntPtr.Zero.ToPointer());

                lightShader.Activate();

                camera.Matrix(lightShader, "camMatrix");
                lightVao.Bind();
                glDrawElements(GL_TRIANGLES, lightIndices.Length, GL_UNSIGNED_INT, IntPtr.Zero.ToPointer());

                Glfw.SwapBuffers(mainWindow);

                Glfw.PollEvents();
            }
        }

        public static void End()
        {
            renderVao.Delete();
            renderVbo.Delete();
            renderEbo.Delete();
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

            renderVao = new VAO();
            renderVao.Bind();
            renderVbo = new VBO(vertices);
            renderEbo = new EBO(indices);

            renderVao.LinkAttrib(renderVbo, 0, 3, GL_FLOAT, 11 * sizeof(float), (void*)0);
            renderVao.LinkAttrib(renderVbo, 1, 3, GL_FLOAT, 11 * sizeof(float), (void*)(3 * sizeof(float)));
            renderVao.LinkAttrib(renderVbo, 2, 2, GL_FLOAT, 11 * sizeof(float), (void*)(6 * sizeof(float)));
            renderVao.LinkAttrib(renderVbo, 3, 3, GL_FLOAT, 11 * sizeof(float), (void*)(8 * sizeof(float)));
            renderVao.Unbind();
            renderVbo.Unbind();
            renderEbo.Unbind();

            //Lightning
            lightShader = new Shader("Shaders/Defaults/light.vert", "Shaders/Defaults/light.frag");

            lightVao = new VAO();
            lightVao.Bind();
            lightVbo = new VBO(lightVertices);
            lightEbo = new EBO(lightIndices);

            lightVao.LinkAttrib(lightVbo, 0, 3, GL_FLOAT, 3 * sizeof(float), (void*)0);
            lightVao.Unbind();
            lightVbo.Unbind();
            lightEbo.Unbind();

            vec4 lightColor = new vec4(1f, 1f, 1f, 1f);
            vec3 lightPos = new vec3(0.5f, 0.5f, 0.5f);
            mat4 lightModel = new mat4(1f);
            lightModel = glm.translate(lightModel, lightPos);

            vec3 pyramidPos = new vec3(0f, 0f, 0f);
            mat4 pyramidModel = new mat4(1f);
            pyramidModel = glm.translate(pyramidModel, pyramidPos);

            lightShader.Activate();
            glUniformMatrix4fv(glGetUniformLocation(lightShader.ID, "model"), 1, false, lightModel.to_array());
            glUniform4f(glGetUniformLocation(lightShader.ID, "lightColor"), lightColor.x, lightColor.y, lightColor.z, lightColor.w);
            shaderProgram.Activate();
            glUniformMatrix4fv(glGetUniformLocation(shaderProgram.ID, "model"), 1, false, pyramidModel.to_array());
            glUniform4f(glGetUniformLocation(shaderProgram.ID, "lightColor"), lightColor.x, lightColor.y, lightColor.z, lightColor.w);
            glUniform3f(glGetUniformLocation(shaderProgram.ID, "lightPos"), lightPos.x, lightPos.y, lightPos.z);

            //Texture
            planeTex = new Texture("Textures/Images/planks.png", GL_TEXTURE_2D, 0, GL_RGBA);
            planeTex.TexUnit(shaderProgram, "tex0", 0);

            planeSpec = new Texture("Textures/Images/planksSpec.png", GL_TEXTURE_2D, 1, GL_RED);
            planeSpec.TexUnit(shaderProgram, "tex1", 1);
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
