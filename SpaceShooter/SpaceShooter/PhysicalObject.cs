using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceShooter

{
    class PhysicalObject : MovingObject
    {
        protected bool isAlive = true;

        public PhysicalObject(Texture2D texture, float X, float Y, float speedX, float speedY)
            : base(texture, X, Y, speedX, speedY)
        {
        }

        public bool CheckCollision(PhysicalObject outher)
        {
            Rectangle myRect = new Rectangle(Convert.ToInt32(X), Convert.ToInt32(Y), Convert.ToInt32(Width), Convert.ToInt32(Height));
            Rectangle outherRect = new Rectangle(Convert.ToInt32(outher.X), Convert.ToInt32(outher.Y), Convert.ToInt32(outher.Width), Convert.ToInt32(outher.Height));
            return myRect.Intersects(outherRect);
        }

        public bool IsAlive { get { return isAlive; } set { isAlive = value; } }
    }
}
