using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
//using MonoGameLibrary;

namespace _3D_Game_Test
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;


        Vector3 camForward;
        Vector3 camRight;

        Vector3 camPosition;


        Matrix projectionMatrix;
        Matrix viewMatrix;
        Matrix worldMatrix;

        Model model;
        Model chiyoModel;

        //Shader

        Vector3 sunLightDirection;
        Color sunLightColor = Color.White;
        float sunLightIntensity;





        float playerSpeed = 0.2f;
        float turnSpeed = 0.8f;

        bool orbit = false;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();

            camForward = new Vector3(0f, 0f, 1f);
            camPosition = new Vector3(0f, 0f, -10f);
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                               MathHelper.ToRadians(45f),
                               GraphicsDevice.DisplayMode.AspectRatio,
                1f, 1000f);
            viewMatrix = Matrix.CreateLookAt(camPosition, camForward,
                         new Vector3(0f, 1f, 0f));// Y up
            worldMatrix = Matrix.CreateWorld(camForward, Vector3.
                          Forward, Vector3.Up);

            
        }


        protected override void LoadContent()
        {
            model = Content.Load<Model>("MonoCube");
            chiyoModel = Content.Load<Model>("models/characters/chiyo_chan/chiyo");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                //camPosition.X += 1f * playerSpeed;
                camPosition += Vector3.Normalize(Vector3.Cross(Vector3.Up, camForward)) * playerSpeed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                camPosition += Vector3.Normalize(Vector3.Cross(Vector3.Up, camForward)) * -playerSpeed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                camPosition.Y += 1f * playerSpeed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.LeftShift))
            {
                camPosition.Y -= 1f * playerSpeed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                camPosition += camForward * playerSpeed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                camPosition += camForward * -playerSpeed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Q))
            {
                Matrix rotationMatrix = Matrix.CreateRotationY(MathHelper.ToRadians(turnSpeed));
                camForward = Vector3.Transform(camForward, rotationMatrix);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.E))
            {
                Matrix rotationMatrix = Matrix.CreateRotationY(MathHelper.ToRadians(-turnSpeed));
                camForward = Vector3.Transform(camForward, rotationMatrix);
            }

            viewMatrix = Matrix.CreateLookAt(camPosition, camPosition + camForward, Vector3.Up);



            base.Update(gameTime);


        }

        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.CornflowerBlue);

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects) {
                    effect.View = viewMatrix;
                    effect.World = worldMatrix;
                    effect.Projection = projectionMatrix;
                    mesh.Draw();
                }
            }
            foreach (ModelMesh mesh in chiyoModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.View = viewMatrix;
                    effect.World = worldMatrix * Matrix.CreateTranslation(new Vector3(0, 2, 0));
                    effect.Projection = projectionMatrix;
                    mesh.Draw();
                }
            }

            base.Draw(gameTime);
        }
    }
}
