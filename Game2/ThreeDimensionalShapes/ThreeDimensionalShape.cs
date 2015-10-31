using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game2.ThreeDimensionalShapes
{
    public abstract class ThreeDimensionalShape
    {
        public abstract void RenderToDevice(GraphicsDevice device, BasicEffect effect);
        public abstract Vector3 Position { get; set; }
        public abstract Vector3 Rotation { get; set; }

        public abstract Vector3 Size { get; set; }
    }
}
