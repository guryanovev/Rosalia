﻿namespace Rosalia.TestingSupport
{
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;
    using Rosalia.Core;
    using Rosalia.Core.Logging;

    public class SpyLogRenderer : ILogRenderer
    {
        private readonly IList<Tuple<Message, Identity>> _messages = new List<Tuple<Message, Identity>>();

        public IList<Tuple<Message, Identity>> Messages
        {
            get { return _messages; }
        }

        public void Init()
        {
        }

        public void Render(Message message, Identity source)
        {
            Messages.Add(new Tuple<Message, Identity>(message, source));
        }

        public void Dispose()
        {
        }

        public void AssertHasMessage(MessageLevel level, string message)
        {
            foreach (var messageTuple in _messages)
            {
                if (messageTuple.Item1.Level == level && messageTuple.Item1.Text == message)
                {
                    return;
                }
            }

            Assert.Fail("Message [{0}] with level {1} not found", message, level);
        }
    }
}