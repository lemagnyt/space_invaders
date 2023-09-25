using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace SpaceInvaders
{
    public partial class Shop : Form
    {
        public StartMenu startMenu;
        public Shop(StartMenu sm)
        {
            InitializeComponent();
            startMenu = sm;
            buttonselected.Image = Game.GetImageByName(sm.skin);
        }

        /// <summary>
        /// Closes the shop. And gives the skin selected here to the startmenu.
        /// </summary>
        /// <param name="sender">the sender</param>
        /// <param name="e">the event</param>
        private void CloseShop(object sender, EventArgs e)
        {
            startMenu.buttonselected.Image = Game.GetImageByName(startMenu.skin);
            startMenu.button3.Image = Game.GetImageByName(startMenu.skin);
            this.Close();
        }
        /// <summary>
        /// Same for all the buttons of the shop. When you click on it, it selects a certains image skin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            startMenu.skin = "playership";
            buttonselected.Image = Game.GetImageByName(startMenu.skin);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            startMenu.skin = "playership1";
            buttonselected.Image = Game.GetImageByName(startMenu.skin);
        }
        private void button3_Click(object sender, EventArgs e)
        {
            startMenu.skin = "playership2";
            buttonselected.Image = Game.GetImageByName(startMenu.skin);
        }
        private void button4_Click(object sender, EventArgs e)
        {
            startMenu.skin = "playership3";
            buttonselected.Image = Game.GetImageByName(startMenu.skin);
        }
        private void button5_Click(object sender, EventArgs e)
        {
            startMenu.skin = "playership4";
            buttonselected.Image = Game.GetImageByName(startMenu.skin);
        }
        private void button6_Click(object sender, EventArgs e)
        {
            startMenu.skin = "playership5";
            buttonselected.Image = Game.GetImageByName(startMenu.skin);
        }
        private void button7_Click(object sender, EventArgs e)
        {
            startMenu.skin = "playership6";
            buttonselected.Image = Game.GetImageByName(startMenu.skin);
        }
        private void button8_Click(object sender, EventArgs e)
        {
            startMenu.skin = "playership7";
            buttonselected.Image = Game.GetImageByName(startMenu.skin);
        }
        private void button9_Click(object sender, EventArgs e)
        {
            startMenu.skin = "playership8";
            buttonselected.Image = Game.GetImageByName(startMenu.skin);
        }
        private void button10_Click(object sender, EventArgs e)
        {
            startMenu.skin = "playership9";
            buttonselected.Image = Game.GetImageByName(startMenu.skin);
        }
        private void button11_Click(object sender, EventArgs e)
        {
            startMenu.skin = "playership10";
            buttonselected.Image = Game.GetImageByName(startMenu.skin);
        }
        private void button12_Click(object sender, EventArgs e)
        {
            startMenu.skin = "playership11";
            buttonselected.Image = Game.GetImageByName(startMenu.skin);
        }
        private void button13_Click(object sender, EventArgs e)
        {
            startMenu.skin = "playership12";
            buttonselected.Image = Game.GetImageByName(startMenu.skin);
        }
        private void button14_Click(object sender, EventArgs e)
        {
            startMenu.skin = "playership13";
            buttonselected.Image = Game.GetImageByName(startMenu.skin);
        }
        private void button15_Click(object sender, EventArgs e)
        {
            startMenu.skin = "playership14";
            buttonselected.Image = Game.GetImageByName(startMenu.skin);
        }
        private void button16_Click(object sender, EventArgs e)
        {
            startMenu.skin = "playership15";
            buttonselected.Image = Game.GetImageByName(startMenu.skin);
        }
        private void button17_Click(object sender, EventArgs e)
        {
            startMenu.skin = "playership16";
            buttonselected.Image = Game.GetImageByName(startMenu.skin);
        }
        private void button18_Click(object sender, EventArgs e)
        {
            startMenu.skin = "playership17";
            buttonselected.Image = Game.GetImageByName(startMenu.skin);
        }
    }
}
