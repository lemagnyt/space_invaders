using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpaceInvaders
{
    internal class Vecteur2D
    {
        public double x;
        public double y;

        /// <summary>
        /// Give the norme of a vector
        /// </summary>
        public double Norme{
            get { return Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2)); }
            set { this.Norme = Norme; }
        }

        /// <summary>
        /// Create a Vecteur in 2 dimension
        /// </summary>
        /// <param name="x">x position</param>
        /// <param name="y">y position</param>
        public Vecteur2D(double x, double y){
            this.x = x; this.y = y;
        }


        #region Vecteur2D operator
        /// <summary>
        /// addition of vector
        /// </summary>
        /// <param name="v1">first vector</param>
        /// <param name="v2">second vector</param>
        /// <returns>result of addition</returns>
        public static Vecteur2D operator+(Vecteur2D v1, Vecteur2D v2){
            return new Vecteur2D(v1.x + v2.x, v1.y + v2.y);
        }

        /// <summary>
        /// soustraction of vector
        /// </summary>
        /// <param name="v1">first vector</param>
        /// <param name="v2">second vector</param>
        /// <returns>result of soustraction</returns>
        public static Vecteur2D operator -(Vecteur2D v1, Vecteur2D v2){
            return new Vecteur2D(v1.x - v2.x, v1.y - v2.y);
        }

        /// <summary>
        /// negation of vector
        /// </summary>
        /// <param name="v">vector</param>
        /// <returns>return the negation</returns>
        public static Vecteur2D operator -(Vecteur2D v){
            return new Vecteur2D(-v.x,-v.y);
        }

        /// <summary>
        /// multiplication double vector
        /// </summary>
        /// <param name="v">vector</param>
        /// <param name="d">double</param>
        /// <returns>result of multiplication</returns>
        public static Vecteur2D operator *(Vecteur2D v, double d){
            return new Vecteur2D(d*v.x, d*v.y);
        }

        /// <summary>
        /// multiplication vector double
        /// </summary>
        /// <param name="d">double</param>
        /// <param name="v">vector</param>
        /// <returns>result of multiplication</returns>
        public static Vecteur2D operator *(double d,Vecteur2D v){
            return new Vecteur2D(d * v.x, d * v.y);
        }
        /// <summary>
        /// division vector double
        /// </summary>
        /// <param name="v">vector</param>
        /// <param name="d">double</param>
        /// <returns>result of division</returns>
        public static Vecteur2D operator /(Vecteur2D v, double d){
            return new Vecteur2D(v.x/d, v.y/d);
        }
        #endregion

    }
}
