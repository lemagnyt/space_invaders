using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib;

namespace SpaceInvaders
{
    internal class Missile : SimpleObject
    {
        private WMPLib.WindowsMediaPlayer soundplayer = new WMPLib.WindowsMediaPlayer();
        private string RunningPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        /// <summary>
        /// Missile speed
        /// </summary>
        public double Vitesse { get; set; }
        /// <summary>
        /// Allows to initialize the position, the number of lives and the image of the missile.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="lives"></param>
        /// <param name="image"></param>
        /// <param name="objectSide"></param>
        public Missile(Vecteur2D p, int lives, Bitmap image, Side objectSide):base(p,lives,image, objectSide){
            this.Vitesse = 1.0;
        }

        /// <summary>
        /// The missile moves vertically according to its speed. If the missile leaves the screen, its life number goes to zero.
        /// </summary>
        /// <param name="gameInstance"></param>
        /// <param name="deltaT"></param>
        public override void Update(Game gameInstance, double deltaT){
            if (this.ObjectSide == Side.Ally){
                this.position.y -= this.Vitesse;
                if (this.position.y + this.Image.Height <= 0){
                    this.Lives = 0;
                }
            }
            else if(this.ObjectSide == Side.Enemy){
                this.position.y += this.Vitesse;
                if (this.position.y + this.Image.Height >= gameInstance.gameSize.Height){
                    this.Lives = 0;
                }
            }
            foreach (GameObject gameobject in gameInstance.gameObjects){
                gameobject.Collision(this);
            }
        }

        /// <summary>
        /// When there is a collision  between 2 missiles, the two are destroyed
        /// </summary>
        /// <param name="m">other missile</param>
        /// <param name="numberOfPixelsInCollision">pixel in collision between the 2</param>
        /// <param name="PixelList">position of pixels</param>
        protected override void OnCollision(SimpleObject m, int numberOfPixelsInCollision, HashSet<int[]> PixelList){
            if (m.GetType() == typeof(Missile))
            {
                this.Lives = 0;
                m.Lives = 0;
                soundplayer.URL = string.Format(@"{0}Resources\collision.wav", Path.GetFullPath(Path.Combine(RunningPath, @"../../")));
            }
        }

    }
}
