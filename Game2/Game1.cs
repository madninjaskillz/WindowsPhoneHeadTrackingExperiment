using System.Collections.Generic;
using System.Linq;
using Windows.Devices.Sensors;
using Game2.ThreeDimensionalShapes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game2
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        Vector3 cameraPosition = new Vector3(0.0f, 0.0f, 50.0f);
        Model myModel;
        float aspectRatio;
        List<ThreeDimensionalShape> shapes = new List<ThreeDimensionalShape>();
        private BasicEffect effect;
        private float rotation = 0f;
        private Viewport leftEye;
        private Viewport rightEye;

        private double zeroPitch;
        private double zeroYaw;
        private double zeroRoll;

        private bool zeroSet = false;

        public double PRotX;
        public double PRotY;
        public double PRotZ;

        public double RotPitch;
        public double RotYaw;
        public double RotRoll;

        public SensorRotationMatrix zeroMatrix;

        public SensorRotationMatrix cameraMatrix;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.SynchronizeWithVerticalRetrace = false;
            IsFixedTimeStep = false;
            graphics.ApplyChanges();
        }

        protected override void LoadContent()
        {
            effect = new BasicEffect(graphics.GraphicsDevice);
            effect.AmbientLightColor = Vector3.One;
            effect.DirectionalLight0.Enabled = true;
            effect.DirectionalLight0.DiffuseColor = Vector3.One;
            effect.DirectionalLight0.Direction = Vector3.Normalize(Vector3.One);
            aspectRatio = graphics.GraphicsDevice.Viewport.AspectRatio;

            leftEye = new Viewport()
            {
                X = 0,
                Y = 0,
                Width = 1920 / 2,
                Height = 1080
            };

            rightEye = new Viewport()
            {
                X = 1920 / 2,
                Y = 0,
                Width = 1920 / 2,
                Height = 1080
            };

            shapes.Add(new Cuboid(new Vector3(0, 0, 30), new Vector3(2, 2, 2), graphics.GraphicsDevice));
            shapes.Add(new Cuboid(new Vector3(3, 3, 33), new Vector3(1, 1, 1), graphics.GraphicsDevice));
            shapes.Add(new Cuboid(new Vector3(-3, -3, 27), new Vector3(1, 1, 5), graphics.GraphicsDevice));
            //    RenderToDevice(GraphicsDevice);
        }

        public void ResetHome()
        {
            var incl = OrientationSensor.GetDefault();


            OrientationSensorReading inc = incl.GetCurrentReading();
            if (inc != null)
            {
                zeroSet = true;

                zeroMatrix = inc.RotationMatrix;

            }

        }

        private bool debug = false;
        protected override void Update(GameTime gameTime)
        {
            if (!zeroSet)
            {
                ResetHome();
            }
            else
            {
                var incl = OrientationSensor.GetDefault();
                var inc = incl.GetCurrentReading();
                if (inc != null)
                {
                    cameraMatrix = inc.RotationMatrix;
                }
            }
            // shapes.Last().Position = shapes.First().Position + new Vector3(0.01f, 0.1f, 0);
            shapes.Last().Rotation = shapes.Last().Rotation + new Vector3(0,0, 0.01f);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            if (cameraMatrix == null)
            {
                return;
            }
            //   effect.World = Matrix.CreateRotationY(MathHelper.ToRadians(rotation)) * Matrix.CreateRotationX(MathHelper.ToRadians(rotation)) * Matrix.CreateTranslation(shapes[1].Position);
            //            effect.View = Matrix.CreateLookAt(cameraPosition, shapes.Last().Position, Vector3.Up);

            Matrix temp = cameraMatrix.ToXnaMatrix() - zeroMatrix.ToXnaMatrix();
            effect.View = Matrix.Invert(cameraMatrix.ToXnaMatrix());
            //Matrix.CreateFromYawPitchRoll(MathHelper.ToRadians((float)RotYaw), MathHelper.ToRadians((float)RotPitch), MathHelper.ToRadians((float)RotRoll));
            effect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, (1920f / 2f) / 1080, 1.0f, 1000.0f);
            //  effect.TextureEnabled = true;
            //  effect.Texture = cubeTexture;
            effect.EnableDefaultLighting();
            Viewport original = graphics.GraphicsDevice.Viewport;

            graphics.GraphicsDevice.Viewport = leftEye;
            DrawEye();

            graphics.GraphicsDevice.Viewport = rightEye;
            DrawEye();

            graphics.GraphicsDevice.Viewport = original;

            base.Draw(gameTime);
        }

        public void DrawEye()
        {

            foreach (ThreeDimensionalShape threeDimensionalShape in shapes)
            {
                effect.World = threeDimensionalShape.World;
                foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                {
                    pass.Apply();

                    threeDimensionalShape.RenderToDevice(graphics.GraphicsDevice, effect);
                }

            }
        }
    }
}

