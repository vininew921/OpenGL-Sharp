using System;
using System.IO;
using static OpenGL.GL;

namespace PhysicsEngine.Shaders
{
    public class Shader
    {
        public uint ID { get; set; }
        public uint UniScale { get; set; }

        public Shader(string vertexFile, string fragmentFile)
        {
            string vertexSource = File.ReadAllText(vertexFile);
            string fragmentSource = File.ReadAllText(fragmentFile);

            uint vertexShader = glCreateShader(GL_VERTEX_SHADER);
            glShaderSource(vertexShader, vertexSource);
            glCompileShader(vertexShader);
            CompileErrors(vertexShader, "VERTEX");

            uint fragmentShader = glCreateShader(GL_FRAGMENT_SHADER);
            glShaderSource(fragmentShader, fragmentSource);
            glCompileShader(fragmentShader);
            CompileErrors(fragmentShader, "FRAGMENT");

            ID = glCreateProgram();
            glAttachShader(ID, vertexShader);
            glAttachShader(ID, fragmentShader);

            glLinkProgram(ID);
            CompileErrors(ID, "PROGRAM");

            glDeleteShader(vertexShader);
            glDeleteShader(fragmentShader);
        }

        public void Activate() => glUseProgram(ID);

        public void Delete() => glDeleteProgram(ID);

        public static unsafe void CompileErrors(uint shader, string type)
        {
            int hasCompiled;

            if (type != "PROGRAM")
            {
                glGetShaderiv(shader, GL_COMPILE_STATUS, &hasCompiled);

                if (hasCompiled == GL_FALSE)
                {
                    glGetShaderInfoLog(shader, 1024);
                    Console.WriteLine($"SHADER_COMPILATION_ERROR for: {type}");
                }
            }
            else
            {
                glGetShaderiv(shader, GL_COMPILE_STATUS, &hasCompiled);

                if (hasCompiled == GL_FALSE)
                {
                    glGetProgramInfoLog(shader, 1024);
                    Console.WriteLine($"SHADER_LINKING_ERROR for: {type}");
                }
            }
        }
    }
}
