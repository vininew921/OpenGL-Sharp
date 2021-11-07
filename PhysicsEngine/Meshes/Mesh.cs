using PhysicsEngine.RenderCamera;
using PhysicsEngine.Shaders;
using PhysicsEngine.Textures;
using System;
using static OpenGL.GL;

namespace PhysicsEngine.Meshes
{
    public class Mesh
    {
        public float[] Vertices;
        public uint[] Indices;
        public Texture[] Textures;
        public VAO VAO;

        public enum MeshType
        {
            Regular,
            Light
        }

        public unsafe Mesh(float[] vertices, uint[] indices, Texture[] textures, MeshType type = MeshType.Regular)
        {
            Vertices = vertices;
            Indices = indices;
            Textures = textures;

            VAO = new VAO();
            VAO.Bind();
            VBO VBO = new VBO(Vertices);
            EBO EBO = new EBO(Indices);

            switch (type)
            {
                case MeshType.Regular:
                    VAO.LinkAttrib(VBO, 0, 3, GL_FLOAT, 11 * sizeof(float), (void*)0);
                    VAO.LinkAttrib(VBO, 1, 3, GL_FLOAT, 11 * sizeof(float), (void*)(3 * sizeof(float)));
                    VAO.LinkAttrib(VBO, 2, 3, GL_FLOAT, 11 * sizeof(float), (void*)(6 * sizeof(float)));
                    VAO.LinkAttrib(VBO, 3, 2, GL_FLOAT, 11 * sizeof(float), (void*)(8 * sizeof(float)));
                    break;
                case MeshType.Light:
                    VAO.LinkAttrib(VBO, 0, 3, GL_FLOAT, 3 * sizeof(float), (void*)0);
                    break;
            }
            
            VAO.Unbind();
            VBO.Unbind();
            EBO.Unbind();
        }

        public unsafe void Draw(Shader shader, Camera camera)
        {
            shader.Activate();
            VAO.Bind();

            uint numDiffuse = 0;
            uint numSpecular = 0;

            for(int i = 0; i < Textures.Length; i++)
            {
                string num;
                string type = Textures[i].Type;

                if(type == "diffuse")
                {
                    num = numDiffuse.ToString();
                    numDiffuse++;
                }
                else
                {
                    num = numSpecular.ToString();
                    numSpecular++;
                }

                Textures[i].TexUnit(shader, type + num, i);
                Textures[i].Bind();
            }

            glUniform3f(glGetUniformLocation(shader.ID, "camPos"), camera.Position.x, camera.Position.y, camera.Position.z);
            camera.Matrix(shader, "camMatrix");

            glDrawElements(GL_TRIANGLES, Indices.Length, GL_UNSIGNED_INT, IntPtr.Zero.ToPointer());
        }
    }
}
