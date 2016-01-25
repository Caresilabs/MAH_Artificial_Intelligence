﻿using MAH_Platformer;
using System;

namespace FSMGame
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (var game = new Start())
                game.Run();
        }
    }
#endif
}
