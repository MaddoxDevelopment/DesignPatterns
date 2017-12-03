using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DesignPatterns.ObserverPattern.impl;

namespace DesignPatterns.ObserverPattern
{
    public class EventBusCrawler
    {
        /// <summary>
        /// Take all the listening methods, and add it to a dictionary with the
        /// parameter as the key.
        /// 
        /// When an object is passed in to dispatch, we can easily lookup
        /// which methods are expecting that type, by the dictionary key.
        /// </summary>
        /// <returns></returns>
        public Dictionary<Type, IList<KeyValuePair<string, MethodInfo>>> BuildListeners()
        {
            var listening = GetListeningMethods();
            var temp = new Dictionary<Type, IList<KeyValuePair<string, MethodInfo>>>();
            foreach (var pair in listening)
            {
                var method = pair.Value;
                var parameter = method.GetParameters()
                    .Select(w => w.ParameterType).FirstOrDefault();
                if (!temp.ContainsKey(parameter))
                    temp.Add(parameter, new List<KeyValuePair<string, MethodInfo>>(new[] {pair}));
                else
                    temp[parameter].Add(pair);
            }
            return temp;
        }
        
        /// <summary>
        /// Find all methods in the current assembly that have a method
        /// with one parameter, and have the event subscriber attribute.
        /// 
        /// We must return a list of key value pairs because we need the full name
        /// of the calling class to invoke the method later on with the actual class instance.
        /// 
        /// This method is heavy and should only be called at the startup of the EventBus.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<KeyValuePair<string, MethodInfo>> GetListeningMethods()
        {
            var temp = new HashSet<KeyValuePair<string, MethodInfo>>();
            var types = Assembly.GetExecutingAssembly().GetTypes();
            foreach (var type in types)
            {
                foreach (var method in type.GetMethods())
                {
                    if(method.GetCustomAttribute<EventSubscriber>() == null)
                        continue;
                    if(method.GetParameters().Length != 1)
                        continue;
                    temp.Add(new KeyValuePair<string, MethodInfo>(type.FullName, method));
                }
            }
            return temp;
        }
    }
}