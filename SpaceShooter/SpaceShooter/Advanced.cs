using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceShooter
{
    class Advanced : Enemy
    {

        public Advanced(Texture2D texture, float X, float Y)
            : base(texture, X, Y, 4f, 0f)
        {
        }

        public override void Update(GameWindow window)
        {
            vector.X -= speed.X;
            if (vector.X < 0)
                isAlive = false;
        }
    }
}