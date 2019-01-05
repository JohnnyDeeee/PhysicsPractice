using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicsPractice {
    class Liquid {
        public Vector2 position { get; private set; }
        public float width { get; private set; }
        public float height { get; private set; }
        public float dragCoefficient { get; private set; }

        public Liquid(float dragCoefficient, float x, float y, float width, float height) {
            this.dragCoefficient = dragCoefficient;
            this.position = new Vector2(x, y);
            this.width = width;
            this.height = height;
        }

        public Boolean Contains(Mover mover) {
            Vector2 moverPos = mover.position;
            return (moverPos.X + mover.radius) > this.position.X && // Mover X > Liquid X
                (moverPos.X + mover.radius) < this.position.X + this.width && // Mover X < Liquid X + Liquid Width
                (moverPos.Y + mover.radius) > this.position.Y && // Mover Y > Liquid Y
                (moverPos.Y + mover.radius) < this.position.Y + this.height; // Mover Y < Liquid Y + Liquid Height
        }

        public Vector2 Drag(Mover mover) {
            // Drag magnitude = coefficient * speed * speed
            float magnitude = (float)Math.Sqrt(Math.Pow(mover.velocity.X, 2) + Math.Pow(mover.velocity.Y, 2));
            float speed = magnitude;
            float dragMagnitude = this.dragCoefficient * (float)Math.Pow(speed, 2);

            // Direction is inverse of velocity
            Vector2 dragForce = mover.velocity;
            dragForce *= -1;

            // Scale according to magnitude
            dragForce.Normalize();
            dragForce *= dragMagnitude;

            return dragForce;
        }

        public void Draw(SpriteBatch batch) {
            batch.Begin();

            batch.FillRectangle(this.position.X, this.position.Y, this.width, this.height, new Color(116, 185, 255, 1));
            //batch.DrawRectangle(this.position, new Size2(this.width, this.height), Color.LightBlue);

            batch.End();
        }
    }
}
