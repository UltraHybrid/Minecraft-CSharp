using OpenTK;

namespace tmp
{
    public static class Extensions
    {
        public static Vector3 Convert(this PointI point)
        {
            return new Vector3(point.X, point.Y, point.Z);
        }
    }
}