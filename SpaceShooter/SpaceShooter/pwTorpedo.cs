using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceShooter
{
    class pwTorpedo : PhysicalObject
    {
        double timeToDie;
        public pwTorpedo(Texture2D texture, float X, float Y, GameTime gameTime)
            : base(texture, X, Y, 0, 2f)
        {
            timeToDie = gameTime.TotalGameTime.TotalMilliseconds + 5000;
        }
        public void Update(GameTime gameTime)
        {
            if (timeToDie < gameTime.TotalGameTime.TotalMilliseconds)
                isAlive = false;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, vector, Color.White);
        }
    }
}