using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders
{
    internal class Coeur : SimpleObject
    {
        /// <summary>
        /// Create a heart
        /// </summary>
        /// <param name="p">position</param>
        public Coeur(Vecteur2D p) : base(p, 1, Game.GetImageByName("vie"), Side.Neutral){}

        protected override void OnCollision(SimpleObject m, int numberOfPixelsInCollision, HashSet<int[]> PixelList){}
       
        /// <summary>
        /// Update the heart position and kills it if it is a the end of the screen
        /// </summary>
        /// <param name="gameInstance">the game where there is the heart</param>
        /// <param name="deltaT">the time between 2 updates</param>
        public override void Update(Game gameInstance, double deltaT){
            this.position.y += 0.75;
            if (this.position.y + this.Image.Height >= gameInstance.gameSize.Height){
                this.Lives = 0;
            }
            gameInstance.playerShip.Collision(this);
        }
    }
}
