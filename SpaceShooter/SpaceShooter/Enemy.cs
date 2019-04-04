using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceShooter
{
    abstract class Enemy : PhysicalObject
    {
        public Enemy(Texture2D texture, float X, float Y, float speedX, float speedY)
            : base(texture, X, Y, speedX, speedX)
        {
        }
        public abstract void Update(GameWindow window);
    }
}
