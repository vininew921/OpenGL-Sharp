using GlmNet;

namespace PhysicsEngine.Glm
{
    public static class GlmHelper
    {
        public static vec3 Rotate(vec3 v, float angle, vec3 normal)
        {
            return glm.rotate(angle, normal).to_mat3() * v;
        }

        public static float Angle(vec3 a, vec3 b)
        {
            vec3 normA = glm.normalize(a);
            vec3 normB = glm.normalize(b);

            return glm.acos(glm.dot(normA, normB));
        }
    }
}
