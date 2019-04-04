using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceShooter
{
    class PrintText
    {
        SpriteFont font;

        public PrintText(SpriteFont font)
        {
            this.font = font;
        }

        public void print(string text, SpriteBatch spriteBatch, int X, int Y)
        {
            spriteBatch.DrawString(font, text, new Vector2(X, Y), Color.White);
        }
    }
}
