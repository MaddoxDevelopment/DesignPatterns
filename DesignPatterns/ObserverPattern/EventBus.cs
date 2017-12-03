using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;

namespace DesignPatterns.ObserverPattern
{
    public class EventBus
    {
        private readonly IDictionary<string, object> _subscribedTypes;
        private readonly IDictionary<Type, IList<KeyValuePair<string, MethodInfo>>> _listeners;

        public EventBus()
        {
            _subscribedTypes = new ConcurrentDictionary<string, object>();
            var crawler = new EventBusCrawler();
            _listeners = crawler.BuildListeners();
        }

        public void Subscribe(object subscriber)
        {
            if (subscriber == null)
                return;
            var type = subscriber.GetType();
            _subscribedTypes.Add(type.FullName, subscriber);
        }

        public void Unsubscribe(object subscriber)
        {
            if (subscriber == null)
                return;
            _subscribedTypes.Remove(subscriber.GetType().FullName);
        }

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