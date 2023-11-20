using System;

namespace StarterGame
{
    /*
     * Spring 2023
     */
    class Program
    {
        static void Main(string[] args)
        {
            //Creates new instance of game
            //Calls its startup, actual loop, and ending
            
            Game game = new Game();
            game.Start();
            game.Play();
            game.End();
        }
    }
}
