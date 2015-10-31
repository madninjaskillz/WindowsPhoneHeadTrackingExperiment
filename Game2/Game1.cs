using System.Collections.Generic;
using System.Linq;
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

            shapes.Add(new Cuboid(new Vector3(0, 0, 0), new Vector3(2, 2, 2), graphics.GraphicsDevice));
            shapes.Add(new Cuboid(new Vector3(3, 3, 3), new Vector3(2, 1, 2), graphics.GraphicsDevice));
            shapes.Add(new Cuboid(new Vector3(0, 0, -3), new Vector3(1, 1, 2), graphics.GraphicsDevice));
            //    RenderToDevice(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            shapes.Last().Position = shapes.First().Position + new Vector3(1, 0.1f, 0);
            shapes.First().Rotation = shapes.First().Rotation + new Vector3(1f, 0, 0);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            effect.World = Matrix.CreateRotationY(MathHelper.ToRadians(rotation)) * Matrix.CreateRotationX(MathHelper.ToRadians(rotation)) * Matrix.CreateTranslation(shapes[1].Position);
            effect.View = Matrix.CreateLookAt(cameraPosition, shapes[1].Position, Vector3.Up);

            effect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, 1080/(1920f/2f), 1.0f, 1000.0f);
            //  effect.TextureEnabled = true;
            //  effect.Texture = cubeTexture;
            //     effect.EnableDefaultLighting();
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
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                foreach (ThreeDimensionalShape threeDimensionalShape in shapes)
                {
                    threeDimensionalShape.RenderToDevice(graphics.GraphicsDevice, effect);
                }

            }
        }
    }
}

