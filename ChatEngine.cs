using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ChatBot
{
    internal class ChatEngine
    {
        private string userName = "";
        private string favouriteTopic = "";
        private string lastTopic = "";

        private readonly Random rng = new Random();

        // Store cybersecurity topics and their possible responses.
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
                    "Avoid unfamiliar Wi-Fi networks that may impersonate legitimate ones.",
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

        // Words and phrases that indicate the user's emotional state.
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

        // Process the user's message and return an appropriate response.
        public string GetResponse(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return "Please enter a message so I can help you.";
            }

            string question = NormaliseInput(input);

            // Capture the user's name as their first message.
            if (string.IsNullOrWhiteSpace(userName))
            {
                userName = input.Trim();

                return $"Nice to meet you, {userName}! " +
                       "You can ask me about passwords, phishing, scams, privacy, " +
                       "malware, hacking, Wi-Fi, backups, updates, or social engineering.";
            }

            // Recall the user's name.
            if (ContainsPhrase(question, "what is my name") ||
                ContainsPhrase(question, "who am i"))
            {
                return $"Your name is {userName}.";
            }

            // Save the user's favourite cybersecurity topic.
            if (ContainsPhrase(question, "my favourite topic is") ||
                ContainsPhrase(question, "my favorite topic is"))
            {
                string topic = ExtractFavouriteTopic(input);

                if (string.IsNullOrWhiteSpace(topic))
                {
                    return "Please tell me your favourite topic. For example: " +
                           "My favourite topic is privacy.";
                }

                string? matchedTopic = FindTopic(topic);

                if (matchedTopic == null)
                {
                    return "I don't recognise that cybersecurity topic. " +
                           "Please choose passwords, phishing, scams, privacy, malware, " +
                           "hacking, Wi-Fi, backups, updates, or social engineering.";
                }

                favouriteTopic = matchedTopic;

                return $"I'll remember that your favourite topic is {favouriteTopic} " +
                       "during this conversation.";
            }

            // Recall the user's favourite topic.
            if (ContainsPhrase(question, "what is my favourite topic") ||
                ContainsPhrase(question, "what is my favorite topic"))
            {
                if (string.IsNullOrWhiteSpace(favouriteTopic))
                {
                    return "You haven't told me your favourite topic yet.";
                }

                return $"Your favourite topic is {favouriteTopic}.";
            }

            // Recognise greetings.
            if (question == "hi" || question.StartsWith("hi ") ||
                question == "hello" || question.StartsWith("hello ") ||
                question == "hey" || question.StartsWith("hey "))
            {
                return $"Hello, {userName}! What would you like to learn about online safety?";
            }

            // Respond to expressions of gratitude.
            if (ContainsPhrase(question, "thank") ||
                ContainsPhrase(question, "thanks"))
            {
                return "You're welcome! Stay alert and stay safe online.";
            }

            // Provide help with supported topics.
            if (ContainsPhrase(question, "help") ||
                ContainsPhrase(question, "what can you do"))
            {
                return "You can ask me about passwords, phishing, scams, privacy, " +
                       "malware, hacking, Wi-Fi, backups, updates, or social engineering. " +
                       "You can also ask for another tip.";
            }

            // Detect worried, confused, or frustrated messages.
            if (ContainsAny(question, worriedWords))
            {
                if (ContainsPhrase(question, "not worried") ||
                    ContainsPhrase(question, "not scared") ||
                    ContainsPhrase(question, "not afraid"))
                {
                    // Do not interpret these expressions as negative sentiment.
                }
                else
                {
                    return "It's understandable to feel worried about online safety. " +
                           "Let's take it step by step. What concerns you most?";
                }
            }

            if (ContainsAny(question, confusedWords))
            {
                if (!ContainsPhrase(question, "not confused") &&
                    !ContainsPhrase(question, "not sure"))
                {
                    return "That's okay. Cybersecurity can be confusing. " +
                           "Which part would you like me to explain more simply?";
                }
            }

            if (ContainsAny(question, frustratedWords))
            {
                if (!ContainsPhrase(question, "not angry") &&
                    !ContainsPhrase(question, "not upset") &&
                    !ContainsPhrase(question, "not frustrated"))
                {
                    return "I understand that this can feel frustrating. " +
                           "Let's work through it one step at a time.";
                }
            }

            // Answer follow-up requests using the previously discussed topic.
            if (ContainsPhrase(question, "tell me more") ||
                ContainsPhrase(question, "another tip") ||
                ContainsPhrase(question, "explain more") ||
                ContainsPhrase(question, "more information"))
            {
                if (!string.IsNullOrWhiteSpace(lastTopic) &&
                    keywordResponses.TryGetValue(lastTopic, out List<string>? responses))
                {
                    return $"Here is another tip about {lastTopic}: " +
                           responses[rng.Next(responses.Count)];
                }

                return "Which cybersecurity topic would you like to learn more about?";
            }

            // Find a supported cybersecurity topic.
            string? detectedTopic = FindTopic(question);

            if (detectedTopic != null)
            {
                lastTopic = detectedTopic;

                List<string> responses = keywordResponses[detectedTopic];

                return responses[rng.Next(responses.Count)];
            }

            // Provide a response when no topic is recognised.
            return "I'm not sure I understood that. Try asking about passwords, phishing, " +
                   "scams, privacy, malware, hacking, Wi-Fi, backups, updates, " +
                   "or social engineering.";
        }

        // Standardise punctuation, spacing, and common Wi-Fi variations.
        private string NormaliseInput(string input)
        {
            string normalised = input.Trim().ToLowerInvariant();

            normalised = Regex.Replace(normalised, @"\bwi[\s-]*fi\b", "wifi");

            normalised = Regex.Replace(normalised, @"[^\w\s']", " ");

            normalised = Regex.Replace(normalised, @"\s+", " ");

            return normalised.Trim();
        }

        // Find a supported topic, including common alternative terms.
        private string? FindTopic(string input)
        {
            string question = NormaliseInput(input);

            foreach (KeyValuePair<string, List<string>> topic in keywordResponses)
            {
                if (ContainsPhrase(question, topic.Key))
                {
                    return topic.Key;
                }
            }

            // Recognise common alternative words for supported topics.
            if (ContainsPhrase(question, "passwords"))
                return "password";

            if (ContainsPhrase(question, "phishing email") ||
                ContainsPhrase(question, "fake email"))
                return "phishing";

            if (ContainsPhrase(question, "scams"))
                return "scam";

            if (ContainsPhrase(question, "malicious software") ||
                ContainsPhrase(question, "virus"))
                return "malware";

            if (ContainsPhrase(question, "wireless network") ||
                ContainsPhrase(question, "public wifi"))
                return "wifi";

            if (ContainsPhrase(question, "backups") ||
                ContainsPhrase(question, "back up"))
                return "backup";

            if (ContainsPhrase(question, "software update") ||
                ContainsPhrase(question, "security update"))
                return "updates";

            if (ContainsPhrase(question, "social engineering attack"))
                return "social engineering";

            return null;
        }

        // Extract the favourite topic from the user's sentence.
        private string ExtractFavouriteTopic(string input)
        {
            string normalised = input.Trim();

            string[] phrases =
            {
                "my favourite topic is",
                "my favorite topic is"
            };

            foreach (string phrase in phrases)
            {
                int position = normalised.IndexOf(
                    phrase, StringComparison.OrdinalIgnoreCase);

                if (position >= 0)
                {
                    return normalised.Substring(position + phrase.Length).Trim(
                        ' ', '.', '!', '?', ',');
                }
            }

            return "";
        }

        // Match complete words or phrases rather than partial words.
        private bool ContainsPhrase(string input, string phrase)
        {
            string normalisedInput = NormaliseInput(input);
            string normalisedPhrase = NormaliseInput(phrase);

            string pattern = @"(?<!\w)" +
                             Regex.Escape(normalisedPhrase) +
                             @"(?!\w)";

            return Regex.IsMatch(
                normalisedInput,
                pattern,
                RegexOptions.IgnoreCase);
        }

        // Check whether the message contains any of the specified words.
        private bool ContainsAny(string input, List<string> words)
        {
            return words.Any(word => ContainsPhrase(input, word));
        }
    }
}
