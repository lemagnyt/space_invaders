using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SpaceInvaders.GameObject;

namespace SpaceInvaders
{
    internal class Bouclier : SimpleObject
    {
        /// <summary>
        /// Create a shield
        /// </summary>
        /// <param name="p">position</param>
        public Bouclier(Vecteur2D p) : base(p, 1, Game.GetImageByName("itembouclier"), Side.Neutral){}

        protected override void OnCollision(SimpleObject m, int numberOfPixelsInCollision, HashSet<int[]> PixelList){}

        /// <summary>
        /// Update the shield position and its lives if it leaves the screen
        /// </summary>
        /// <param name="gameInstance">the game where is the shield</param>
        /// <param name="deltaT">time between 2 updates</param>
        public override void Update(Game gameInstance, double deltaT){
            this.position.y += 0.75;
            if (this.position.y + this.Image.Height >= gameInstance.gameSize.Height) {
                this.Lives = 0;
            }
            gameInstance.playerShip.Collision(this);
        }
    }
}
