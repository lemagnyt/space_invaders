using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using WMPLib;

namespace SpaceInvaders
{
    internal class Bunker : SimpleObject
    {
        /// <summary>
        /// Constructor bunker
        /// </summary>
        /// <param name="position"></param>
        /// <param name="image"></param>
        public Bunker(Vecteur2D position,Bitmap image) :base(position,1, image,Side.Neutral){}

        /// <summary>
        /// Overriding the Update method which does nothing
        /// </summary>
        /// <param name="gameInstance"></param>
        /// <param name="deltaT"></param>
        public override void Update(Game gameInstance, double deltaT){}

        /// <summary>
        /// Each missile will test if it collides with other game objects. 
        /// First, we go through all the game objects (gameInstance.gameObjects list). 
        /// Then we call the Collision method on these objects.
        /// </summary>
        /// <param name="m"></param>
        /// <param name="numberOfPixelsInCollision"></param>
        /// <param name="PixelList"></param>
        protected override void OnCollision(SimpleObject m, int numberOfPixelsInCollision, HashSet<int[]> PixelList){
            m.Lives -= numberOfPixelsInCollision;
            if (m.Lives < 0){
                m.Lives = 0;
            }
            if (m.GetType().Equals(typeof(Missile))){
                m.Lives -= numberOfPixelsInCollision;
                if (m.Lives < 0){
                    m.Lives = 0;
                }
                foreach (int[] Pixel in PixelList){
                    this.Image.SetPixel(Pixel[0], Pixel[1], Color.FromArgb(0, 0, 0, 0));
                }
            }
        }

    }
}
