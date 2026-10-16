
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

        // Track the conversation stage.
        // 0 = ask for name, 1 = ask how the user feels, 2 = normal chat.
        private int conversationStage = 0;

        // Track whether Bope has asked about continuing the conversation.
        private bool awaitingAnotherTopic = false;

        private readonly Random rng = new Random();

        // Store detailed explanations for different cybersecurity topics.
        private readonly Dictionary<string, List<string>> keywordResponses =
            new Dictionary<string, List<string>>(
                StringComparer.OrdinalIgnoreCase)
            {
                ["password"] = new List<string>
                {
                    "PASSWORD SECURITY\n\n" +
                    "Passwords protect your online accounts, including " +
                    "email, social media and online banking. Weak or reused " +
                    "passwords can put your accounts at risk.\n\n" +
                    "To stay safe, use long, unique passwords for different " +
                    "accounts. Avoid easily guessed personal information, " +
                    "never share your passwords, and consider using a " +
                    "reputable password manager. Enable multifactor " +
                    "authentication whenever possible.",

                    "CREATING STRONG PASSWORDS\n\n" +
                    "A strong password should be difficult for other people " +
                    "to guess. Consider using a long passphrase made from " +
                    "several unrelated words. Use a different password " +
                    "for every account.\n\n" +
                    "Avoid common passwords, names, birthdays and predictable " +
                    "sequences. Multifactor authentication adds another " +
                    "layer of protection if your password is compromised.",

                    "PROTECTING YOUR PASSWORDS\n\n" +
                    "Never send your password to someone who requests it " +
                    "in an unexpected message. Be cautious of fake login " +
                    "pages that imitate legitimate websites.\n\n" +
                    "Check website addresses before signing in. If you " +
                    "believe your password has been compromised, change it " +
                    "through the official website and review your account's " +
                    "security settings."
                },

                ["phishing"] = new List<string>
                {
                    "UNDERSTANDING PHISHING\n\n" +
                    "Phishing is a scam in which someone pretends to be a " +
                    "trusted person or organisation to trick you into " +
                    "revealing private information or visiting a harmful " +
                    "website.\n\n" +
                    "Messages may claim that your account has a problem, " +
                    "that you have won a prize, or that urgent action is " +
                    "required. Check the sender, avoid suspicious links, " +
                    "and verify requests through an official contact method.",

                    "HOW TO RECOGNISE PHISHING\n\n" +
                    "Warning signs include unexpected attachments, unusual " +
                    "sender addresses, suspicious links, requests for " +
                    "passwords, and pressure to act immediately.\n\n" +
                    "Some phishing messages look professional. Do not rely " +
                    "on spelling mistakes alone. Check the website address " +
                    "and contact the organisation independently if you are unsure.",

                    "WHAT TO DO ABOUT PHISHING\n\n" +
                    "Do not reply to suspicious messages or enter personal " +
                    "information through their links. Report suspicious " +
                    "messages using your email provider's reporting tools " +
                    "where available.\n\n" +
                    "If you already entered your password on a suspicious " +
                    "site, change it through the legitimate website and " +
                    "review your account security."
                },

                ["scam"] = new List<string>
                {
                    "UNDERSTANDING ONLINE SCAMS\n\n" +
                    "Online scams attempt to trick people into giving away " +
                    "money, personal information or access to their accounts. " +
                    "Scammers may pretend to be businesses, friends, " +
                    "employers or officials.\n\n" +
                    "Be suspicious of unexpected payment requests, offers " +
                    "that sound too good to be true, and messages demanding " +
                    "immediate action. Verify the person's identity before responding.",

                    "HOW TO AVOID SCAMS\n\n" +
                    "Pause before sending money or sharing personal " +
                    "information. Research unfamiliar organisations and " +
                    "independently verify offers or requests.\n\n" +
                    "Do not trust a message just because it contains a " +
                    "familiar logo or someone's name. Contact the person " +
                    "or organisation using a trusted phone number or " +
                    "official website.",

                    "RECOGNISING SUSPICIOUS REQUESTS\n\n" +
                    "Scammers may create fear, excitement or urgency to stop " +
                    "you from thinking carefully. They may ask for gift " +
                    "cards, payments, verification codes or confidential information.\n\n" +
                    "Take time to check the facts. Never share a one-time " +
                    "security code with someone who contacts you unexpectedly."
                },

                ["privacy"] = new List<string>
                {
                    "ONLINE PRIVACY\n\n" +
                    "Online privacy means having control over how your " +
                    "personal information is shared and used. Your location, " +
                    "phone number, photographs and account details can " +
                    "reveal a lot about you.\n\n" +
                    "Review your privacy settings, limit what you share " +
                    "publicly, and check application permissions. Only " +
                    "provide personal information when it is genuinely necessary.",

                    "PROTECTING PERSONAL INFORMATION\n\n" +
                    "Think carefully before posting photographs, travel " +
                    "plans, your address or other identifying information " +
                    "online. Information shared publicly may be copied or redistributed.\n\n" +
                    "Use private account settings where appropriate, remove " +
                    "unnecessary application permissions, and avoid sharing " +
                    "sensitive information with unverified people or websites.",

                    "PRIVACY SETTINGS AND PERMISSIONS\n\n" +
                    "Applications may request access to your camera, " +
                    "microphone, contacts or location. Review these requests " +
                    "and allow only the permissions needed for the application's purpose.\n\n" +
                    "Regularly check who can view your posts and profile. " +
                    "Privacy settings can reduce exposure, but they cannot " +
                    "guarantee that information will never be shared."
                },

                ["malware"] = new List<string>
                {
                    "UNDERSTANDING MALWARE\n\n" +
                    "Malware is software designed to harm devices, disrupt " +
                    "operations, spy on activity or gain unauthorised access. " +
                    "Examples include viruses, spyware and ransomware.\n\n" +
                    "Malware can arrive through suspicious downloads, unsafe " +
                    "attachments or compromised websites. Keep software " +
                    "updated, use reputable security tools, and download " +
                    "files from trusted sources.",

                    "PREVENTING MALWARE\n\n" +
                    "Avoid unexpected email attachments and untrusted " +
                    "downloads. Keep your operating system, browser and " +
                    "security software updated to reduce exposure to " +
                    "known vulnerabilities.\n\n" +
                    "Be cautious when applications request unusual " +
                    "permissions. Back up important files regularly so " +
                    "you have copies if your data becomes unavailable.",

                    "WHAT TO DO IF YOU SUSPECT MALWARE\n\n" +
                    "If your device behaves suspiciously, avoid entering " +
                    "sensitive information on it. Use trusted security " +
                    "software and seek help from a trusted technical support professional.\n\n" +
                    "From a separate trusted device, secure important " +
                    "accounts if necessary. Keep reliable backups and " +
                    "follow your organisation's security procedures if applicable."
                },

                ["hacking"] = new List<string>
                {
                    "UNDERSTANDING ACCOUNT HACKING\n\n" +
                    "Unauthorised access can happen when attackers obtain " +
                    "stolen passwords, exploit software weaknesses or trick " +
                    "people into revealing security information.\n\n" +
                    "Protect your accounts with unique passwords and " +
                    "multifactor authentication. Keep devices updated and " +
                    "review account activity for unfamiliar sign-ins.",

                    "PROTECTING YOUR ACCOUNTS\n\n" +
                    "Use multifactor authentication wherever available. " +
                    "It adds a verification step beyond your password and " +
                    "can make unauthorised access more difficult.\n\n" +
                    "Never share verification codes, avoid suspicious login " +
                    "links, and use official websites to access accounts. " +
                    "Review your recovery information regularly.",

                    "RECOGNISING POSSIBLE ACCOUNT COMPROMISE\n\n" +
                    "Unexpected password-reset messages, unfamiliar sign-ins, " +
                    "or account changes you did not make may indicate a " +
                    "security problem.\n\n" +
                    "If you suspect unauthorised access, change your password " +
                    "from a trusted device, review active sessions, and " +
                    "enable multifactor authentication."
                },

                ["wifi"] = new List<string>
                {
                    "WI-FI SECURITY\n\n" +
                    "Wi-Fi connects your devices to a network. Unfamiliar " +
                    "or poorly secured networks can expose you to privacy " +
                    "and security risks.\n\n" +
                    "Use trusted networks when possible. When using public " +
                    "Wi-Fi, check the network name with the venue, use " +
                    "secure websites, and do not ignore browser security warnings.",

                    "STAYING SAFE ON PUBLIC WI-FI\n\n" +
                    "Public Wi-Fi is convenient, but you should not assume " +
                    "every network is genuine or properly secured.\n\n" +
                    "Confirm the correct network name, keep your device " +
                    "updated, disable automatic connections to unfamiliar " +
                    "networks, and use multifactor authentication on important accounts."
                },

                ["backup"] = new List<string>
                {
                    "WHY BACKUPS MATTER\n\n" +
                    "Backups are extra copies of important files. They can " +
                    "help you recover documents, photographs and other " +
                    "information after accidental deletion, device failure " +
                    "or certain cybersecurity incidents.\n\n" +
                    "Back up files regularly, protect backup copies, and " +
                    "test whether you can restore your information successfully.",

                    "CREATING A SAFER BACKUP PLAN\n\n" +
                    "Keep more than one copy of important information when " +
                    "practical. Consider a separate external backup or a " +
                    "trusted cloud backup service.\n\n" +
                    "Protect backups with appropriate access controls. Keep " +
                    "at least one backup separate from your everyday device " +
                    "so a problem affecting the device does not affect every copy."
                },

                ["updates"] = new List<string>
                {
                    "WHY SOFTWARE UPDATES MATTER\n\n" +
                    "Software updates can fix security weaknesses, correct " +
                    "bugs and improve reliability. Delaying important " +
                    "security updates may leave devices exposed to known problems.\n\n" +
                    "Install updates from official sources, keep your " +
                    "operating system and applications current, and enable " +
                    "automatic updates when appropriate.",

                    "UPDATING YOUR DEVICES SAFELY\n\n" +
                    "Check for updates through your device's built-in " +
                    "settings or the software provider's official website. " +
                    "Avoid unexpected pop-ups asking you to install unknown programs.\n\n" +
                    "Restart your device when required to finish installing " +
                    "updates, and remove applications you no longer need."
                },

                ["social engineering"] = new List<string>
                {
                    "UNDERSTANDING SOCIAL ENGINEERING\n\n" +
                    "Social engineering involves manipulating people into " +
                    "revealing confidential information, transferring money " +
                    "or performing unsafe actions.\n\n" +
                    "An attacker might pretend to be technical support, " +
                    "a manager or someone you know. Verify unusual requests " +
                    "independently and do not let pressure replace careful checking.",

                    "HOW TO RECOGNISE MANIPULATION\n\n" +
                    "Be careful when someone unexpectedly asks for " +
                    "passwords, security codes, private documents or urgent payments.\n\n" +
                    "Even if a request appears to come from a familiar " +
                    "person, confirm it through another trusted channel. " +
                    "Legitimate-looking messages and caller names can sometimes be imitated."
                }
            };

        // Words that may indicate the user's emotional state.
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

        // Process the user's message and return Bope's response.
        public string GetResponse(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return "Please enter a message so I can help you.";
            }

            string question = NormaliseInput(input);

            // Ask for the user's name first.
            if (string.IsNullOrWhiteSpace(userName))
            {
                userName = input.Trim();
                conversationStage = 1;

                return $"Nice to meet you, {userName}! " +
                       "How are you feeling about cybersecurity today? " +
                       "You can tell me if you feel confident, worried, confused, " +
                       "or if there is something specific you want to learn.";
            }

            // Ask about feelings after collecting the user's name.
            if (conversationStage == 1)
            {
                conversationStage = 2;

                string? topicDuringIntroduction = FindTopic(question);

                if (topicDuringIntroduction != null)
                {
                    return GiveTopicResponse(topicDuringIntroduction);
                }

                if (ContainsAny(question, worriedWords))
                {
                    return "It's understandable to feel worried about online safety, " +
                           userName + ". I'll help you step by step.\n\n" +
                           "You can ask me about passwords, phishing, scams, privacy, " +
                           "malware, hacking, Wi-Fi, backups, software updates, " +
                           "or social engineering. Which would you like to explore?";
                }

                if (ContainsAny(question, confusedWords))
                {
                    return "That's okay, " + userName + "! Cybersecurity can be confusing, " +
                           "but I'll explain things in simple language.\n\n" +
                           "You can ask me about any of my cybersecurity topics, " +
                           "or describe a question in your own words.";
                }

                if (ContainsAny(question, frustratedWords))
                {
                    return "I understand, " + userName + ". We can take things one step " +
                           "at a time.\n\nWhich cybersecurity question would you like " +
                           "to start with?";
                }

                return "Thanks for sharing, " + userName + "!\n\n" +
                       "What would you like to learn about today? You can ask me " +
                       "about passwords, phishing, scams, privacy, malware, hacking, " +
                       "Wi-Fi, backups, updates, social engineering, or another " +
                       "cybersecurity question.";
            }

            // Handle the user's answer to the offer of another topic.
            if (awaitingAnotherTopic)
            {
                if (IsAffirmative(question))
                {
                    awaitingAnotherTopic = false;

                    return "Great, " + userName + "! What would you like to explore?\n\n" +
                           "You can ask about passwords, phishing, scams, privacy, " +
                           "malware, hacking, Wi-Fi, backups, updates, or social engineering. " +
                           "You can also type your question in your own words.";
                }

                if (IsNegative(question))
                {
                    awaitingAnotherTopic = false;

                    return "No problem, " + userName + "! Thanks for chatting with me. " +
                           "Remember to stay alert, protect your personal information, " +
                           "and stay safe online. You can ask me another question anytime.";
                }

                // If the user asks a real question instead of answering yes or no,
                // continue processing it normally.
                awaitingAnotherTopic = false;
            }

            // Recall the user's name.
            if (ContainsPhrase(question, "what is my name") ||
                ContainsPhrase(question, "who am i"))
            {
                return "Your name is " + userName + ".";
            }

            // Save the user's favourite topic.
            if (ContainsPhrase(question, "my favourite topic is") ||
                ContainsPhrase(question, "my favorite topic is"))
            {
                string topic = ExtractFavouriteTopic(input);
                string? matchedTopic = FindTopic(topic);

                if (matchedTopic == null)
                {
                    return "I couldn't match that to a topic I know. " +
                           "Try passwords, phishing, scams, privacy, malware, hacking, " +
                           "Wi-Fi, backups, updates, or social engineering.";
                }

                favouriteTopic = matchedTopic;

                return "I'll remember that your favourite topic is " +
                       favouriteTopic + " during this conversation.\n\n" +
                       "Would you like to learn more about it or explore another topic?";
            }

            // Recall the user's favourite topic.
            if (ContainsPhrase(question, "what is my favourite topic") ||
                ContainsPhrase(question, "what is my favorite topic"))
            {
                if (string.IsNullOrWhiteSpace(favouriteTopic))
                {
                    return "You haven't told me your favourite topic yet. " +
                           "For example, you can say: My favourite topic is privacy.";
                }

                return "Your favourite topic is " + favouriteTopic + ".";
            }

            // Recognise greetings.
            if (question == "hi" || question.StartsWith("hi ") ||
                question == "hello" || question.StartsWith("hello ") ||
                question == "hey" || question.StartsWith("hey"))
            {
                return "Hi again, " + userName + "! What would you like to learn about today?";
            }

            // Respond to gratitude.
            if (ContainsPhrase(question, "thanks") ||
                ContainsPhrase(question, "thank you") ||
                question == "thank")
            {
                return "You're welcome, " + userName +
                       "! Stay alert and stay safe online.";
            }

            // Provide a list of capabilities when asked for help.
            if (ContainsPhrase(question, "help") ||
                ContainsPhrase(question, "what can you do") ||
                ContainsPhrase(question, "what topics do you know"))
            {
                return "I'm Bope, your Cybersecurity Awareness Chatbot!\n\n" +
                       "I can explain password security, phishing, online scams, " +
                       "privacy, malware, hacking, Wi-Fi safety, backups, software " +
                       "updates and social engineering. I can provide more information, " +
                       "give another tip, and help you recognise common online risks.\n\n" +
                       "Ask me about any topic or type a question in your own words.";
            }

            // Answer requests for more information about the last topic.
            if (ContainsPhrase(question, "tell me more") ||
                ContainsPhrase(question, "another tip") ||
                ContainsPhrase(question, "explain more") ||
                ContainsPhrase(question, "more information") ||
                ContainsPhrase(question, "explain in more detail"))
            {
                if (!string.IsNullOrWhiteSpace(lastTopic))
                {
                    return GiveTopicResponse(lastTopic);
                }

                return "Of course! What would you like to learn about? " +
                       "You can ask me a question about any cybersecurity topic.";
            }

            // Allow users to request a simpler explanation.
            if (ContainsPhrase(question, "explain simply") ||
                ContainsPhrase(question, "explain in simple words") ||
                ContainsPhrase(question, "explain that simply"))
            {
                if (!string.IsNullOrWhiteSpace(lastTopic))
                {
                    return GiveSimpleExplanation(lastTopic);
                }

                return "Of course! Tell me which cybersecurity topic you'd like " +
                       "me to explain in simple words.";
            }

            // Detect emotional messages during the normal conversation.
            if (ContainsAny(question, worriedWords) &&
                !ContainsPhrase(question, "not worried") &&
                !ContainsPhrase(question, "not scared") &&
                !ContainsPhrase(question, "not afraid"))
            {
                return "It's understandable to feel worried about online safety, " +
                       userName + ". Let's take it step by step. " +
                       "Tell me what concerns you, and I'll try to explain how to stay safe.";
            }

            if (ContainsAny(question, confusedWords) &&
                !ContainsPhrase(question, "not confused"))
            {
                return "That's okay, " + userName + "! Cybersecurity can be confusing. " +
                       "I can explain a topic in simpler words or give you an example. " +
                       "What would you like help understanding?";
            }

            if (ContainsAny(question, frustratedWords))
            {
                return "I understand that this can feel frustrating, " + userName +
                       ". Let's work through it one step at a time. " +
                       "Which part would you like help with?";
            }

            // Find a supported topic in the user's question.
            string? detectedTopic = FindTopic(question);

            if (detectedTopic != null)
            {
                return GiveTopicResponse(detectedTopic);
            }

            // Handle questions that are not recognised by the chatbot.
            return "That's an interesting question, " + userName + ". " +
                   "I don't have a specific answer for that yet, but I can explain " +
                   "password security, phishing, scams, privacy, malware, hacking, " +
                   "Wi-Fi, backups, updates, and social engineering.\n\n" +
                   "You can also ask me to explain a topic in more detail.";
        }

        // Choose a detailed explanation and invite the user to continue.
        private string GiveTopicResponse(string topic)
        {
            if (!keywordResponses.TryGetValue(topic, out List<string>? responses))
            {
                return "I couldn't find information about that topic.";
            }

            lastTopic = topic;

            string response = responses[rng.Next(responses.Count)];

            awaitingAnotherTopic = true;

            return response +
                   "\n\nWould you like to learn about anything else, " +
                   userName + "? You can ask about another cybersecurity topic, " +
                   "request another tip, or ask me to explain this topic in more detail.";
        }

        // Give a short, simple explanation of the last or requested topic.
        private string GiveSimpleExplanation(string topic)
        {
            string explanation;

            switch (topic)
            {
                case "password":
                    explanation = "A password is like a key to your online account. " +
                                  "Use a long, different password for every account " +
                                  "and never share it.";
                    break;

                case "phishing":
                    explanation = "Phishing is when someone pretends to be trustworthy " +
                                  "to trick you into clicking a link or sharing private information.";
                    break;

                case "scam":
                    explanation = "A scam is a trick designed to take your money, " +
                                  "personal information or access to your accounts.";
                    break;

                case "privacy":
                    explanation = "Online privacy means being careful about who can " +
                                  "see your personal information and what you share online.";
                    break;

                case "malware":
                    explanation = "Malware is harmful software that can damage a device, " +
                                  "spy on activity or access information without permission.";
                    break;

                case "hacking":
                    explanation = "Unauthorised hacking means accessing a device or " +
                                  "account without permission. Strong passwords and " +
                                  "multifactor authentication help protect accounts.";
                    break;

                case "wifi":
                    explanation = "Wi-Fi connects your device to a network. Check that " +
                                  "you are connecting to a trusted network, especially in public places.";
                    break;

                case "backup":
                    explanation = "A backup is an extra copy of your files. It helps " +
                                  "you recover information if your original files are lost.";
                    break;

                case "updates":
                    explanation = "Software updates improve programs and often fix " +
                                  "security weaknesses. Install them from official sources.";
                    break;

                case "social engineering":
                    explanation = "Social engineering is when someone manipulates " +
                                  "another person into sharing information or doing something unsafe.";
                    break;

                default:
                    explanation = "I can explain this topic in more detail if you tell me " +
                                  "which part you would like to understand.";
                    break;
            }

            awaitingAnotherTopic = true;

            return explanation +
                   "\n\nWould you like to learn about anything else, " +
                   userName + "?";
        }

        // Recognise common affirmative answers.
        private bool IsAffirmative(string question)
        {
            return question == "yes" ||
                   question == "yes please" ||
                   question == "yeah" ||
                   question == "yep" ||
                   question == "sure" ||
                   question == "sure thing" ||
                   question == "of course" ||
                   question == "please" ||
                   question == "another topic" ||
                   question == "something else";
        }

        // Recognise common negative answers.
        private bool IsNegative(string question)
        {
            return question == "no" ||
                   question == "no thanks" ||
                   question == "no thank you" ||
                   question == "nope" ||
                   question == "not now" ||
                   question == "nothing else" ||
                   question == "that's all" ||
                   question == "that is all";
        }

        // Standardise punctuation, spacing, and common Wi-Fi variations.
        private string NormaliseInput(string input)
        {
            string normalised = input.Trim().ToLowerInvariant();

            normalised = Regex.Replace(
                normalised, @"\bwi[\s-]*fi\b", "wifi");

            normalised = Regex.Replace(
                normalised, @"[^\w\s']", " ");

            normalised = Regex.Replace(
                normalised, @"\s+", " ");

            return normalised.Trim();
        }

        // Find a supported topic, including alternative phrases.
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

            // Password-related alternatives.
            if (ContainsPhrase(question, "passwords") ||
                ContainsPhrase(question, "passphrase") ||
                ContainsPhrase(question, "account security") ||
                ContainsPhrase(question, "strong password") ||
                ContainsPhrase(question, "keep my accounts safe"))
                return "password";

            // Phishing-related alternatives.
            if (ContainsPhrase(question, "phishing email") ||
                ContainsPhrase(question, "fake email") ||
                ContainsPhrase(question, "fake message") ||
                ContainsPhrase(question, "suspicious email") ||
                ContainsPhrase(question, "fake link"))
                return "phishing";

            // Scam-related alternatives.
            if (ContainsPhrase(question, "scams") ||
                ContainsPhrase(question, "fraud") ||
                ContainsPhrase(question, "online fraud"))
                return "scam";

            // Privacy-related alternatives.
            if (ContainsPhrase(question, "protect my information") ||
                ContainsPhrase(question, "personal information") ||
                ContainsPhrase(question, "online privacy") ||
                ContainsPhrase(question, "private information"))
                return "privacy";

            // Malware-related alternatives.
            if (ContainsPhrase(question, "malicious software") ||
                ContainsPhrase(question, "virus") ||
                ContainsPhrase(question, "computer virus") ||
                ContainsPhrase(question, "ransomware") ||
                ContainsPhrase(question, "spyware"))
                return "malware";

            // Wi-Fi-related alternatives.
            if (ContainsPhrase(question, "wireless network") ||
                ContainsPhrase(question, "public wifi") ||
                ContainsPhrase(question, "public network"))
                return "wifi";

            // Backup-related alternatives.
            if (ContainsPhrase(question, "backups") ||
                ContainsPhrase(question, "back up") ||
                ContainsPhrase(question, "save my files") ||
                ContainsPhrase(question, "recover my files"))
                return "backup";

            // Software-update alternatives.
            if (ContainsPhrase(question, "software update") ||
                ContainsPhrase(question, "security update") ||
                ContainsPhrase(question, "update my computer"))
                return "updates";

            // Social engineering alternatives.
            if (ContainsPhrase(question, "social engineering attack") ||
                ContainsPhrase(question, "trick me") ||
                ContainsPhrase(question, "pretending to be someone"))
                return "social engineering";

            // Recognise general questions about account protection.
            if (ContainsPhrase(question, "protect my account") ||
                ContainsPhrase(question, "protect my accounts"))
                return "hacking";

            return null;
        }

        // Extract the favourite topic from the user's sentence.
        private string ExtractFavouriteTopic(string input)
        {
            string[] phrases =
            {
                "my favourite topic is",
                "my favorite topic is"
            };

            foreach (string phrase in phrases)
            {
                int position = input.IndexOf(
                    phrase, StringComparison.OrdinalIgnoreCase);

                if (position >= 0)
                {
                    return input.Substring(position + phrase.Length)
                        .Trim(' ', '.', '!', '?', ',');
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

        // Check whether the message contains any specified words.
        private bool ContainsAny(string input, List<string> words)
        {
            return words.Any(word => ContainsPhrase(input, word));
        }
    }
}
