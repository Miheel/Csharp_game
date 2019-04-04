using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace SpaceShooter
{
    class Menu
    {
        List<MenuItem> menu;
        int selected = 0;
        
        float currentHeight = 0;
        double lastChange = 0;
        int defaultMenuState;

        public Menu(int defaultMenuState)
        {
            menu = new List<MenuItem>();
            this.defaultMenuState = defaultMenuState;
        }
        public void AddItem(Texture2D itemTexture, int state)
        {
            float X = 0;
            float Y = 0 + currentHeight;

            currentHeight += itemTexture.Height + 20;
            MenuItem temp = new MenuItem(itemTexture, new Vector2(X, Y), state);
            menu.Add(temp);
        }
        public int Update(GameTime gameTime, SoundEffect menuUSelect, SoundEffect menuDSelect, Song hsSound, Song bgSound)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            if (lastChange + 130 < gameTime.TotalGameTime.TotalMilliseconds)
            {
                if (keyboardState.IsKeyDown(Keys.Down))
                {
                    menuDSelect.Play();
                    selected++;
                    if (selected > menu.Count - 1)
                        selected = 0;
                    
                }
                if (keyboardState.IsKeyDown(Keys.Up))
                {
                    menuUSelect.Play();
                    selected--;
                    if (selected < 0)
                        selected = menu.Count - 1;
                    
                }
                lastChange = gameTime.TotalGameTime.TotalMilliseconds;
            }
            if (keyboardState.IsKeyDown(Keys.Enter))
            {
                if (selected < menu.Count)
                {
                    MediaPlayer.Play(bgSound);
                    MediaPlayer.IsRepeating = true;
                }
                if (selected > 0)
                { 
                    MediaPlayer.Play(hsSound);
                    MediaPlayer.IsRepeating = true;
                }
                return menu[selected].State;
            }
            return defaultMenuState;
        }
                              
        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < menu.Count; i++)
            {
                if (i == selected)
                    spriteBatch.Draw(menu[i].Texture, menu[i].Position, Color.RosyBrown);
                else
                    spriteBatch.Draw(menu[i].Texture, menu[i].Position, Color.White);
            }
        }
    }
}
