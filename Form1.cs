
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

        // Replay the voice greeting when the audio button is clicked.
        private void button2_Click(object sender, EventArgs e)
        {
            PlayGreeting();
        }

        // No action is required when the first picture box is clicked.
        private void pictureBox1_Click(object sender, EventArgs e)
        {
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
                    "Audio file not found:\n" + audioPath,
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
                    "Image file not found:\n" + imagePath,
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

            // Do not process empty messages.
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

        // Release audio and image resources when the form closes.
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            player?.Stop();
            player?.Dispose();

            pictureBox1.Image?.Dispose();

            base.OnFormClosed(e);
        }

        // No action is required when the second picture box is clicked.
        private void pictureBox2_Click(object sender, EventArgs e)
        {
        }
    }
}
