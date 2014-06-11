using Autofac.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcSample.Web
{
    public class MonitoringController
    {
        public string ActivatedTypes()
        {
            var values = MonitoringModule.InstanceCount.ToArray();

            var builder = new StringBuilder();

            Array.Sort(values, new InstancesComparer());

            foreach (var item in values)
            {
                builder.AppendLine(GetTypeName(item.Key.Item1) + " " + item.Value);
            }

            return builder.ToString();
        }

        public void Clear()
        {
            MonitoringModule.Clear();
        }

        private string GetTypeName(Type type)
        {
            var genericArgs = type.GetGenericArguments().Select(t => t.Name).ToArray();
            
            if (genericArgs.Length > 0)
            {
                return type.Name.Substring(0, type.Name.Length - 2) + "<" + string.Join(",", genericArgs) + ">";
            }
            else
            {
                return type.Name;
            }
        }

        public class InstancesComparer : IComparer<KeyValuePair<Tuple<Type, IComponentLifetime>, int>>
        {
            public int Compare(KeyValuePair<Tuple<Type, IComponentLifetime>, int> x, KeyValuePair<Tuple<Type, IComponentLifetime>, int> y)
            {
                if (x.Value < y.Value)
                {
                    return 1;
                }
                else if (x.Value > y.Value)
                {
                    return -1;
                }
                else
                {
                    return 0;
                }
            }
        }
    }
}