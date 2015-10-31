using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Sensors;
using Microsoft.Xna.Framework;

namespace Game2
{
    public static class Extensions
    {
        public static Matrix ToXnaMatrix(this SensorRotationMatrix cameraMatrix)
        {
            float[,] m = new float[3,3];

            m[0, 0] = cameraMatrix.M11;
            m[1, 0] = cameraMatrix.M12;
            m[2, 0] = cameraMatrix.M13;

            m[0, 1] = cameraMatrix.M21;
            m[1, 1] = cameraMatrix.M22;
            m[2, 1] = cameraMatrix.M23;

            m[0, 2] = cameraMatrix.M31;
            m[1, 2] = cameraMatrix.M32;
            m[2, 2] = cameraMatrix.M33;

            int r1 = 0;
            int r2 = 2;
            int r3 = 1;

            int c1 = 1;
            int c2 = 0;
            int c3 = 2;

            return new Matrix(
                -m[c1, r1], m[c2, r1], m[c3, r1],0,
                -m[c1, r2], m[c2, r2], m[c3, r2],0,
                -m[c1, r3], m[c2, r3], m[c3, r3],0,
                0, 0, 0, 1
                );
        }
    }
}
