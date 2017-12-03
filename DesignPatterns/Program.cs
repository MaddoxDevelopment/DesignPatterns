using System.Threading;
using DesignPatterns.ObserverPattern;
using DesignPatterns.ObserverPattern.Examples;

namespace DesignPatterns
{
    class Program
    {
        static void Main(string[] args)
        {
            new EventListener();

            for (var i = 0; i < 50000; i++)
            {
                EventMediator.Instance.Bus.Dispatch(new UserCreatedEvent
                {
                    Email = "dev@madev.me",
                    Username = "Tester"
                });
                Thread.Sleep(10);
            }
        }
    }
}