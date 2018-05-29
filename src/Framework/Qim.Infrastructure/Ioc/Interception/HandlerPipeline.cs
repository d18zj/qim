using System.Collections.Generic;
using System.Linq;

namespace Qim.Ioc.Interception
{
    public class HandlerPipeline
    {
        private readonly List<InterceptorAttribute> _attributes;

        internal HandlerPipeline()
        {
            _attributes = new List<InterceptorAttribute>();
        }


        public HandlerPipeline(IEnumerable<InterceptorAttribute> attributes)
        {
            _attributes = new List<InterceptorAttribute>(attributes);
        }


        public int Count => _attributes.Count;

        internal void Append(IEnumerable<InterceptorAttribute> attributes)
        {
            _attributes.AddRange(attributes);
        }

        public IMethodInterceptor[] GetMethodInterceptors(IIocResolver resolver)
        {
            return _attributes.Select(a => a.GetInterceptor(resolver)).Where(a => a != null).OrderBy(a => a.Order).ToArray(); 
        }
    }
}