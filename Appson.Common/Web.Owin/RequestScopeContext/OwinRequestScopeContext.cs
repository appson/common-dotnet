using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using Microsoft.Owin;

namespace Appson.Common.Web.Owin.RequestScopeContext
{
    public class OwinRequestScopeContext
    {
        const string CallContextKey = "owin.rscopectx";

        /// <summary>
        /// Gets or sets the IOwinRequestScopeContext object for the current HTTP request.
        /// </summary>
        public static OwinRequestScopeContext Current
        {
            get
            {
                return CallContext.LogicalGetData(CallContextKey) as OwinRequestScopeContext;
            }
            set
            {
                CallContext.LogicalSetData(CallContextKey, value);
            }
        }

        internal static void FreeContextSlot()
        {
            CallContext.FreeNamedDataSlot(CallContextKey);
        }

        private readonly DateTime _utcTimestamp;
        private readonly List<UnsubscribeDisposable> _disposables;

        public IDictionary<string, object> Environment { get; private set; }
        public IOwinContext OwinContext { get; private set; }
        public ConcurrentDictionary<string, object> Items { get; private set; }
        public DateTime Timestamp => _utcTimestamp.ToLocalTime();
        public object SyncObject { get; }

        public OwinRequestScopeContext(IDictionary<string, object> environment)
        {
            _utcTimestamp = DateTime.UtcNow;
            _disposables = new List<UnsubscribeDisposable>();
            SyncObject = new object();

            Environment = environment;
            OwinContext = new OwinContext(environment);
            Items = new ConcurrentDictionary<string, object>();
        }

        public IDisposable DisposeOnPipelineCompleted(IDisposable target)
        {
            if (target == null) 
                throw new ArgumentNullException(nameof(target));

            var token = new UnsubscribeDisposable(target);
            _disposables.Add(token);

            return token;
        }

        internal void Complete()
        {
            var exceptions = new List<Exception>();
            try
            {
                foreach (var item in _disposables)
                {
                    item.CallTargetDispose();
                }
            }
            catch (Exception ex)
            {
                exceptions.Add(ex);
            }
            finally
            {
                if (exceptions.Any())
                {
                    throw new AggregateException("failed on disposing", exceptions);
                }
            }
        }
    }
}