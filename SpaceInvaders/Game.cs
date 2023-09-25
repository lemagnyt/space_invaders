using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace SpaceInvaders
{
    /// <summary>
    /// This class represents the entire game, it implements the singleton pattern
    /// </summary>
    class Game
    {

        #region GameObjects management

        /// <summary>
        /// Set of all game objects currently in the game
        /// </summary>
        public HashSet<GameObject> gameObjects;

        /// <summary>
        /// Set of new game objects scheduled for addition to the game
        /// </summary>
        private HashSet<GameObject> pendingNewGameObjects = new HashSet<GameObject>();

        /// <summary>
        /// Schedule a new object for addition in the game.
        /// The new object will be added at the beginning of the next update loop
        /// </summary>
        /// <param name="gameObject">object to add</param>
        public void AddNewGameObject(GameObject gameObject)
        {
            pendingNewGameObjects.Add(gameObject);
        }
        #endregion

        #region game technical elements
        /// <summary>
        /// Size of the game area
        /// </summary>
        public Size gameSize;
        private double duree;
        public PlayerSpaceship playerShip;
        private EnemyBlock enemies;
        public Bunker bunker1;
        public int level;
        public double playerSpeed;
        private string RunningPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        public enum GameState { Play, Pause, Lost, Win };
        public GameState state;
        public int Score;
        public WMPLib.WindowsMediaPlayer soundplayer = new WMPLib.WindowsMediaPlayer();
        private bool canRestart;
        /// <summary>
        /// State of the keyboard
        /// </summary>
        public HashSet<Keys> keyPressed = new HashSet<Keys>();

        #endregion

        #region static fields (helpers)

        /// <summary>
        /// Singleton for easy access
        /// </summary>
        public static Game game { get; private set; }

        /// <summary>
        /// A shared black brush
        /// </summary>
        private static Brush blackBrush = new SolidBrush(Color.Black);

        /// <summary>
        /// A shared simple font
        /// </summary>
        private static Font defaultFont = new Font("Times New Roman", 24, FontStyle.Bold, GraphicsUnit.Pixel);

        /// <summary>
        /// A shared media player
        /// </summary>
        private static WMPLib.WindowsMediaPlayer musicplayer = new WMPLib.WindowsMediaPlayer();
        #endregion

        #region constructors
        /// <summary>
        /// Singleton constructor
        /// </summary>
        /// <param name="gameSize">Size of the game area</param>
        /// 
        /// <returns></returns>
        public static Game CreateGame(Size gameSize)
        {
            game = new Game(gameSize);
            return game;
        }

        /// <summary>
        /// Gives an image in the Ressources by giving its name
        /// </summary>
        /// <param name="imageName">name of the image that we need</param>
        /// <returns>returns the image corresponding</returns>
        public static Bitmap GetImageByName(string imageName)
        {
            System.Reflection.Assembly asm = System.Reflection.Assembly.GetExecutingAssembly();
            string resourceName = asm.GetName().Name + ".Properties.Resources";
            var rm = new System.Resources.ResourceManager(resourceName, asm);
            return (Bitmap)rm.GetObject(imageName);
        }

        /// <summary>
        /// Charge the level by reading it in a text
        /// </summary>
        /// <param name="level">the level number</param>
        public void Init_level(int level)
        {
            soundplayer.URL = string.Format(@"{0}Resources\debut.wav", Path.GetFullPath(Path.Combine(RunningPath, @"../../")));
            int startLevel = 0;
            string LevelText = string.Format(@"{0}Resources\Level.txt", Path.GetFullPath(Path.Combine(RunningPath, @"../../")));
            string[] readText = File.ReadAllLines(LevelText);
            while (readText[startLevel] != ("Level " + level)){
                startLevel++;
            }
            gameObjects = new HashSet<GameObject>();
            string[] bunkerConfig = readText[startLevel + 2].Split('|');
            Bitmap imageBunker = GetImageByName(bunkerConfig[1]);
            int nbBunker = int.Parse(bunkerConfig[2]);

            string[] enemyBlockConfig = readText[startLevel + 3].Split('|');
            int enemyBw = int.Parse(enemyBlockConfig[1]);
            double probaShoot = double.Parse(enemyBlockConfig[2]);
            string[] enemyLines = enemyBlockConfig[3].Split(':');

            this.state = GameState.Play;

            this.playerShip.invincible = false;
            this.playerShip.Timer = 0;
            this.playerSpeed = double.Parse(readText[startLevel + 1].Split('|')[1]);
            double positionX = gameSize.Width / 2 - this.playerShip.Image.Width/2;
            double positionY = gameSize.Height - this.playerShip.Image.Height;
            this.playerShip.position= new Vecteur2D(positionX, positionY);
            AddNewGameObject(this.playerShip);

            float yBunker = (float)(gameSize.Height - imageBunker.Height - 80);
            float xBunker;
            double space = (gameSize.Width - nbBunker * imageBunker.Width) / (nbBunker + 1);
            for (int i = 0; i < nbBunker; i++){
                xBunker = (float)(space+i*(imageBunker.Width + space));
                Bunker bunker = new Bunker(new Vecteur2D(xBunker,yBunker),(Bitmap)imageBunker.Clone());
                AddNewGameObject(bunker);
                if (i == 0)
                {
                    this.bunker1 = bunker;
                }
            }
            if (this.bunker1 == null)
            {
                Bunker bunker = new Bunker(new Vecteur2D(0, 0), (Bitmap)imageBunker.Clone());
            }

            this.enemies = new EnemyBlock(enemyBw, new Vecteur2D(gameSize.Width / 2 - enemyBw/2, 60), probaShoot);
            foreach(string l in enemyLines){
                string[] tab = l.Split(',');
                int score = int.Parse(tab[2].Split('p')[1])*10;
                this.enemies.AddLine(int.Parse(tab[0]), int.Parse(tab[1]), GetImageByName(tab[2]),score);
            }
            AddNewGameObject(this.enemies);
        }

        /// <summary>
        /// Creates the game and set its starting parameters
        /// </summary>
        /// <param name="gameSize">Size of the screen</param>
        public void Init_Game(Size gameSize){
            string musicpath = string.Format(@"{0}Resources\fond.wav", Path.GetFullPath(Path.Combine(RunningPath, @"../../")));
            musicplayer.URL = musicpath;
            musicplayer.settings.setMode("loop", true);
            this.duree = 0;
            string SkinText = string.Format(@"{0}Resources\Skin.txt", Path.GetFullPath(Path.Combine(RunningPath, @"../../")));
            Bitmap skin = GetImageByName(File.ReadAllLines(SkinText)[0]);
            double positionX = gameSize.Width / 2 - skin.Width/2;
            double positionY = gameSize.Height - skin.Height;
            this.playerShip = new PlayerSpaceship(new Vecteur2D(positionX, positionY), 150, skin);
            this.Score = 0;
            this.gameSize = gameSize;
            this.level = 1;
            this.Init_level(this.level);
        }

        /// <summary>
        /// Private constructor
        /// </summary>
        /// <param name="gameSize">Size of the game area</param>
        private Game(Size gameSize){
            Init_Game(gameSize);
            canRestart = true;
        }

        #endregion

        #region methods

        /// <summary>
        /// Force a given key to be ignored in following updates until the user
        /// explicitily retype it or the system autofires it again.
        /// </summary>
        /// <param name="key">key to ignore</param>
        public void ReleaseKey(Keys key)
        {
            keyPressed.Remove(key);
        }

        /// <summary>
        /// Draw the whole game
        /// </summary>
        /// <param name="g">Graphics to draw in</param>
        public void Draw(Graphics g)
        {
            Font drawFont = new Font("Arial", 16);
            SolidBrush drawBrush = new SolidBrush(Color.White);
            SolidBrush drawBrushBouclier = new SolidBrush(Color.Yellow);
            g.DrawString("LEVEL " + this.level, drawFont, drawBrush, (gameSize.Width / 2) - 50, 0);
            if (this.playerShip != null)
            {
                Bitmap vie = GetImageByName("vie");
                Bitmap nonvie = GetImageByName("nonvie");
                double xStartLives = this.gameSize.Width - 5 * vie.Width - 20;
                for (int i = 0; i < 5; i++)
                {
                    if (i < this.playerShip.Lives / 30)
                    {
                        g.DrawImage(vie, (float)(xStartLives + i * (vie.Width + 3)), 0, vie.Width, vie.Height);
                    }
                    else
                    {
                        g.DrawImage(nonvie, (float)(xStartLives + i * (vie.Width + 3)), 0, vie.Width, vie.Height);
                    }
                }
                if (this.playerShip.invincible)
                {
                    Bitmap imageBouclier = GetImageByName("itembouclier");
                    g.DrawImage(imageBouclier, (float)xStartLives-140, 0, imageBouclier.Width, imageBouclier.Height);
                    g.DrawString("" + (20 - (int)this.playerShip.Timer), drawFont, drawBrushBouclier, (float)xStartLives - 100, 0);
                }
            }
            g.DrawString("Score : " + this.Score, drawFont, drawBrush, 20, 0);
            g.DrawString("Time : " + (int)this.duree + "s / 500s", drawFont, drawBrush, 200, 0);

            if (state == GameState.Pause){
                g.DrawString("PAUSE", drawFont, drawBrush, (gameSize.Width / 2) - 35, gameSize.Height / 2);
            }

            if (state == GameState.Lost){
                g.DrawString("Lost", drawFont, drawBrush, (gameSize.Width / 2) - 30, gameSize.Height / 2 - 40);
                g.DrawString("Score : " + this.Score, drawFont, drawBrush, (gameSize.Width / 2)-50 , gameSize.Height / 2 - 10);
            }

            if (state == GameState.Win){
                g.DrawString("Win", drawFont, drawBrush, (gameSize.Width / 2) - 40, gameSize.Height / 2 -180);
                g.DrawString("Score : "+this.Score, drawFont, drawBrush, (gameSize.Width / 2) - 150, gameSize.Height / 2 -100);
                g.DrawString("Bonus Lives : "+this.playerShip.Lives/30+" x 30 = + "+this.playerShip.Lives, drawFont, drawBrush, (gameSize.Width / 2) - 150, gameSize.Height / 2 -70);
                g.DrawString("Bonus Time : 500 - " + (int)this.duree+" = + " + (500-(int)this.duree), drawFont, drawBrush, (gameSize.Width / 2) - 150, gameSize.Height / 2 -40);
                g.DrawString("Final Score : "+this.Score+" + "+this.playerShip.Lives+" + "+ (500-(int)this.duree)+" = "+WithBonusScore(), drawFont, drawBrush, (gameSize.Width / 2) - 150, gameSize.Height / 2 -10);
            }


            foreach (GameObject gameObject in gameObjects){
                gameObject.Draw(this, g);
            }

            if (this.state == GameState.Win || this.state == GameState.Lost || this.state == GameState.Pause)
            {
                g.DrawString("Press on <SPACE> to restart the game", drawFont, drawBrush, (gameSize.Width / 2) - 170, gameSize.Height / 2+ 30);
                g.DrawString("Press on <R> to return to menu", drawFont, drawBrush, (gameSize.Width / 2) - 135, gameSize.Height / 2 + 60);
            }


        }

        /// <summary>
        ///  returns the score with all its bonus added depending if you have win or not.
        /// </summary>
        /// <returns>returns the final score</returns>
        public int WithBonusScore()
        {
            if (this.state == GameState.Win) return this.Score + this.playerShip.Lives + 500 - (int)this.duree;
            else return this.Score;
        }


        /// <summary>
        /// Saves your score and pseudo in a file. And then sorts the file from the biggest to the smallest score 
        /// </summary>
        public void SaveScore()
        {
            int i = 0;
            string SkinText = string.Format(@"{0}Resources\Skin.txt", Path.GetFullPath(Path.Combine(RunningPath, @"../../")));
            string HightScoreText = string.Format(@"{0}Resources\HightScore.txt", Path.GetFullPath(Path.Combine(RunningPath, @"../../")));
            String pseudo = File.ReadAllLines(SkinText)[1];
            String[] ScoreRanking = File.ReadAllLines(HightScoreText);
            String[] newScoreRanking = new String[ScoreRanking.Length + 1];
            while (i < ScoreRanking.Length && int.Parse(ScoreRanking[i].Split(':')[1]) > WithBonusScore()){
                newScoreRanking[i] = ScoreRanking[i];
                i++;
            }
            newScoreRanking[i] = pseudo + ":" + WithBonusScore();
            for (int j = i+1; j < newScoreRanking.Length; j++){
                newScoreRanking[j] = ScoreRanking[j-1];
            }
            File.WriteAllLines(HightScoreText, newScoreRanking);
        }

        /// <summary>
        /// Update game depending on the state.
        /// </summary>
        /// 
        public void Update(double deltaT)
        {
            gameObjects.UnionWith(pendingNewGameObjects);// add new game objects
            pendingNewGameObjects.Clear();

            if (keyPressed.Contains(Keys.P)){
                string musicpath = string.Format(@"{0}Resources\pause.wav", Path.GetFullPath(Path.Combine(RunningPath, @"../../")));
                soundplayer.URL = musicpath;
                if (this.state == GameState.Pause){
                    musicplayer.controls.play();
                    this.state = GameState.Play;
                }
                else if (this.state == GameState.Play)
                {
                    musicplayer.controls.pause();
                    this.state = GameState.Pause;
                }  
                ReleaseKey(Keys.P);
            }

            if (this.state == GameState.Play){
                this.duree += deltaT;
                foreach (GameObject gameObject in gameObjects){
                    // update each game object
                    gameObject.Update(this, deltaT);
                    if (this.enemies.size.Height + this.enemies.Position.y > this.bunker1.position.y){
                        foreach (Enemy enemi in this.enemies.enemyShips){
                            enemi.Lives = 0;
                        }
                        this.state = GameState.Lost;
                        SaveScore();
                        break;
                    }
                    else if (!this.enemies.IsAlive()){
                        if (this.level == 5){
                            state = GameState.Win;
                            SaveScore();
                            musicplayer.controls.pause();
                            soundplayer.URL = string.Format(@"{0}Resources\win.wav", Path.GetFullPath(Path.Combine(RunningPath, @"../../")));
                        }
                        else{
                            this.level += 1;
                            Init_level(this.level);
                        }
                    } 
                }
                if (this.playerShip.Lives <= 0 || this.duree>=500){
                    this.state = GameState.Lost;
                    SaveScore();
                    musicplayer.controls.pause();
                    soundplayer.URL = string.Format(@"{0}Resources\fin.wav", Path.GetFullPath(Path.Combine(RunningPath, @"../../")));
                }
            }
            else if(this.state == GameState.Win || this.state == GameState.Lost || this.state == GameState.Pause){
                if (this.state == GameState.Win || this.state == GameState.Lost)
                {
                    gameObjects.RemoveWhere(gameObject => true);
                    game = null;
                }
                if (keyPressed.Contains(Keys.Space))
                {
                    this.playerShip.Lives = 0;// remove all objects
                    Init_Game(gameSize);
                    ReleaseKey(Keys.Space);
                }
                else if (keyPressed.Contains(Keys.R) && canRestart)
                {
                    Program.goToMenu = true;                    
                }
            }

            gameObjects.RemoveWhere(gameObject => !gameObject.IsAlive());// remove dead objects
        }
        #endregion

    }
}
