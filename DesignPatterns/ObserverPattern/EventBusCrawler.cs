using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DesignPatterns.ObserverPattern
{
    public class EventBusCrawler
    {
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
        
        private IEnumerable<KeyValuePair<string, MethodInfo>> GetListeningMethods()
        {
            var temp = new HashSet<KeyValuePair<string, MethodInfo>>();
            var types = Assembly.GetExecutingAssembly().GetTypes();
            foreach (var type in types)
            {
                foreach (var method in type.GetMethods())
                {
                    if(method.GetParameters().Length != 1)
                        continue;
                    temp.Add(new KeyValuePair<string, MethodInfo>(type.FullName, method));
                }
            }
            return temp;
        }
    }
}