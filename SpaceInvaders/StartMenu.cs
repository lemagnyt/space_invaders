using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.IO;
using System.Security.Cryptography;

namespace SpaceInvaders
{
    
    public partial class StartMenu : Form
    {
        public GameForm gameForm;
        public String skin = "playership";
        public String pseudo = "";
        private string RunningPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        /// <summary>
        /// Create a start menu
        /// </summary>
        public StartMenu()
        {
            InitializeComponent();
            chargeBestScore();
            gameForm = null;
        }

        /// <summary>
        /// function which charge besto score
        /// </summary>
        public void chargeBestScore()
        {
            string HightScoreText = string.Format(@"{0}Resources\HightScore.txt", Path.GetFullPath(Path.Combine(RunningPath, @"../../")));
            String[] HightScore = File.ReadAllLines(HightScoreText);
            String[] BestScore = HightScore[0].Split(':');
            if (BestScore.Length < 2)
            {
                File.WriteAllText(HightScoreText, "Test:0");
                HightScore = File.ReadAllLines(HightScoreText);
                BestScore = HightScore[0].Split(':');
            }
            label5.Text = BestScore[1];
            label8.Text = BestScore[0];
        }


        /// <summary>
        /// Start the game by closing this window and opening the GameForm Window and giving the good skin for the player spaceship
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartGame(object sender, EventArgs e)
        {
            if (textBox2.Text.Length != 0 && !textBox2.Text.Contains(":")) {
                pseudo = textBox2.Text;
                String[] tab = { skin, pseudo };
                string SkinText = string.Format(@"{0}Resources\Skin.txt", Path.GetFullPath(Path.Combine(RunningPath, @"../../")));
                File.WriteAllLines(SkinText, tab);
                GameForm f = new GameForm();
                f.FormClosing += new FormClosingEventHandler(f_FormClosing);
                this.Hide();
                f.Show();
            }
            else
            {
                label3.Show();
            }
        }
        private void f_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Go to window Shop to change skin and hide this window when doing it
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            Shop shop = new Shop(this);
            shop.ShowDialog();
        }

        /// <summary>
        /// Recharge the score
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            chargeBestScore();
        }
    }
}
