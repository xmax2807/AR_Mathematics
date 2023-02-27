using UnityEngine;

namespace Project.Utils.ExtensionMethods
{
    public static class VectorExtensionMethods
    {
        public enum Direction
        {
            //Flags flag;
            // if ((flag & Flags.Value10) != 0) {
            //     // flag "value 10" set
            // }

            // // set a value
            // Flags flag;
            // flag |= Flags.Value15; // flag value 15 set

            // //unset a value
            // Flags flag;
            // flag &= ~Flags.Value15; // flag value 15 unset

            X = 1 << 0,
            Y = 1 << 1,
            Z = 1 << 2,
        }
        public static Vector3[] SpawnPoints(Vector3 source, Vector3 baseDir, float range, int count = 4)
        {
            baseDir = baseDir.ToDirection();

            Vector3[] result = new Vector3[count];

            for (int i = 0; i < count; i++)
            {
                result[i] = source + Randomize(range, baseDir);
            }
            return result;
        }
        public static Vector3 ToDirection(this Vector3 vector)
        {
            float x, y, z;
            x = vector.x != 0 ? vector.x / Mathf.Abs(vector.x) : 0;
            y = vector.y != 0 ? vector.y / Mathf.Abs(vector.y) : 0;
            z = vector.z != 0 ? vector.z / Mathf.Abs(vector.z) : 0;

            return new Vector3(x, y, z);
        }
        public static Vector3 Randomize(float range, Vector3 lockDir)
        {
            Vector3 result = new(
                Random.Range(-range, range),
                Random.Range(-range, range),
                Random.Range(-range, range)
            );

            result.Scale(lockDir);
            return result;
        }
        public static Vector3 Randomize(Vector3 origin, float range, Vector3 lockDir)
        {
            return origin + Randomize(range, lockDir);
        }
        public static void LookAtUpdate(this Transform current, Vector3 waypoint, float currentSpeed)
        {
            float turnSpeed = currentSpeed * Random.Range(1, 3);
            Vector3 LookAt = waypoint - current.position;
            current.rotation = Quaternion.Slerp(current.rotation, Quaternion.LookRotation(LookAt), turnSpeed * Time.deltaTime);
        }
        public static bool IsNear(this Vector3 current, Vector3 point, float maxRange = 0.5f)
        {
            return Vector3.Distance(current, point) <= maxRange;
        }
        public static bool IsBetween(this Vector3 target, Vector3 lowestPoint, Vector3 highestPoint)
        {
            bool isBetween = true;
            for (int i = 0; i < 3 && isBetween; ++i)
            {
                isBetween = isBetween && target[i] >= lowestPoint[i] && target[i] <= highestPoint[i];
            }
            return isBetween;
        }
    }
}