
using System;
using System.Windows.Forms;

namespace ChatBot
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Initialise Windows Forms application settings.
            ApplicationConfiguration.Initialize();

            // Open the main chatbot window.
            Application.Run(new Form1());
        }
    }
}
