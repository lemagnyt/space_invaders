using System;
using System.Collections.Generic;
using System.Drawing;

namespace SpaceInvaders
{
    abstract class SimpleObject : GameObject
    {
        public Vecteur2D position { get; set; }
        public int Lives { get; set; }
        public Bitmap Image { get; set; }

        /// <summary>
        /// Create a simple object and initialize itsp osition, number of lives, image, and side
        /// </summary>
        /// <param name="p">position</param>
        /// <param name="lives">its number of lives</param>
        /// <param name="image">its image</param>
        /// <param name="objectSide">side</param>
        public SimpleObject(Vecteur2D p, int lives, Bitmap image, Side objectSide) : base(objectSide){
            this.position = p;
            this.Lives = lives;
            this.Image = image;
        }

        /// <summary>
        /// Draw the image on the game screen
        /// </summary>
        /// <param name="gameInstance"></param>
        /// <param name="graphics"></param>
        public override void Draw(Game gameInstance, Graphics graphics){
            graphics.DrawImage(this.Image, (float)this.position.x, (float)this.position.y, this.Image.Width, this.Image.Height);
        }

        /// <summary>
        /// Return if the object has still lives or no
        /// </summary>
        /// <returns></returns>
        public override bool IsAlive(){
            return (this.Lives > 0);
        }

        /// <summary>
        /// Detect the collision of the object with another one and do an action if there are pixel in collision between the two.
        /// To know that we search in each pixel of the two image and we compare the position with the other one. 
        /// Then we see if its a transparent pixel for each or not. If it is there is a collision. 
        /// And the pixel list is not empty.
        /// </summary>
        /// <param name="m">other object</param>
        public override void Collision(SimpleObject m){
            if (m.ObjectSide == this.ObjectSide){
                return; 
            }
            int nbPixel = 0;
            HashSet<int[]> PixelList = new HashSet<int[]>();
            if (CollisionRectangle(m)){
                for (int i = 0; i < m.Image.Width; i++){
                    for (int j = 0; j < m.Image.Height; j++){
                        int iObjekt = (int)(i + m.position.x - this.position.x);
                        int jObjekt = (int)(j + m.position.y - this.position.y);
                        if (iObjekt >= 0 && iObjekt < Image.Width){
                            if (jObjekt >= 0 && jObjekt < Image.Height){
                                if (this.Image.GetPixel(iObjekt, jObjekt) != Color.FromArgb(0, 0, 0, 0) && m.Image.GetPixel(i, j) != Color.FromArgb(0,0,0,0))
                                {
                                    PixelList.Add(new int[] {iObjekt,jObjekt});
                                    nbPixel += 1;
                                }
                            }
                        }
                    }
                }
                if (nbPixel > 0){
                    OnCollision(m, nbPixel, PixelList);
                }
            }
        }

        /// <summary>
        /// See if the rectangle of the two image objects are in collision, not verifying the pixels for the moment
        /// </summary>
        /// <param name="m">other object</param>
        /// <returns></returns>
        bool CollisionRectangle(SimpleObject m){
            bool droite = m.position.x > this.position.x + this.Image.Width;
            bool gauche = this.position.x > m.position.x + m.Image.Width;
            bool haut = this.position.y > m.position.y + m.Image.Height;
            bool bas = m.position.y > this.position.y + this.Image.Height;
            return !(droite || bas || gauche | haut);
        }

        /// <summary>
        /// Go from a repere to another one
        /// </summary>
        /// <param name="v1">the position of the object in the first repere</param>
        /// <param name="o2">the origin of the second repair</param>
        /// <returns>return the position of the object in the second repere</returns>
        Vecteur2D Repere1aRepere2(Vecteur2D v1, Vecteur2D o2){
            return (v1+o2);
        }
        protected abstract void OnCollision(SimpleObject m, int numberOfPixelsInCollision, HashSet<int[]> PixelList);
    }
}