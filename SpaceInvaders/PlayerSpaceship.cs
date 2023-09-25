using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib;

namespace SpaceInvaders
{
    internal class PlayerSpaceship : SpaceShip
    {
        public double Timer;
        public bool invincible;
        /// <summary>
        /// Create the player Spaceship
        /// </summary>
        /// <param name="p">position</param>
        /// <param name="lives">number of lives</param>
        /// <param name="image">image</param>
        public PlayerSpaceship(Vecteur2D p, int lives, Bitmap image) : base(p, lives, image, Side.Ally) {
            this.invincible = false;
        }
        /// <summary>
        /// Update the player space ship
        /// </summary>
        /// <param name="gameInstance">the game where he is</param>
        /// <param name="deltaT">the time between 2 updates</param>
        public override void Update(Game gameInstance, double deltaT){
            if (this.invincible){
                Timer += deltaT;
                if(Timer >= 20){
                    Timer = 0;
                    invincible = false;
                }
            }
            if (gameInstance.keyPressed.Contains(Keys.Left)){
                if (this.position.x - gameInstance.playerSpeed > 0){
                    this.position.x -= gameInstance.playerSpeed;
                }
            }
            else if (gameInstance.keyPressed.Contains(Keys.Right)){
                if ((this.position.x + (double)this.Image.Width + gameInstance.playerSpeed) < (double)gameInstance.gameSize.Width){
                    this.position.x += gameInstance.playerSpeed;
                }
            }
            if (gameInstance.keyPressed.Contains(Keys.Space)){
                this.Shoot(gameInstance);
            }
        }

        /// <summary>
        /// Do the collision action depending on the object type which is in collision whit this one and if there is a collision or no.
        /// </summary>
        /// <param name="m">object in collision</param>
        /// <param name="numberOfPixelsInCollision">number of pixel that are in collision</param>
        /// <param name="PixelList">list of the positions of these pixels</param>
        protected override void OnCollision(SimpleObject m, int numberOfPixelsInCollision, HashSet<int[]> PixelList)
        {
            if (m.GetType().Equals(typeof(Missile)))
            {
                if (!this.invincible)
                {
                    soundplayer.URL = string.Format(@"{0}Resources\collision.wav", Path.GetFullPath(Path.Combine(RunningPath, @"../../")));
                    int nbLives = Math.Min(m.Lives, this.Lives);
                    m.Lives -= nbLives;
                    this.Lives -= nbLives;
                }
                else
                {
                    soundplayer.URL = string.Format(@"{0}Resources\bouclier.wav", Path.GetFullPath(Path.Combine(RunningPath, @"../../")));
                    m.Lives = 0;
                }
            }
            else if (m.GetType().Equals(typeof(Coeur))){
                m.Lives = 0;
                if (Lives < (5 * 30)){
                    this.Lives += 30;
                }
                soundplayer.URL = string.Format(@"{0}Resources\coeur.wav", Path.GetFullPath(Path.Combine(RunningPath, @"../../")));
            }
            else if (m.GetType().Equals(typeof(Bouclier))){
                this.invincible = true;
                m.Lives = 0;
                soundplayer.URL = string.Format(@"{0}Resources\bouclier.wav", Path.GetFullPath(Path.Combine(RunningPath, @"../../")));
            }
        }
    }
}
