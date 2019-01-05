using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using MonoGame.Extended;

namespace PhysicsPractice {
    public class Main : Game {
        public static SpriteFont font { get; private set; }
        public static GraphicsDeviceManager graphics { get; private set; }
        public static readonly int height = 600;
        public static readonly int width = 800;

        private SpriteBatch spriteBatch;
        private readonly Random random = new Random();
        private KeyboardState prevKeyboardState;

        private Mover[] movers = new Mover[5];
        private Liquid[] liquids = new Liquid[1];

        public Main() {
            Main.graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            Main.graphics.PreferredBackBufferHeight = Main.height;
            Main.graphics.PreferredBackBufferWidth = Main.width;
        }

        protected override void Initialize() {
            base.Initialize();

            this.IsMouseVisible = true;

            // Create movers
            for(int i = 0; i < this.movers.Length; i++) {
                float mass = RandomExtensions.NextSingle(random, 0.5f, 3);
                float radius = mass * 16;
                this.movers[i] = new Mover(mass,
                    RandomExtensions.NextSingle(random, 0, Main.width - radius),
                    //RandomExtensions.NextSingle(random, 0, this.graphics.PreferredBackBufferHeight));
                    0,
                    radius);
            }

            // Create liquids
            this.liquids[0] = new Liquid(0.1f, 0, Main.height/2, Main.width, Main.height/2);
        }

        protected override void LoadContent() {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Main.font = Content.Load<SpriteFont>("font");
        }

        protected override void UnloadContent() {
            
        }

        protected override void Update(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (Keyboard.GetState().IsKeyUp(Keys.R) && this.prevKeyboardState.IsKeyDown(Keys.R))
                this.Initialize();

            foreach(Mover mover in this.movers) {
                // Mover -> Liquid collision detection
                foreach(Liquid liquid in this.liquids) {
                    if(liquid.Contains(mover)) {
                        Vector2 dragForce = liquid.Drag(mover);
                        mover.ApplyForce(dragForce);
                    }
                }

                // Calculate gravity according to mass
                Vector2 gravity = new Vector2(0, 0.1f * mover.mass);
                mover.ApplyForce(gravity);

                // Make mover move
                mover.Update();

                // Mover -> Screen bounds collision detection
                mover.CheckEdges(Main.height);
            }

            base.Update(gameTime);
            this.prevKeyboardState = Keyboard.GetState();
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(new Color(45, 52, 54));

            foreach (Mover mover in this.movers) {
                mover.Draw(spriteBatch);
            }

            foreach (Liquid liquid in this.liquids) {
                liquid.Draw(spriteBatch);
            }

            spriteBatch.Begin();

            spriteBatch.DrawString(Main.font, "Press R to restart", new Vector2(5, 5), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
