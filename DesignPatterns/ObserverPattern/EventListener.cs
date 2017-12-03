using System;
using DesignPatterns.ObserverPattern.Examples;
using DesignPatterns.ObserverPattern.impl;

namespace DesignPatterns.ObserverPattern
{
    public class EventListener
    {
        public EventListener()
        {
            EventMediator.Instance.Bus.Subscribe(this);
        }

        [EventSubscriber]
        public void OnName(string name)
        {
            Console.WriteLine("Got name. " + name);
        }

        [EventSubscriber]
        public void OnTestEvent(UserCreatedEvent created)
        {
            Console.WriteLine("User was created: " + created.Email + " " + created.Username);
        }
    }
}