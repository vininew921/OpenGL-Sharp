using static OpenGL.GL;

namespace PhysicsEngine.Shaders
{
    public class VAO
    {
        public uint ID { get; set; }

        public VAO()
        {
            ID = glGenVertexArray();
        }

        public unsafe void LinkAttrib(VBO vbo, uint layout, int numComponents, int type, int stride, void* offset)
        {
            vbo.Bind();

            glVertexAttribPointer(layout, numComponents, type, false, stride, offset);
            glEnableVertexAttribArray(layout);

            vbo.Unbind();
        }

        public void Bind()
        {
            glBindVertexArray(ID);
        }

        public void Unbind()
        {
            glBindVertexArray(0);
        }

        public void Delete()
        {
            glDeleteVertexArray(ID);
        }
    }
}
