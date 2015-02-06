﻿using System;
using System.Collections.Generic;

namespace CSharpNote.Data.CSharpPracticeMethod.SubClass
{
    public class ChatPipeLine
    {
        ChatSomething root;
        ChatSomething curent;

        public void Collect(Boolean hello, Boolean niceToMeetYou)
        {
            foreach (var say in GenerateChatContent(hello, niceToMeetYou))
            {
                if (root == null)
                {
                    root = say;
                    curent = say;
                }
                else
                {
                    curent.Next = say;
                    curent = say;
                }
            }
        }

        public void ReleaseAll()
        {
            Release(root);
        }

        public void Release(ChatSomething current)
        {
            current.Speak("Kewos");
            if (current.Next != null) Release(current.Next);
        }

        public IEnumerable<ChatSomething> GenerateChatContent(Boolean hello, Boolean niceToMeetYou)
        {
            yield return new SayHello();
            yield return new NiceToMeetYou();
        }
    }

    public interface ChatSomething
    {
        void Speak(string name);
        ChatSomething Next { set; get; }
    }

    public abstract class BasicChat : ChatSomething
    {
        public abstract void Speak(string name);

        ChatSomething next;
        public ChatSomething Next
        {
            get
            {
                return next;
            }
            set
            {
                next = value;
            }
        }
    }


    public class SayHello : BasicChat
    {
        public override void Speak(string name)
        {
            Console.WriteLine("Hello {0}", name);
        }
    }

    public class NiceToMeetYou : BasicChat
    {
        public override void Speak(string name)
        {
            Console.WriteLine("Nice To Meet You {0}", name);
        }
    }
}