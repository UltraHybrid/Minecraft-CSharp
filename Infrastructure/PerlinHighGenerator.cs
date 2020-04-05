using System;

namespace tmp
{
    public class PerlinHighGenerator : IGenerator<byte>
    {
        private readonly float rndParameter;
        private readonly float persistence;
        private readonly float frequency;
        private readonly float amplitude;
        private readonly int amountOctaves;

        public PerlinHighGenerator(float persistence, float frequency,
            float amplitude, int amountOctaves)
        {
            this.persistence = persistence;
            this.frequency = frequency;
            this.amplitude = amplitude;
            this.amountOctaves = amountOctaves;
            rndParameter = (float) (20 * Math.PI + new Random().NextDouble() * 30 * Math.PI);
        }

        public byte Generate(int x, int z)
        {
            return GetPerlinNoise(x, z, rndParameter);
        }

        private byte GetPerlinNoise(float x, float z, float factor)
        {
            var total = 0f;
            var persistence = this.persistence; //неизменность
            var frequency = this.frequency; // частота, частотность
            var amplitude = this.amplitude;
            x += factor;
            z += factor;
            for (var i = 0; i < amountOctaves; i++)
            {
                total += GeneralNoise(x * frequency, z * frequency) * amplitude;
                amplitude *= persistence;
                frequency *= 2;
            }

            total = Math.Abs(total);
            var res = (byte) (total * 22.0f); //значение 0-255
            return res;
        }

        private float Get2DNoise(float x, float z)
        {
            var n = (int) x + (int) z * 57;
            n = (n << 13) ^ n;
            return (1.0f - ((n * (n * n * 15731 + 789221) + 1376312589) & 0x7fffffff) /
                1073741824.0f);
        }

        private float SmoothedNoise(float x, float z)
        {
            var corners = (Get2DNoise(x - 1, z - 1) + Get2DNoise(x + 1, z - 1) +
                           Get2DNoise(x - 1, z + 1) + Get2DNoise(x + 1, z + 1)) / 16;
            var sides = (Get2DNoise(x - 1, z) + Get2DNoise(x + 1, z) +
                         Get2DNoise(x, z - 1) + Get2DNoise(x, z + 1)) / 8;
            var center = Get2DNoise(x, z) / 4;
            return corners + sides + center;
        }

        private float CosineInterpolate(float min, float max, float value)
        {
            var phi = value * Math.PI;
            var interpolation = (float) ((1 - Math.Cos(phi)) * 0.5);
            return min * (1 - interpolation) + max * interpolation;
        }

        private float GeneralNoise(float x, float z)
        {
            var xI = (int) x;
            var zI = (int) z;
            var xFraction = x - xI;
            var zFraction = z - zI;

            var v1 = SmoothedNoise(xI, zI);
            var v2 = SmoothedNoise(xI + 1, zI);
            var v3 = SmoothedNoise(xI, zI + 1);
            var v4 = SmoothedNoise(xI + 1, zI + 1);

            var i1 = CosineInterpolate(v1, v2, xFraction);
            var i2 = CosineInterpolate(v3, v4, xFraction);
            return CosineInterpolate(i1, i2, zFraction);
        }
    }
}