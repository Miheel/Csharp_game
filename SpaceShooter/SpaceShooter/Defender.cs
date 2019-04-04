using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceShooter
{
    class Defender : Enemy
    {
        public Defender(Texture2D texture, float X, float Y)
            : base(texture, X, Y, 2f, 4f)
        {
        }

        public override void Update(GameWindow window)
        {
            vector.Y += speed.Y;
            if (vector.Y >= window.ClientBounds.Height - texture.Height)
                speed.Y -= +1;

            if (vector.Y <= 0)
                speed.Y += +1;
            vector.X -= speed.X;
            if (vector.X < 0)
                isAlive = false;
        }
    }
}
    