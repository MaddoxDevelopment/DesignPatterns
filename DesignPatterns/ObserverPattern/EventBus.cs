using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;

namespace DesignPatterns.ObserverPattern
{
    //todo better locking on subscribing / unsubscribing while it dispatches? The concurrent dictionary should handle this, but look into more.
    public class EventBus
    {
        /// <summary>
        /// The objects that are actually subscribed to the listener.
        /// </summary>
        private readonly IDictionary<string, object> _subscribedTypes;
        
        /// <summary>
        /// Every method in the assembly that has one parameter and the EventSubscriber attribute
        /// Even if they are not subscribed to the event bus.
        /// </summary>
        private readonly IDictionary<Type, IList<KeyValuePair<string, MethodInfo>>> _listeners;

        public EventBus()
        {
            _subscribedTypes = new ConcurrentDictionary<string, object>();
            var crawler = new EventBusCrawler();
            _listeners = crawler.BuildListeners();
        }

        /// <summary>
        /// Subscribes to the event bus.
        /// Events will only be dispatched to the particular listening type
        /// once they are subscribed and the instance is in _subscribeTypes.
        /// </summary>
        /// <param name="subscriber"></param>
        public void Subscribe(object subscriber)
        {
            if (subscriber == null)
                return;
            var type = subscriber.GetType();
            _subscribedTypes.Add(type.FullName, subscriber);
        }

        /// <summary>
        /// Removes the instance from the event bus.
        /// It will not longer recieve events.
        /// </summary>
        /// <param name="subscriber"></param>
        public void Unsubscribe(object subscriber)
        {
            if (subscriber == null)
                return;
            _subscribedTypes.Remove(subscriber.GetType().FullName);
        }

        /// <summary>
        /// Dispatch the event to all subscribed listeners that have a method
        /// with one parameter that matches the exact type of the object being passed in.
        /// </summary>
        /// <param name="toDispatch"></param>
        /// //todo try catch this?
        public void Dispatch(object toDispatch)
        {
            var eventType = toDispatch.GetType();
            if (!_listeners.ContainsKey(eventType))
                return;
            var pairs = _listeners[eventType];
            using (var enumerator = pairs.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    var pair = enumerator.Current;
                    var typeName = pair.Key;
                    if (_subscribedTypes.ContainsKey(typeName))
                        pair.Value.Invoke(_subscribedTypes[typeName], new[] {toDispatch});
                }
            }
        }
    }
}