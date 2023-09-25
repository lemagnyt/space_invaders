using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace SpaceInvaders
{
    internal class EnemyBlock : GameObject
    {
        private Random random = new Random();
        private string RunningPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        public double dx {  get; set; }
        private double randomShootProbability;
        public double Timer;
        public double moveTime;
        /// <summary>
        /// All Block Ships
        /// </summary>
        public HashSet<SpaceShip> enemyShips;
        /// <summary>
        /// Width of the block at the time of its creation
        /// </summary>
        private int baseWidth;
        /// <summary>
        /// Block size (width, height), adapted as the game progresses.
        /// </summary>
        public Size size{ get; set; }
        /// <summary>
        /// Block size (width, height), adapted as the game progresses.
        /// </summary>
        public Vecteur2D Position { get; set; }
        /// <summary>
        /// Adds a new line of enemies to the block: see full description below.
        /// </summary>
        /// <param name="bw"></param>
        /// <param name="position"></param>
        /// <param name="rShoot"></param>
        /// 
        public EnemyBlock(int bw, Vecteur2D position, double rShoot):base(Side.Enemy){
            this.moveTime = 0.75;
            this.baseWidth = bw;
            this.Position = position;
            this.enemyShips= new HashSet<SpaceShip>();
            this.size= new Size(bw,0);
            this.dx = 15;
            this.randomShootProbability = rShoot;
        }

        /// <summary>
        /// Adds a new line of enemies to the block: see full description below.
        /// </summary>
        /// <param name="nbShips"></param>
        /// <param name="nbLives"></param>
        /// <param name="shipImage"></param>
        /// <param name="score"></param>
        public void AddLine(int nbShips, int nbLives, Bitmap shipImage, int score){
            this.size = new Size(size.Width, size.Height+shipImage.Height+5);
            for (int i = 0; i < nbShips; i++){
                double space = (this.baseWidth - (nbShips * shipImage.Width/2)) / (nbShips + 1);
                this.enemyShips.Add(new Enemy((new Vecteur2D(this.Position.x + space + i * (shipImage.Width/2 + space), this.Position.y + this.size.Height+5)), nbLives, shipImage, score));
            }
        }

        /// <summary>
        /// Recalculates the size and position of the block based on the ships it contains.
        /// </summary>
        public void UpdateSize(){
            double xmin = this.Position.x + this.size.Width;
            double xmax = this.Position.x;
            double ymin = this.Position.y + this.size.Height;
            double ymax = this.Position.y;
            foreach (Enemy enemyShip in enemyShips){
                if (xmin > enemyShip.position.x){
                    xmin = enemyShip.position.x; 
                }
                if(xmax < enemyShip.position.x + enemyShip.Image.Width){
                    xmax = enemyShip.position.x + enemyShip.Image.Width; 
                }
                if (ymin > enemyShip.position.y){ 
                    ymin = enemyShip.position.y; 
                }
                if (ymax < enemyShip.position.y + enemyShip.Image.Height){ 
                    ymax = enemyShip.position.y + enemyShip.Image.Height; 
                }
            }
            this.Position = new Vecteur2D(xmin, ymin);
            this.size = new Size((int)(xmax - xmin), (int)(ymax - ymin));
        }

        /// <summary>
        ///  Update the enemy block by moving it, make some enemy shot. Probability and speed rise as things progress.
        ///  When they die there is also a probability that an item is drop.
        /// </summary>
        /// <param name="gameInstance">the game in which the enemyblock is</param>
        /// <param name="deltaT">the time between two updates</param>
        public override void Update(Game gameInstance, double deltaT){
            bool limit = (this.Position.x + this.size.Width + this.dx > gameInstance.gameSize.Width && this.dx > 0) || (this.Position.x + this.dx < 0 && this.dx < 0);
            this.Timer += deltaT;
            this.UpdateSize();
            foreach (Enemy enemyShip in enemyShips){
                if (Timer > moveTime){
                    if (limit){
                        enemyShip.position.y += 20;
                    }
                    else{
                        enemyShip.position.x += this.dx;
                        double r = random.NextDouble();
                        if (r > (1 - randomShootProbability) && enemyShip.Lives>0){
                            enemyShip.Shoot(gameInstance);
                        }
                    }
                }
                enemyShip.Update(gameInstance, deltaT);
                if (!enemyShip.IsAlive()){
                    gameInstance.Score += enemyShip.Score;
                    if (randomShootProbability < 0.5){
                        randomShootProbability += 0.03;
                    }
                    double r = random.NextDouble();
                    if (r > 0.7){
                        addCoeur(gameInstance,enemyShip);
                    }
                    else if (r < 0.2 && !gameInstance.playerShip.invincible){
                        addBouclier(gameInstance, enemyShip);
                    }
                }
            }
            if (this.Timer > this.moveTime){
                if (limit){
                    if (randomShootProbability < 0.3){
                        this.randomShootProbability += 0.025;
                    }
                    if (this.moveTime >= 0.4){
                        this.moveTime -= 0.075;
                    }
                    this.dx *= -1;
                }
                this.Timer = 0;
            }
            this.enemyShips.RemoveWhere(ship => !ship.IsAlive());
        }
        /// <summary>
        /// Draw the vessels of the block
        /// </summary>
        /// <param name="gameInstance"></param>
        /// <param name="graphics"></param>
        public override void Draw(Game gameInstance, Graphics graphics){
            foreach(Enemy enemyShip in enemyShips){
                if (enemyShip.IsAlive()){
                    enemyShip.Draw(gameInstance, graphics);
                }
                enemyShip.Draw(gameInstance, graphics);
            }
        }
        /// <summary>
        /// Returns true if there is at least 1 living ship in the block
        /// </summary>
        /// <returns></returns>
        public override bool IsAlive(){
            foreach (Enemy enemyShip in enemyShips){
                if (enemyShip.Lives > 0){
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Verify the collision if the enemyship has still life because he can still be in explosion mode
        /// </summary>
        /// <param name="m">objects with which there is a collision</param>
        public override void Collision(SimpleObject m){
            foreach (Enemy enemyShip in enemyShips){
                if (enemyShip.Lives > 0)
                {
                    enemyShip.Collision(m);
                }
            }
        }

        /// <summary>
        /// Adds a heart in the game depending on which enemy ship is dead
        /// </summary>
        /// <param name="gameInstance">the game where the heart is added</param>
        /// <param name="enemyShip">the enemy ship which gives the heart when we kill it</param>
        public void addCoeur(Game gameInstance, Enemy enemyShip){
            Bitmap vie = Game.GetImageByName("vie");
            float xVie = (float)(enemyShip.position.x + enemyShip.Image.Width / 2 - vie.Width / 2);
            float yVie = (float)(enemyShip.position.y + enemyShip.Image.Height / 2 - vie.Height / 2);
            Coeur coeur = new Coeur(new Vecteur2D(xVie, yVie));
            gameInstance.AddNewGameObject(coeur);
        }

        /// <summary>
        /// Adds a shield in the game depending on which enemy ship is dead
        /// </summary>
        /// <param name="gameInstance">the game where the shield is added</param>
        /// <param name="enemyShip">the enemy ship which gives the shield when we kill it</param>
        public void addBouclier(Game gameInstance, Enemy enemyShip) {
            Bitmap bouclierImage = Game.GetImageByName("itembouclier");
            float x = (float)(enemyShip.position.x + enemyShip.Image.Width / 2 - bouclierImage.Width / 2);
            float y = (float)(enemyShip.position.y + enemyShip.Image.Height / 2 - bouclierImage.Height / 2);
            Bouclier bouclier = new Bouclier(new Vecteur2D(x, y));
            gameInstance.AddNewGameObject(bouclier);
        }
    }
}
