using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace SpaceShooter
{
    class Player : PhysicalObject
    {
        List<Bullet> bullets;
        Texture2D bulletsTexture;
        List<ProtonTorpedo> torpedo;
        Texture2D torpedoTexture;
        double timeSinceLastBullet = 0;
        double timeSinceLastTorpedo = 0;
        public int points = 0;
        public int PW = 0;
        public int Points { get { return points; } set { points = value; } }
        public int shields = 500;
        public int Shields { get { return shields; } set { shields = value; } }
        public List<Bullet> Bullets { get { return bullets; } }
        public List<ProtonTorpedo> Torpedo { get { return torpedo; } }
        public Player(Texture2D texture, float X, float Y, float speedX, float speedY, Texture2D bulletTexture, Texture2D torpedoTexture)
            : base(texture, X, Y, speedX, speedY)
        {
            bullets = new List<Bullet>();
            this.bulletsTexture= bulletTexture;
            torpedo = new List<ProtonTorpedo>();
            this.torpedoTexture = torpedoTexture;
        }
        public void update(GameWindow window, GameTime gameTime, SoundEffect Lazer, SoundEffect Proton)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Escape))
            {
                isAlive = false;
            }

            if (vector.X <= window.ClientBounds.Width - texture.Width && vector.X >= 0)
            {
                if (keyboardState.IsKeyDown(Keys.Right))
                    vector.X += speed.X;
                if (keyboardState.IsKeyDown(Keys.Left))
                    vector.X -= speed.X;
            }
            if (vector.Y <= window.ClientBounds.Height - texture.Height && vector.Y >= 0)
            {
                if (keyboardState.IsKeyDown(Keys.Down))
                    vector.Y += speed.Y;
                if (keyboardState.IsKeyDown(Keys.Up))
                    vector.Y -= speed.Y;
            }

            if (vector.X < 0)
                vector.X = 0;
            if (vector.X > window.ClientBounds.Width - texture.Width)
            {
                vector.X = window.ClientBounds.Width - texture.Width;
            }
            if (vector.Y < 0)
                vector.Y = 0;
            if (vector.Y > window.ClientBounds.Height - texture.Height)
            {
                vector.Y = window.ClientBounds.Height - texture.Height;
            }
            if (keyboardState.IsKeyDown(Keys.Space))
            {
                
                if (gameTime.TotalGameTime.TotalMilliseconds > timeSinceLastBullet + 200)
                {
                    Lazer.Play();
                    Bullet temp1 = new Bullet(bulletsTexture, vector.X + texture.Width, vector.Y);
                    bullets.Add(temp1);
                    
                    Bullet temp2 = new Bullet(bulletsTexture, vector.X + texture.Width, vector.Y + 36);
                    bullets.Add(temp2);
                    timeSinceLastBullet = gameTime.TotalGameTime.TotalMilliseconds;
                }
            }
            if (PW >= 1)
            {
                if (keyboardState.IsKeyDown(Keys.E))
                {

                    if (gameTime.TotalGameTime.TotalMilliseconds > timeSinceLastTorpedo + 5000)
                    {
                        Proton.Play();
                        PW--;
                        ProtonTorpedo temp2 = new ProtonTorpedo(torpedoTexture, vector.X + texture.Width, vector.Y + 12);
                        torpedo.Add(temp2);
                        timeSinceLastTorpedo = gameTime.TotalGameTime.TotalMilliseconds;
                    }
                }
            }
            foreach (Bullet b in bullets.ToList())
            {
                b.Update(window);
                if (!b.IsAlive)
                    bullets.Remove(b);
            }
            foreach (ProtonTorpedo pt in torpedo.ToList())
            {
                pt.Update(window);
                if (!pt.IsAlive)
                    torpedo.Remove(pt);
            }

        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, vector, Color.White);
            foreach (Bullet b in bullets) b.Draw(spriteBatch);
            foreach (ProtonTorpedo pt in torpedo) pt.Draw(spriteBatch);
        }
        public void Reset(float X, float Y, float speedX, float speedY)
        {
            vector.X = X;
            vector.Y = Y;
            speed.X = speedX;
            speed.Y = speedY;
            bullets.Clear();
            torpedo.Clear();
            timeSinceLastBullet = 0;
            timeSinceLastTorpedo = 0;
            shields = 500;
            isAlive = true;
        }
    }
}
