using System;
using System.Drawing;
using System.IO;
using System.Media;
using System.Windows.Forms;

namespace ChatBot
{
    public partial class Form1 : Form
    {
        private SoundPlayer? player;
        private readonly ChatEngine engine = new ChatEngine();

        public Form1()
        {
            InitializeComponent();
        }

        // Load the logo, play the greeting, and welcome the user.
        private void Form1_Load(object sender, EventArgs e)
        {
            DisplayLogo();
            PlayGreeting();

            AddBotMessage("Welcome to the Cybersecurity Awareness Chatbot!");
            AddBotMessage("Please enter your name to begin.");

            ChatInput2.Focus();
        }

        // Replay the voice greeting when the greeting button is clicked.
        private void button2_Click(object sender, EventArgs e)
        {
            PlayGreeting();
        }

        // Play the WAV greeting from the output directory.
        private void PlayGreeting()
        {
            string audioPath = Path.Combine(
                AppContext.BaseDirectory,
                "voice",
                "Bope_recording.wav");

            if (!File.Exists(audioPath))
            {
                MessageBox.Show(
                    "Audio file not found:\n" + audioPath +
                    "\n\nCheck that voice\\Bope_recording.wav exists " +
                    "in your project and is copied to the output directory.",
                    "Audio Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            try
            {
                player?.Stop();
                player?.Dispose();

                player = new SoundPlayer(audioPath);
                player.Load();
                player.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Could not play the audio:\n" + ex.Message,
                    "Audio Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // Display the logo in the picture box.
        private void DisplayLogo()
        {
            string imagePath = Path.Combine(
                AppContext.BaseDirectory,
                "images",
                "Bope0401.JPG");

            if (!File.Exists(imagePath))
            {
                MessageBox.Show(
                    "Image file not found:\n" + imagePath +
                    "\n\nCheck that images\\Bope0401.JPG exists " +
                    "in your project and is copied to the output directory.",
                    "Image Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            try
            {
                using Image original = Image.FromFile(imagePath);

                pictureBox1.Image?.Dispose();
                pictureBox1.Image = new Bitmap(original);
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Could not load the image:\n" + ex.Message,
                    "Image Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // Send a message when the Send button is clicked.
        private void Send_Click(object sender, EventArgs e)
        {
            SendMessage();
        }

        // Send a message when the user presses Enter.
        private void ChatInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                SendMessage();
            }
        }

        // Process the user's input and display the chatbot's response.
        private void SendMessage()
        {
            string input = ChatInput2.Text.Trim();

            // Prevent empty messages from being processed.
            if (string.IsNullOrWhiteSpace(input))
            {
                return;
            }

            // Display the user's message.
            AddUserMessage(input);

            // Get and display the chatbot's response.
            string reply = engine.GetResponse(input);
            AddBotMessage(reply);

            // Clear the input field and return focus to it.
            ChatInput2.Clear();
            ChatInput2.Focus();
        }

        // Display the user's message in the conversation area.
        private void AddUserMessage(string message)
        {
            ChatInput.AppendText(
                "You: " + message +
                Environment.NewLine + Environment.NewLine);

            ChatInput.SelectionStart = ChatInput.TextLength;
            ChatInput.ScrollToCaret();
        }

        // Display the chatbot's response and scroll to the latest message.
        private void AddBotMessage(string message)
        {
            ChatInput.AppendText(
                "Bot: " + message +
                Environment.NewLine + Environment.NewLine);

            ChatInput.SelectionStart = ChatInput.TextLength;
            ChatInput.ScrollToCaret();
        }

        // Stop the audio and release resources when the form closes.
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            player?.Stop();
            player?.Dispose();
            player = null;

            pictureBox1.Image?.Dispose();
            pictureBox1.Image = null;

            base.OnFormClosed(e);
        }

        // Leave this method only if the Designer uses this event handler.
        private void pictureBox1_Click(object sender, EventArgs e)
        {
        }

        // Leave this method only if the Designer uses this event handler.
        private void pictureBox2_Click(object sender, EventArgs e)
        {
        }
    }
}
