using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceShooter
{
    class ProtonTorpedo : Projectile
    {
        public ProtonTorpedo(Texture2D texture, float X, float Y)
            : base(texture, X, Y, 15f, 0)
        {
        }
        public override void Update(GameWindow window)
        {
            vector.X += speed.X;
            if (vector.X < 0)
                isAlive = false;
        }
        public static Vector2 Vector2FromAngle(double angle, bool normalize = true)
        {
            Vector2 vector = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
            if (vector != Vector2.Zero && normalize)
                vector.Normalize();
            return vector;
        }
    }
}