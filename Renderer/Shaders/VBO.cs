using static OpenGL.GL;

namespace Renderer.Shaders
{
    public class VBO
    {
        public uint ID { get; set; }

        public unsafe VBO(float[] vertices)
        {
            ID = glGenBuffer();
            Bind();

            fixed (float* v = &vertices[0])
            {
                glBufferData(GL_ARRAY_BUFFER, sizeof(float) * vertices.Length, v, GL_STATIC_DRAW);
            }
        }

        public void Bind() => glBindBuffer(GL_ARRAY_BUFFER, ID);

        public static void Unbind() => glBindBuffer(GL_ARRAY_BUFFER, 0);

        public void Delete() => glDeleteBuffer(ID);
    }
}
