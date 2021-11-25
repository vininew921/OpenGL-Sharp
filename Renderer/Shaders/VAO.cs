using static OpenGL.GL;

namespace Renderer.Shaders
{
    public class VAO
    {
        public uint ID { get; set; }

        public VAO()
        {
            ID = glGenVertexArray();
        }

        public static unsafe void LinkAttrib(VBO vbo, uint layout, int numComponents, int type, int stride, void* offset)
        {
            vbo.Bind();

            glVertexAttribPointer(layout, numComponents, type, false, stride, offset);
            glEnableVertexAttribArray(layout);

            VBO.Unbind();
        }

        public void Bind() => glBindVertexArray(ID);

        public static void Unbind() => glBindVertexArray(0);

        public void Delete() => glDeleteVertexArray(ID);
    }
}
