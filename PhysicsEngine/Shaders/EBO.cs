using static OpenGL.GL;

namespace PhysicsEngine.Shaders
{
    public class EBO
    {
        public uint ID { get; set; }

        public unsafe EBO(uint[] indices)
        {
            ID = glGenBuffer();
            Bind();

            fixed (uint* v = &indices[0])
            {
                glBufferData(GL_ELEMENT_ARRAY_BUFFER, sizeof(uint) * indices.Length, v, GL_STATIC_DRAW);
            }
        }

        public void Bind()
        {
            glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, ID);
        }

        public void Unbind()
        {
            glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, 0);
        }

        public void Delete()
        {
            glDeleteBuffer(ID);
        }
    }
}
