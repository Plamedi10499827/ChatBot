
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChatBot
{
    internal class ChatEngine
    {
        private string userName = "";
        private string favouriteTopic = "";
        private string lastTopic = "";

        private readonly Random rng = new Random();

        private readonly Dictionary<string, List<string>> keywordResponses =
            new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase)
            {
                ["password"] = new List<string>
                {
                    "Use a long, unique password for each account.",
                    "Never share your passwords with other people.",
                    "Consider using a password manager to manage strong passwords."
                },

                ["phishing"] = new List<string>
                {
                    "Check the sender and links before opening suspicious messages.",
                    "Phishing messages may imitate trusted organisations to steal your information.",
                    "Do not enter personal information on suspicious websites."
                },

                ["scam"] = new List<string>
                {
                    "Be cautious of unexpected requests for money or personal information.",
                    "Verify suspicious messages through an official contact method.",
                    "Do not let urgent messages pressure you into acting without checking."
                },

                ["privacy"] = new List<string>
                {
                    "Review your privacy settings regularly.",
                    "Think carefully before sharing personal information online.",
                    "Only give applications the permissions they need."
                },

                ["malware"] = new List<string>
                {
                    "Avoid downloading files from untrusted sources.",
                    "Keep your security software and operating system updated.",
                    "Be careful with unexpected email attachments."
                },

                ["hacking"] = new List<string>
                {
                    "Use multifactor authentication where available.",
                    "Use unique passwords and keep your devices updated.",
                    "Review your accounts for unfamiliar activity."
                },

                ["wifi"] = new List<string>
                {
                    "Be careful when using public Wi-Fi for sensitive activities.",
                    "Avoid unfamiliar Wi-Fi networks that may be impersonating legitimate ones.",
                    "Check a website's address before entering sensitive information."
                },

                ["backup"] = new List<string>
                {
                    "Back up important files regularly.",
                    "Keep backups separate from your main device when possible.",
                    "Check that your backups can be restored."
                },

                ["updates"] = new List<string>
                {
                    "Install security updates promptly.",
                    "Updates can fix weaknesses in software.",
                    "Enable automatic updates when appropriate."
                },

                ["social engineering"] = new List<string>
                {
                    "Verify someone's identity before sharing sensitive information.",
                    "Be cautious of people pretending to be technical support.",
                    "Pause and check unusual requests, even if they seem to come from someone you know."
                }
            };

        private readonly List<string> worriedWords = new List<string>
        {
            "worried", "scared", "nervous", "afraid", "anxious", "concerned"
        };

        private readonly List<string> confusedWords = new List<string>
        {
            "confused", "don't understand", "do not understand",
            "unclear", "not sure"
        };

        private readonly List<string> frustratedWords = new List<string>
        {
            "frustrated", "annoyed", "angry", "upset"
        };

        public string GetResponse(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return "Please enter a message so I can help you.";
            }

            string question = input.Trim().ToLowerInvariant();

            // Capture the user's name as their first message.
            if (string.IsNullOrWhiteSpace(userName))
            {
                userName = input.Trim();

                return $"Nice to meet you, {userName}! You can ask me about passwords, phishing, scams, privacy, malware, hacking, Wi-Fi, backups, updates, or social engineering.";
            }

            // Remember the user's favourite topic.
            if (question.Contains("my favourite topic is") ||
                question.Contains("my favorite topic is"))
            {
                string phrase = question.Contains("my favourite topic is")
                    ? "my favourite topic is"
                    : "my favorite topic is";

                int position = question.IndexOf(
                    phrase, StringComparison.OrdinalIgnoreCase);

                favouriteTopic = input.Substring(position + phrase.Length).Trim();

                if (string.IsNullOrWhiteSpace(favouriteTopic))
                {
                    favouriteTopic = "";
                    return "Please tell me your favourite topic. For example: My favourite topic is privacy.";
                }

                return $"I'll remember that your favourite topic is {favouriteTopic} during this conversation.";
            }

            // Recall the user's favourite topic.
            if (question.Contains("what is my favourite topic") ||
                question.Contains("what is my favorite topic"))
            {
                if (string.IsNullOrWhiteSpace(favouriteTopic))
                {
                    return "You haven't told me your favourite topic yet.";
                }

                return $"Your favourite topic is {favouriteTopic}.";
            }

            // Recall the user's name.
            if (question.Contains("what is my name") ||
                question.Contains("who am i"))
            {
                return $"Your name is {userName}.";
            }

            // Respond to emotional cues.
            if (ContainsAny(question, worriedWords))
            {
                return "It's understandable to feel worried about online safety. Let's take it step by step. What concerns you most?";
            }

            if (ContainsAny(question, confusedWords))
            {
                return "That's okay. Cybersecurity can be confusing. Which part would you like me to explain more simply?";
            }

            if (ContainsAny(question, frustratedWords))
            {
                return "I understand that this can feel frustrating. Let's work through it one step at a time.";
            }

            // Handle follow-up questions using the last topic.
            if (question.Contains("tell me more") ||
                question.Contains("another tip") ||
                question.Contains("explain more") ||
                question.Contains("more information"))
            {
                if (!string.IsNullOrWhiteSpace(lastTopic) &&
                    keywordResponses.TryGetValue(lastTopic, out List<string>? responses))
                {
                    return $"Here is another tip about {lastTopic}: {responses[rng.Next(responses.Count)]}";
                }

                return "Which cybersecurity topic would you like to learn more about?";
            }

            // Handle greetings as complete words to avoid matching
            // words such as "phishing" by mistake.
            if (question == "hi" || question.StartsWith("hi ") ||
                question == "hello" || question.StartsWith("hello ") ||
                question == "hey" || question.StartsWith("hey "))
            {
                return $"Hello, {userName}! What would you like to learn about online safety?";
            }

            if (question.Contains("thank"))
            {
                return "You're welcome! Stay alert and stay safe online.";
            }

            if (question.Contains("help"))
            {
                return "You can ask me about passwords, phishing, scams, privacy, malware, hacking, Wi-Fi, backups, updates, or social engineering. You can also ask for another tip.";
            }

            // Find a cybersecurity keyword and choose a random response.
            foreach (KeyValuePair<string, List<string>> topic in keywordResponses)
            {
                if (question.Contains(topic.Key))
                {
                    lastTopic = topic.Key;
                    return topic.Value[rng.Next(topic.Value.Count)];
                }
            }

            // Fallback for messages the chatbot does not recognise.
            return "I'm not sure I understood that. Try asking about passwords, phishing, scams, privacy, malware, hacking, Wi-Fi, backups, updates, or social engineering.";
        }

        private bool ContainsAny(string input, List<string> words)
        {
            return words.Any(word =>
                input.Contains(word, StringComparison.OrdinalIgnoreCase));
        }
    }
}
