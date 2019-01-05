using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicsPractice {
    class Mover {
        public Vector2 position { get; private set; }
        public Vector2 velocity { get; private set; }
        public Vector2 acceleration { get; private set; }
        public float radius { get; private set; }
        public float mass { get; private set; }

        public Mover(float mass, float x, float y, float radius) {
            this.mass = mass;
            this.position = new Vector2(x, y);
            this.radius = radius;
            this.velocity = Vector2.Zero;
            this.acceleration = Vector2.Zero;
        }

        // Newton's 2nd law: F = M * A
        // or A = F / M
        public void ApplyForce(Vector2 force) {
            Vector2 _force = Vector2.Divide(force, this.mass);
            this.acceleration += _force;
        }

        public void Update() {
            this.velocity += this.acceleration;
            this.position += this.velocity;
            this.acceleration *= 0; // Clear the acceleration
        }

        public void Draw(SpriteBatch batch) {
            batch.Begin();

            batch.DrawCircle(this.position, this.radius, 32, Color.Red, this.radius);
            //batch.DrawString(Main.font, $"Y = {this.position.Y}", new Vector2(this.position.X, this.position.Y - 15), Color.White);

            batch.End();
        }

        public void CheckEdges(float maxHeight) {
            if((position.Y + this.radius) > maxHeight) {
                this.velocity = new Vector2(this.velocity.X, this.velocity.Y * -0.9f);
                this.position = new Vector2(this.position.X, maxHeight - this.radius);
            }
        }
    }
}
