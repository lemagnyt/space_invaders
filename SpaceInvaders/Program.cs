using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SpaceInvaders
{
    static class Program
    {
        public static bool goToMenu = false;
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// We first start by opening the Start Menu. Then we will open new startMenu each time the game is restart.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            StartMenu startMenu = new StartMenu();
            Application.Run(startMenu);
            startMenu.Close();
            while (goToMenu)
            {
                goToMenu = false;
                startMenu = new StartMenu();
                Application.Run(startMenu);

            }
        }
    }
}
