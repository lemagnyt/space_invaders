using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders
{
    /// <summary>
    ///  This class represents the enemy space ship
    /// </summary>
    internal class Enemy : SpaceShip
    {
        public bool isExplosing;
        private Bitmap Spritesheet;
        private int NbSprite;
        public int Score;
        private double Timer;

        /// <summary>
        /// Creates an enemy space ship
        /// </summary>
        /// <param name="p">the position of the enemy ship</param>
        /// <param name="lives">its number of Lives</param>
        /// <param name="image">its image</param>
        /// <param name="score">the score given when you kill it</param>
        public Enemy(Vecteur2D p, int lives, Bitmap image, int score) : base(p, lives, GetSprite(0, image, 2), Side.Enemy)
        {
            this.Score = score;
            this.Timer = 0;
            this.NbSprite = 0;
            this.Spritesheet = image;
            this.isExplosing = false;
        }
        /// <summary>
        ///  Update the Enemy ship during the game. When the enemy ship dies, there is an explosion animation.
        ///  Also all sprite from the spritesheet are updated here.
        /// </summary>
        /// <param name="gameInstance">the game where the enemy is update</param>
        /// <param name="deltaT">the time between each update</param>
        public override void Update(Game gameInstance, double deltaT)
        {
            Timer += deltaT;
            if (this.Lives == 0)
            {
                if (!this.isExplosing)
                {
                    soundplayer.URL = string.Format(@"{0}Resources\explosion.wav", Path.GetFullPath(Path.Combine(RunningPath, @"../../")));
                    this.isExplosing = true;
                    this.Spritesheet = Game.GetImageByName("explosion");
                    this.position.x = this.position.x + this.Image.Width / 2 - 35;
                    this.Image = GetSprite(0, Spritesheet, 6);
                    this.NbSprite = 0;
                    this.Timer = 0;
                }
                else
                {
                    if (this.Timer >= 0.1)
                    {
                        this.NbSprite += 1;
                        if (this.NbSprite == 6)
                        {
                            this.isExplosing = false;
                            return;
                        }
                        this.Image = GetSprite(this.NbSprite, Spritesheet, 6);
                        this.Timer = 0;
                    }
                }
            }
            else
            {
                if (Timer >= 0.75)
                {
                    if (this.NbSprite == 1)
                    {
                        this.NbSprite = 0;
                    }
                    else
                    {
                        this.NbSprite = 1;
                    }
                    this.Image = GetSprite(this.NbSprite, this.Spritesheet, 2);
                    this.Timer = 0;
                }
            }
        }

        /// <summary>
        /// Tells if the enemy is alive or no. We let it alive until the 
        /// 
        /// 
        /// animation is not finished.
        /// </summary>
        /// <returns>true if its alive, in contrary false</returns>
        public override bool IsAlive()
        {
            return !(this.Lives == 0 && !this.isExplosing);
        }

        /// <summary>
        /// give one of the spritesheet's sprite. It is use for the enemy Ship and for its explosion
        /// </summary>
        /// <param name="i">the index of the sprite in the spritesheet</param>
        /// <param name="image">thec spritesheet with all the sprites</param>
        /// <param name="totalSprite">the number of sprites in the spritesheet</param>
        /// <returns>return the wanted sprite</returns>
        public static Bitmap GetSprite(int i, Bitmap image, int totalSprite)
        {
            Rectangle cloneRect = new Rectangle(i * image.Width / totalSprite, 0, image.Width / totalSprite, image.Height);
            System.Drawing.Imaging.PixelFormat format = image.PixelFormat;
            Bitmap cloneBitmap = image.Clone(cloneRect, format);
            return cloneBitmap;
        }
    }
}
