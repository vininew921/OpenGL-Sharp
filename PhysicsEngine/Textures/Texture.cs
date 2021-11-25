using PhysicsEngine.Shaders;
using StbiSharp;
using System.IO;
using static OpenGL.GL;

namespace PhysicsEngine.Textures
{
    public class Texture
    {
        public uint ID { get; set; }
        public int Unit { get; set; }
        public string Type { get; set; }

        public unsafe Texture(string imagePath, string texType, int slot, int format)
        {
            Type = texType;
            ID = glGenTexture();

            glActiveTexture(GL_TEXTURE0 + slot);
            Unit = slot;
            glBindTexture(GL_TEXTURE_2D, ID);

            glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_NEAREST);
            glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_NEAREST);
            glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_REPEAT);
            glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_REPEAT);

            using (FileStream stream = File.OpenRead(imagePath))
            {
                using (MemoryStream mStream = new MemoryStream())
                {
                    stream.CopyTo(mStream);
                    Stbi.SetFlipVerticallyOnLoad(true);
                    StbiImage img = Stbi.LoadFromMemory(mStream, 4);
                    byte[] bytes = img.Data.ToArray();

                    fixed (void* v = &bytes[0])
                    {
                        glTexImage2D(GL_TEXTURE_2D, 0, GL_RGBA, img.Width, img.Height, 0, format, GL_UNSIGNED_BYTE, v);
                    }
                }
            }

            glGenerateMipmap(GL_TEXTURE_2D);
            glBindTexture(GL_TEXTURE_2D, 0);
        }

        public void TexUnit(Shader shader, string uniform, int unit)
        {
            int texUni = glGetUniformLocation(shader.ID, uniform);
            shader.Activate();
            glUniform1i(texUni, unit);
        }

        public void Bind()
        {
            glActiveTexture(GL_TEXTURE0 + Unit);
            glBindTexture(GL_TEXTURE_2D, ID);
        }

        public void Unbind() => glBindTexture(GL_TEXTURE_2D, 0);

        public void Delete() => glDeleteTexture(ID);
    }
}
