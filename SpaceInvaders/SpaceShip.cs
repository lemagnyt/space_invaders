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
    internal class SpaceShip : SimpleObject
    {
        public WMPLib.WindowsMediaPlayer soundplayer = new WMPLib.WindowsMediaPlayer();
        public string RunningPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        public Missile missile;
        private double speedPixelPerSecond;

        /// <summary>
        /// Create a spaceship
        /// </summary>
        /// <param name="p">position</param>
        /// <param name="lives">number of lives</param>
        /// <param name="image">image</param>
        /// <param name="objectSide">side of the space ship</param>
        public SpaceShip(Vecteur2D  p, int lives, Bitmap image,Side objectSide):base(p,lives,image,objectSide){}
        public override void Update(Game gameInstance, double deltaT){}

        /// <summary>
        /// Make the spaceship shoot by creating missile and adding it to game
        /// </summary>
        /// <param name="gameInstance">the game where the missile is create</param>
        public void Shoot(Game gameInstance){
            if (this.missile==null || !(this.missile.IsAlive())){
                Bitmap imageMissile = SpaceInvaders.Properties.Resources.shoot1;
                double positionX = this.position.x +(this.Image.Width / 2) - imageMissile.Width/2;
                double positionY = this.position.y - (double)imageMissile.Height-1;
                this.missile = new Missile(new Vecteur2D(positionX, positionY), 30, imageMissile,this.ObjectSide);
                gameInstance.AddNewGameObject(this.missile);
                soundplayer.URL = string.Format(@"{0}Resources\tirSpaceship.wav", Path.GetFullPath(Path.Combine(RunningPath, @"../../")));
            }
        }

        /// <summary>
        /// When collision the function will take off the minimum life between the missile and the space ship to the two
        /// </summary>
        /// <param name="m">missile</param>
        /// <param name="numberOfPixelsInCollision">pixel in collision between missile and ship</param>
        /// <param name="PixelList">position of these pixel</param>
        protected override void OnCollision(SimpleObject m, int numberOfPixelsInCollision, HashSet<int[]> PixelList){
            if (m.GetType().Equals(typeof(Missile))){
                int nbLives = Math.Min(m.Lives, this.Lives);
                m.Lives -= nbLives;
                this.Lives -= nbLives;
            }
        }
    }
}
