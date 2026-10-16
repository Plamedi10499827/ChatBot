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

        // Load the logo, play the greeting, and start the conversation.
        private void Form1_Load(object sender, EventArgs e)
        {
            DisplayLogo();
            PlayGreeting();

            AddBotMessage(
                "Hi! I'm Bope, your Cybersecurity Awareness Chatbot.");

            AddBotMessage("What's your name?");

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

        // Process the user's input and display Bope's response.
        private void SendMessage()
        {
            string input = ChatInput2.Text.Trim();

            if (string.IsNullOrWhiteSpace(input))
            {
                AddBotMessage("Please type a message first.");
                ChatInput2.Focus();
                return;
            }

            AddUserMessage(input);

            string reply = engine.GetResponse(input);
            AddBotMessage(reply);

            ChatInput2.Clear();
            ChatInput2.Focus();
        }

        // Display the user's message in blue.
        private void AddUserMessage(string message)
        {
            ChatInput.SelectionStart = ChatInput.TextLength;
            ChatInput.SelectionLength = 0;
            ChatInput.SelectionColor = Color.DodgerBlue;

            ChatInput.AppendText(
                "You: " + message +
                Environment.NewLine + Environment.NewLine);

            ChatInput.SelectionColor = ChatInput.ForeColor;
            ChatInput.SelectionStart = ChatInput.TextLength;
            ChatInput.ScrollToCaret();
        }

        // Display Bope's response in green.
        private void AddBotMessage(string message)
        {
            ChatInput.SelectionStart = ChatInput.TextLength;
            ChatInput.SelectionLength = 0;
            ChatInput.SelectionColor = Color.ForestGreen;

            ChatInput.AppendText(
                "Bope: " + message +
                Environment.NewLine + Environment.NewLine);

            ChatInput.SelectionColor = ChatInput.ForeColor;
            ChatInput.SelectionStart = ChatInput.TextLength;
            ChatInput.ScrollToCaret();
        }

        // Release audio and image resources when the form closes.
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            player?.Stop();
            player?.Dispose();
            player = null;

            pictureBox1.Image?.Dispose();
            pictureBox1.Image = null;

            base.OnFormClosed(e);
        }

        // Keep these methods if the Designer uses these event handlers.
        private void pictureBox1_Click(object sender, EventArgs e)
        {
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
        }

        private void ChatInput2_TextChanged(object sender, EventArgs e)
        {
        }
    }
}
