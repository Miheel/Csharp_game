using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace SpaceShooter
{
    static class GameElements
    {
        static HighScore highScore;
        static HSItem hsItem;

        static Player player;
        #region Listor
        //skapr listor 
        static List<Enemy> enemies;
        static List<Enemy> bossD1;
        static List<Enemy> bossD2;
        static List<GoldCoin> goldCoins;
        static List<GoldCoin> goldShield;
        #endregion
        static Texture2D goldCoinSprite;
        static Texture2D shieldSprite;
        static List<pwTorpedo> pwTorpedo;
        static Texture2D Torpedo;

        static PrintText printText;
        static Menu menu;
        static Background background;

        public static state currentState;
        public enum state { Menu, Run, Quit, PrintHighScore, EnterHighScore };// Bestämmer vilka gameStates som ska finas
        static SoundEffect Lazer, Proton, menuUSelect, menuDSelect;
        static public Song bgSound;
        static public Song hsSound;
        static public Song menuSound;

        static double timeSinceLastWave = 0;
        static int wave = 1, lv = 1, HPD1 = 500, HPD2 = 1000, j;
        static int points = 0;
        static string name = null;
        public static void Initialize()
        {
            goldShield = new List<GoldCoin>();
            goldCoins = new List<GoldCoin>();
            pwTorpedo = new List<pwTorpedo>();
            hsItem = new HSItem(name, points);
            highScore = new HighScore(10);
        }
        public static void LoadContent(ContentManager content, GameWindow window)
        {
            #region Sound
            //Lägger till ljud 
            hsSound = content.Load<Song>("Sound/hs");
            bgSound = content.Load<Song>("Sound/Amnesia");
            menuSound = content.Load<Song>("Sound/Something's Wrong");
            Lazer = content.Load<SoundEffect>("Sound/Lazer");
            Proton = content.Load<SoundEffect>("Sound/Proton");
            menuDSelect = content.Load<SoundEffect>("Sound/Menu-Down");
            menuUSelect = content.Load<SoundEffect>("Sound/Menu-Up");
            MediaPlayer.Play(menuSound);
            MediaPlayer.IsRepeating = true;
            #endregion
            #region Sprites
            //lägger till bilder 
            player = new Player(content.Load<Texture2D>("images/player/ship"), 50, 200, 4f, 4f, content.Load<Texture2D>("images/player/bullet"), content.Load<Texture2D>("images/player/Torpedo"));
            goldCoinSprite = content.Load<Texture2D>("images/powerups/coin");
            shieldSprite = content.Load<Texture2D>("images/powerups/GoldShield");
            Torpedo = content.Load<Texture2D>("images/powerups/pwTorpedo");
            printText = new PrintText(content.Load<SpriteFont>("myFont"));
            background = new Background(content.Load<Texture2D>("images/PrimaryBackground"), window);
            #endregion
            #region Menu
            //betämmer och lägger till vad som ska finnas på start menyn
            menu = new Menu((int)state.Menu);
            menu.AddItem(content.Load<Texture2D>("images/menu/start"), (int)state.Run);
            menu.AddItem(content.Load<Texture2D>("images/menu/highscore"), (int)state.PrintHighScore);
            menu.AddItem(content.Load<Texture2D>("images/menu/exit"), (int)state.Quit);
            #endregion
            enemies = new List<Enemy>();
            bossD1 = new List<Enemy>();
            bossD2 = new List<Enemy>();
        }

        public static void UnLoadContent(ContentManager content, GameWindow window)
        {
        }
        public static state MenuUpdate(GameTime gameTime)
        {
            return (state)menu.Update(gameTime, menuUSelect, menuDSelect, hsSound, bgSound);
        }

        public static void MenuDraw(SpriteBatch spriteBatch)
        {
            background.Draw(spriteBatch);
            menu.Draw(spriteBatch);
        }
        public static state RunUpdate(ContentManager content, GameWindow window, GameTime gameTime)
        {
            player.update(window, gameTime, Lazer, Proton);
            background.Update(window);
            #region Enemy Wave
            Random random = new Random();
            int w = 0;
            // besämmer hur länga det är mellan varige Wave
            if (gameTime.TotalGameTime.TotalSeconds > timeSinceLastWave + 9)
            {
                if (wave < 4)//ökar wave om wave < 4
                {
                    while (w < 1)
                    { wave++; w++; }
                }
                if (wave > 4 && wave < 9)
                {
                    while (w < 1)
                    { wave++; w++; }
                }
                timeSinceLastWave = gameTime.TotalGameTime.TotalSeconds;
            }

            #region Level 1
            if (lv == 1)
            {
                if (wave == 1)
                {

                    while (j < 1)//ser till att det bara körs en gång och inte lika många gånger som spelet updateras
                    {
                        // anger vilker bild som tillhör denna fiende
                        Texture2D tmpSprite = content.Load<Texture2D>("images/enemies/TIE-Defender");
                        for (int i = 0; i < 10; i++)
                        {
                            //sätter fiender på random platser på x 600
                            int rndX = random.Next(600, window.ClientBounds.Width);
                            int rndY = random.Next(0, window.ClientBounds.Height - tmpSprite.Height);
                            Defender temp = new Defender(tmpSprite, rndX, rndY);
                            enemies.Add(temp);
                        }
                        tmpSprite = content.Load<Texture2D>("images/enemies/TIE-fighter");
                        for (int i = 0; i < 10; i++)
                        {
                            int rndX = random.Next(600, window.ClientBounds.Width);
                            int rndY = random.Next(0, window.ClientBounds.Height - tmpSprite.Height);
                            Fighter temp = new Fighter(tmpSprite, rndX, rndY);
                            enemies.Add(temp);
                        }
                        j = 1;
                    }
                }
                if (wave == 2)
                {

                    while (j < 2)
                    {
                        Texture2D tmpSprite = content.Load<Texture2D>("images/enemies/TIE-Defender");
                        for (int i = 0; i < 20; i++)
                        {
                            int rndX = random.Next(600, window.ClientBounds.Width);
                            int rndY = random.Next(0, window.ClientBounds.Height - tmpSprite.Height);
                            Defender temp = new Defender(tmpSprite, rndX, rndY);
                            enemies.Add(temp);
                        }
                        tmpSprite = content.Load<Texture2D>("images/enemies/TIE-fighter");
                        for (int i = 0; i < 20; i++)
                        {
                            int rndX = random.Next(600, window.ClientBounds.Width);
                            int rndY = random.Next(0, window.ClientBounds.Height - tmpSprite.Height);
                            Fighter temp = new Fighter(tmpSprite, rndX, rndY);
                            enemies.Add(temp);
                        }
                        j = 2;
                    }
                }
                if (wave == 3)
                {
                    while (j < 3)
                    {
                        Texture2D tmpSprite = content.Load<Texture2D>("images/enemies/TIE-Defender");
                        for (int i = 0; i < 30; i++)
                        {
                            int rndX = random.Next(600, window.ClientBounds.Width);
                            int rndY = random.Next(0, window.ClientBounds.Height - tmpSprite.Height);
                            Defender temp = new Defender(tmpSprite, rndX, rndY);
                            enemies.Add(temp);
                        }
                        tmpSprite = content.Load<Texture2D>("images/enemies/TIE-fighter");
                        for (int i = 0; i < 30; i++)
                        {
                            int rndX = random.Next(600, window.ClientBounds.Width);
                            int rndY = random.Next(0, window.ClientBounds.Height - tmpSprite.Height);
                            Fighter temp = new Fighter(tmpSprite, rndX, rndY);
                            enemies.Add(temp);
                        }
                        j = 3;
                    }
                }
                if (wave == 4)
                {
                    while (j < 4)
                    {
                        Texture2D tmpSprite = content.Load<Texture2D>("images/enemies/Boss/The_Death_Star");
                        for (int i = 0; i < 1; i++)
                        {
                            int rndX = random.Next(650, window.ClientBounds.Width - tmpSprite.Width);
                            int rndY = random.Next(0, window.ClientBounds.Height - tmpSprite.Height);
                            Boss temp = new Boss(tmpSprite, rndX, rndY);
                            bossD1.Add(temp);
                        }
                        j = 4;
                    }
                }
            }
            #endregion
            #region Level 2
            if (lv == 2)
            {
                if (wave == 6)
                {
                    while (j < 5)
                    {
                        Texture2D tmpSprite = content.Load<Texture2D>("images/enemies/TIE-Defender");
                        for (int i = 0; i < 10; i++)
                        {
                            int rndX = random.Next(600, window.ClientBounds.Width);
                            int rndY = random.Next(0, window.ClientBounds.Height - tmpSprite.Height);
                            Defender temp = new Defender(tmpSprite, rndX, rndY);
                            enemies.Add(temp);
                        }
                        tmpSprite = content.Load<Texture2D>("images/enemies/TIE-Advanced");
                        for (int i = 0; i < 10; i++)
                        {
                            int rndX = random.Next(600, window.ClientBounds.Width);
                            int rndY = random.Next(0, window.ClientBounds.Height - tmpSprite.Height);
                            Advanced temp = new Advanced(tmpSprite, rndX, rndY);
                            enemies.Add(temp);
                        }
                        j = 5;
                    }
                }
                if (wave == 7)
                {
                    while (j < 6)
                    {
                        Texture2D tmpSprite = content.Load<Texture2D>("images/enemies/TIE-Defender");
                        for (int i = 0; i < 20; i++)
                        {
                            int rndX = random.Next(600, window.ClientBounds.Width);
                            int rndY = random.Next(0, window.ClientBounds.Height - tmpSprite.Height);
                            Defender temp = new Defender(tmpSprite, rndX, rndY);
                            enemies.Add(temp);
                        }
                        tmpSprite = content.Load<Texture2D>("images/enemies/TIE-Advanced");
                        for (int i = 0; i < 20; i++)
                        {
                            int rndX = random.Next(600, window.ClientBounds.Width);
                            int rndY = random.Next(0, window.ClientBounds.Height - tmpSprite.Height);
                            Advanced temp = new Advanced(tmpSprite, rndX, rndY);
                            enemies.Add(temp);
                        }
                        j = 6;
                    }
                }
                if (wave == 8)
                {
                    while (j < 7)
                    {
                        Texture2D tmpSprite = content.Load<Texture2D>("images/enemies/TIE-Defender");
                        for (int i = 0; i < 30; i++)
                        {
                            int rndX = random.Next(600, window.ClientBounds.Width);
                            int rndY = random.Next(0, window.ClientBounds.Height - tmpSprite.Height);
                            Defender temp = new Defender(tmpSprite, rndX, rndY);
                            enemies.Add(temp);
                        }
                        tmpSprite = content.Load<Texture2D>("images/enemies/TIE-Advanced");
                        for (int i = 0; i < 30; i++)
                        {
                            int rndX = random.Next(600, window.ClientBounds.Width);
                            int rndY = random.Next(0, window.ClientBounds.Height - tmpSprite.Height);
                            Advanced temp = new Advanced(tmpSprite, rndX, rndY);
                            enemies.Add(temp);
                        }
                        j = 7;
                    }
                }
                if (wave == 9)
                {
                    while (j < 8)
                    {
                        Texture2D tmpSprite = content.Load<Texture2D>("images/enemies/Boss/Death-Star-2");
                        for (int i = 0; i < 1; i++)
                        {
                            int rndX = random.Next(650, window.ClientBounds.Width - tmpSprite.Width);
                            int rndY = random.Next(0, window.ClientBounds.Height - tmpSprite.Height);
                            Boss temp = new Boss(tmpSprite, rndX, rndY);
                            bossD2.Add(temp);
                        }
                        j = 8;
                    }
                }
            }
            #endregion
            #endregion
            #region Collision
            // anger vadsom ska hända när objekt kolliderar med varandra
            #region Collision Enemy
            
            foreach (Enemy e in enemies.ToList())
            {
                foreach (Bullet b in player.Bullets)
                {
                    // kollision mellan fiende och skott
                    if (e.CheckCollision(b))
                    {
                        // vid kollision dödar finede ohc skott
                        e.IsAlive = false;
                        b.IsAlive = false;
                        // vid kollision ger dig 10 poeng
                        player.points += 10;
                    }
                }
                foreach (ProtonTorpedo pt in player.Torpedo)
                {

                    if (e.CheckCollision(pt))
                    {
                        // vid kollision dödar finede
                        e.IsAlive = false;
                        // vid kollision ger dig 10 poeng
                        player.points += 10;
                    }
                }
                if (e.IsAlive)
                {
                    // om player har sköld 
                    if (player.shields > 0)
                    {
                        //kollision mellen fiende och player
                        if (e.CheckCollision(player))
                        {
                            // tar bort sköld
                            player.shields -= 1;
                            //om sköld fins kvar så dör inte player
                            player.IsAlive = true;
                            if (player.points > 0)
                            {
                                // tar bort poeng om kollision inträffar
                                player.points -= 1;
                            }
                        }
                        e.Update(window);
                    }
                    else
                        // döda player om sköld är 0
                        player.IsAlive = false;
                }
                else
                    enemies.Remove(e);
            }
            #endregion
            #region Collision BossD1
            foreach (Enemy boss in bossD1.ToList())
            {
                foreach (Bullet b in player.Bullets)
                {
                    if (boss.IsAlive)
                    {
                        if (HPD1 > 0)
                        {
                            if (boss.CheckCollision(b))
                            {
                                b.IsAlive = false;
                                player.points += 10;
                                HPD1--;
                            }
                        }
                        else
                            boss.IsAlive = false;

                    }
                    foreach (ProtonTorpedo pt in player.Torpedo)
                    {
                        if (boss.IsAlive)
                        {
                            if (HPD1 > 0)
                            {
                                if (boss.CheckCollision(pt))
                                {
                                    pt.IsAlive = false;
                                    player.points += 50;
                                    HPD1--;
                                }
                            }
                            else
                                boss.IsAlive = false;
                        }
                    }
                }
                if (boss.IsAlive)
                {
                    if (boss.CheckCollision(player))
                    {
                        player.IsAlive = false;
                    }
                    boss.Update(window);
                }
                else
                {
                    bossD1.Remove(boss);
                    lv++;
                    wave++;
                }
            }
            #endregion
            #region Collision BossD2
            foreach (Enemy boss in bossD2.ToList())
            {
                foreach (Bullet b in player.Bullets)
                {
                    if (boss.IsAlive)
                    {
                        if (HPD2 > 0)
                        {
                            if (boss.CheckCollision(b))
                            {
                                b.IsAlive = false;
                                player.points += 10;
                                HPD2--;
                            }
                        }
                        else
                            boss.IsAlive = false;

                    }
                    foreach (ProtonTorpedo pt in player.Torpedo)
                    {
                        if (boss.IsAlive)
                        {
                            if (HPD2 > 0)
                            {
                                if (boss.CheckCollision(pt))
                                {
                                    pt.IsAlive = false;
                                    player.points += 50;
                                    HPD2--;
                                }
                            }
                            else
                                boss.IsAlive = false;
                        }
                    }
                }
                if (boss.IsAlive)
                {
                    if (boss.CheckCollision(player))
                    {
                        player.IsAlive = false;
                    }
                    boss.Update(window);
                }
                else
                {
                    bossD2.Remove(boss);
                    return state.EnterHighScore;
                }
            }
            #endregion
            #region Coin Collision
            int newCoin = random.Next(1, 200);
            if (newCoin == 1)
            {
                int rndX = random.Next(0, window.ClientBounds.Width - goldCoinSprite.Width);
                int rndY = random.Next(0, window.ClientBounds.Height - goldCoinSprite.Height);

                goldCoins.Add(new GoldCoin(goldCoinSprite, rndX, rndY, gameTime));
            }
            foreach (GoldCoin gc in goldCoins.ToList())
            {
                if (gc.IsAlive)
                {
                    gc.Update(gameTime);
                    if (gc.CheckCollision(player))
                    {
                        goldCoins.Remove(gc);
                        player.points += 10;
                    }
                }
                else
                    goldCoins.Remove(gc);
            }
            #endregion
            #region Shield Collision
            int newshield = random.Next(1, 200);
            if (wave < 4)
            {
                if (newshield == 1)
                {
                    int rndX = random.Next(0, window.ClientBounds.Width - shieldSprite.Width);
                    int rndY = random.Next(0, window.ClientBounds.Height - shieldSprite.Height);

                    goldShield.Add(new GoldCoin(shieldSprite, rndX, rndY, gameTime));
                }
            }

            foreach (GoldCoin gs in goldShield.ToList())
            {
                if (gs.IsAlive)
                {
                    gs.Update(gameTime);
                    if (gs.CheckCollision(player))
                    {
                        goldShield.Remove(gs);
                        player.shields += 50;
                    }
                }
                else
                    goldShield.Remove(gs);
            }
            #endregion
            #region Torpedo Collision
            int newTorpedo = random.Next(1, 200);
            if (newTorpedo == 1)
            {
                int rndX = random.Next(0, window.ClientBounds.Width - Torpedo.Width);
                int rndY = random.Next(0, window.ClientBounds.Height - Torpedo.Height);

                pwTorpedo.Add(new pwTorpedo(Torpedo, rndX, rndY, gameTime));
            }
            foreach (pwTorpedo pw in pwTorpedo.ToList())
            {
                if (pw.IsAlive)
                {
                    pw.Update(gameTime);
                    if (pw.CheckCollision(player))
                    {
                        pwTorpedo.Remove(pw);
                        player.PW++;
                    }
                }
                else
                    pwTorpedo.Remove(pw);
            }
            #endregion
            #endregion
            #region Died
            // anger vad som ska hända när spelaren dör
            if (!player.IsAlive)
            {
                // stoppa och starta en ny song 
                MediaPlayer.Stop();
                MediaPlayer.Play(hsSound);
                MediaPlayer.IsRepeating = true;
                hsItem.Points = player.points;
                //anroppar Reset
                Reset(window, content, gameTime);
                // gör så att du kan ange ditt namn och till highscorelistan
                return state.EnterHighScore;
            }
            #endregion
            return state.Run;
        }
        public static void RunDraw(SpriteBatch spriteBatch)
        {
            
            #region Draw sprite
            // ritar ut de olika fiendera, pwoerups och player
            background.Draw(spriteBatch);
            player.Draw(spriteBatch);
            foreach (Enemy e in enemies)
                e.Draw(spriteBatch);
            foreach (Enemy e2 in bossD1)
                e2.Draw(spriteBatch);
            foreach (Enemy e2 in bossD2)
                e2.Draw(spriteBatch);
            foreach (GoldCoin gc in goldCoins)
                gc.Draw(spriteBatch);
            foreach (GoldCoin sh in goldShield)
                sh.Draw(spriteBatch);
            foreach (pwTorpedo pw in pwTorpedo)
                pw.Draw(spriteBatch);
            #endregion
            #region Text
            // Ritar ut text på skärmen 
            // vasar information om poeng, din sköld, hur många torpeder du har
            //vilken level och vilken fiende våg  du är på 
            printText.print("points:" + player.points, spriteBatch, 0, 0);
            printText.print("points:" + hsItem.Points, spriteBatch, 100, 0);
            printText.print("Shield:" + player.shields, spriteBatch, 0, 20);
            printText.print("Level:" + GameElements.lv, spriteBatch, 0, 40);
            printText.print("Wave:" + GameElements.wave, spriteBatch, 0, 60);
            printText.print("Torpedo:" + player.PW, spriteBatch, 0, 80);
            // visar Bossens HP om fiendevåg är 4 och on level inte är 2
            if (wave == 4 && lv != 2)
            {
                printText.print("Boss HP:" + GameElements.HPD1, spriteBatch, 380, 0);
            }
            // visar Bossens HP om fiendevåg är 9
            if (wave == 9)
            {
                printText.print("Boss HP:" + GameElements.HPD2, spriteBatch, 380, 0);
            }

            #endregion
        }
        public static state EnterUpdate(GameTime gameTime, int points)
        {
            highScore.EnterUpdate(gameTime, points);
            KeyboardState keyBoardState = Keyboard.GetState();
            if (keyBoardState.IsKeyDown(Keys.Escape))
            {
                // går tillbaka till starmenyn och startar en my song
                MediaPlayer.Stop();
                MediaPlayer.Play(menuSound);
                return state.Menu;
            }
            return state.EnterHighScore;
        }
        public static void EnterDraw(SpriteBatch spriteBatch, SpriteFont myFont)
        {
            // ritar ut vad som ska finnas när du skriver in ditt namn
            background.Draw(spriteBatch);
            highScore.EnterDraw(spriteBatch, myFont);
        }
        public static void PrintDraw(SpriteBatch spriteBatch, SpriteFont myFont)
        {
            // ritar ut vad som ska finnas på highscorelistan
            background.Draw(spriteBatch);
            highScore.PrintDraw(spriteBatch, myFont);
        }
        public static state printUpdate()
        {
            // går tillbaka till starmenyn och startar en my song
            KeyboardState keyBoardState = Keyboard.GetState();
            if (keyBoardState.IsKeyDown(Keys.Escape))
            {
                MediaPlayer.Stop();
                MediaPlayer.Play(menuSound);
                return state.Menu;
            }
            return state.PrintHighScore;

        }
        public static void Reset(GameWindow window, ContentManager content, GameTime gameTime)
        {
            //sätter allt till orginal värden när player dör
            player.Reset(50, 200, 4f, 4f);
            enemies.Clear();
            bossD1.Clear();
            bossD2.Clear();
            wave = 1;
            lv = 1;
            player.points = 0;
            player.PW = 0;
            j = 0;
            int w = 0;
            timeSinceLastWave = 0;
            #region Enemy Wave
            Random random = new Random();

            if (gameTime.TotalGameTime.TotalSeconds > timeSinceLastWave + 9)
            {
                if (wave < 4)
                {
                    while (w < 1)
                    { wave++; w++; }
                }
                if (wave > 4 && wave < 9)
                {
                    while (w < 1)
                    { wave++; w++; }
                }
                timeSinceLastWave = gameTime.TotalGameTime.TotalSeconds;
            }

            #region Level 1
            if (lv == 1)
            {
                if (wave == 1)
                {

                    while (j < 1)
                    {
                        Texture2D tmpSprite = content.Load<Texture2D>("images/enemies/TIE-Defender");
                        for (int i = 0; i < 10; i++)
                        {
                            int rndX = random.Next(600, window.ClientBounds.Width);
                            int rndY = random.Next(0, window.ClientBounds.Height - tmpSprite.Height);
                            Defender temp = new Defender(tmpSprite, rndX, rndY);
                            enemies.Add(temp);
                        }
                        tmpSprite = content.Load<Texture2D>("images/enemies/TIE-fighter");
                        for (int i = 0; i < 10; i++)
                        {
                            int rndX = random.Next(600, window.ClientBounds.Width);
                            int rndY = random.Next(0, window.ClientBounds.Height - tmpSprite.Height);
                            Fighter temp = new Fighter(tmpSprite, rndX, rndY);
                            enemies.Add(temp);
                        }
                        j = 1;
                    }
                }
                if (wave == 2)
                {

                    while (j < 2)
                    {
                        Texture2D tmpSprite = content.Load<Texture2D>("images/enemies/TIE-Defender");
                        for (int i = 0; i < 20; i++)
                        {
                            int rndX = random.Next(600, window.ClientBounds.Width);
                            int rndY = random.Next(0, window.ClientBounds.Height - tmpSprite.Height);
                            Defender temp = new Defender(tmpSprite, rndX, rndY);
                            enemies.Add(temp);
                        }
                        tmpSprite = content.Load<Texture2D>("images/enemies/TIE-fighter");
                        for (int i = 0; i < 20; i++)
                        {
                            int rndX = random.Next(600, window.ClientBounds.Width);
                            int rndY = random.Next(0, window.ClientBounds.Height - tmpSprite.Height);
                            Fighter temp = new Fighter(tmpSprite, rndX, rndY);
                            enemies.Add(temp);
                        }
                        j = 2;
                    }
                }
                if (wave == 3)
                {
                    while (j < 3)
                    {
                        Texture2D tmpSprite = content.Load<Texture2D>("images/enemies/TIE-Defender");
                        for (int i = 0; i < 30; i++)
                        {
                            int rndX = random.Next(600, window.ClientBounds.Width);
                            int rndY = random.Next(0, window.ClientBounds.Height - tmpSprite.Height);
                            Defender temp = new Defender(tmpSprite, rndX, rndY);
                            enemies.Add(temp);
                        }
                        tmpSprite = content.Load<Texture2D>("images/enemies/TIE-fighter");
                        for (int i = 0; i < 30; i++)
                        {
                            int rndX = random.Next(600, window.ClientBounds.Width);
                            int rndY = random.Next(0, window.ClientBounds.Height - tmpSprite.Height);
                            Fighter temp = new Fighter(tmpSprite, rndX, rndY);
                            enemies.Add(temp);
                        }
                        j = 3;
                    }
                }
                if (wave == 4)
                {
                    while (j < 4)
                    {
                        Texture2D tmpSprite = content.Load<Texture2D>("images/enemies/Boss/The_Death_Star");
                        for (int i = 0; i < 1; i++)
                        {
                            int rndX = random.Next(650, window.ClientBounds.Width - tmpSprite.Width);
                            int rndY = random.Next(0, window.ClientBounds.Height - tmpSprite.Height);
                            Boss temp = new Boss(tmpSprite, rndX, rndY);
                            bossD1.Add(temp);
                        }
                        j = 4;
                    }
                }
            }
            #endregion
            #region Level 2
            if (lv == 2)
            {
                if (wave == 6)
                {
                    while (j < 5)
                    {
                        Texture2D tmpSprite = content.Load<Texture2D>("images/enemies/TIE-Defender");
                        for (int i = 0; i < 10; i++)
                        {
                            int rndX = random.Next(600, window.ClientBounds.Width);
                            int rndY = random.Next(0, window.ClientBounds.Height - tmpSprite.Height);
                            Defender temp = new Defender(tmpSprite, rndX, rndY);
                            enemies.Add(temp);
                        }
                        tmpSprite = content.Load<Texture2D>("images/enemies/TIE-Advanced");
                        for (int i = 0; i < 10; i++)
                        {
                            int rndX = random.Next(600, window.ClientBounds.Width);
                            int rndY = random.Next(0, window.ClientBounds.Height - tmpSprite.Height);
                            Advanced temp = new Advanced(tmpSprite, rndX, rndY);
                            enemies.Add(temp);
                        }
                        j = 5;
                    }
                }
                if (wave == 7)
                {
                    while (j < 6)
                    {
                        Texture2D tmpSprite = content.Load<Texture2D>("images/enemies/TIE-Defender");
                        for (int i = 0; i < 20; i++)
                        {
                            int rndX = random.Next(600, window.ClientBounds.Width);
                            int rndY = random.Next(0, window.ClientBounds.Height - tmpSprite.Height);
                            Defender temp = new Defender(tmpSprite, rndX, rndY);
                            enemies.Add(temp);
                        }
                        tmpSprite = content.Load<Texture2D>("images/enemies/TIE-Advanced");
                        for (int i = 0; i < 20; i++)
                        {
                            int rndX = random.Next(600, window.ClientBounds.Width);
                            int rndY = random.Next(0, window.ClientBounds.Height - tmpSprite.Height);
                            Advanced temp = new Advanced(tmpSprite, rndX, rndY);
                            enemies.Add(temp);
                        }
                        j = 6;
                    }
                }
                if (wave == 8)
                {
                    while (j < 7)
                    {
                        Texture2D tmpSprite = content.Load<Texture2D>("images/enemies/TIE-Defender");
                        for (int i = 0; i < 30; i++)
                        {
                            int rndX = random.Next(600, window.ClientBounds.Width);
                            int rndY = random.Next(0, window.ClientBounds.Height - tmpSprite.Height);
                            Defender temp = new Defender(tmpSprite, rndX, rndY);
                            enemies.Add(temp);
                        }
                        tmpSprite = content.Load<Texture2D>("images/enemies/TIE-Advanced");
                        for (int i = 0; i < 30; i++)
                        {
                            int rndX = random.Next(600, window.ClientBounds.Width);
                            int rndY = random.Next(0, window.ClientBounds.Height - tmpSprite.Height);
                            Advanced temp = new Advanced(tmpSprite, rndX, rndY);
                            enemies.Add(temp);
                        }
                        j = 7;
                    }
                }
                if (wave == 9)
                {
                    while (j < 8)
                    {
                        Texture2D tmpSprite = content.Load<Texture2D>("images/enemies/Boss/Death-Star-2");
                        for (int i = 0; i < 1; i++)
                        {
                            int rndX = random.Next(650, window.ClientBounds.Width - tmpSprite.Width);
                            int rndY = random.Next(0, window.ClientBounds.Height - tmpSprite.Height);
                            Boss temp = new Boss(tmpSprite, rndX, rndY);
                            bossD2.Add(temp);
                        }
                        j = 8;
                    }
                }
            }
            #endregion
            #endregion
        }
    }
}
