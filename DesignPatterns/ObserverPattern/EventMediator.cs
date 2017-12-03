namespace DesignPatterns.ObserverPattern
{
    public class EventMediator
    {
        public static EventMediator Instance => _instance ?? (_instance = new EventMediator());
        private static EventMediator _instance;
        private EventMediator() => Bus = new EventBus();
        public EventBus Bus { get; }      
    }
}