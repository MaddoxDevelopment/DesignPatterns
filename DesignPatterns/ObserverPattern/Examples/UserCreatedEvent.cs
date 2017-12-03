using System;

namespace DesignPatterns.ObserverPattern.Examples
{
    public class UserCreatedEvent
    {
        public string Email { get; set; }      
        public string Username { get; set; }
    }
}