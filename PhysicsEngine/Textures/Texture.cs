using PhysicsEngine.Shaders;
using StbiSharp;
using System.IO;
using static OpenGL.GL;

namespace PhysicsEngine.Textures
{
    public class Texture
    {
        public uint ID { get; set; }
        public int Type { get; set; }

        public unsafe Texture(string imagePath, int texType, int slot, int format)
        {
            Type = texType;
            ID = glGenTexture();

            glActiveTexture(slot);
            Bind();

            glTexParameteri(texType, GL_TEXTURE_MIN_FILTER, GL_NEAREST);
            glTexParameteri(texType, GL_TEXTURE_MAG_FILTER, GL_NEAREST);
            glTexParameteri(texType, GL_TEXTURE_WRAP_S, GL_REPEAT);
            glTexParameteri(texType, GL_TEXTURE_WRAP_T, GL_REPEAT);

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
                        glTexImage2D(texType, 0, GL_RGBA, img.Width, img.Height, 0, format, GL_UNSIGNED_BYTE, v);
                    }
                }
            }

            glGenerateMipmap(texType);
            glBindTexture(texType, 0);
        }

        public void TexUnit(Shader shader, string uniform, int unit)
        {
            int texUni = glGetUniformLocation(shader.ID, uniform);
            shader.Activate();
            glUniform1i(texUni, unit);
        }

        public void Bind()
        {
            glBindTexture(Type, ID);
        }

        public void Unbind()
        {
            glBindTexture(Type, 0);
        }

        public void Delete()
        {
            glDeleteTexture(ID);
        }
    }
}
