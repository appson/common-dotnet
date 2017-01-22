using System;

namespace Appson.Common.Owin
{
    internal class UnsubscribeDisposable : IDisposable
    {
        readonly IDisposable _target;
        bool _unsubscribe;

        public UnsubscribeDisposable(IDisposable target)
        {
            _target = target;
        }

        public void CallTargetDispose()
        {
            if (!_unsubscribe)
            {
                _target.Dispose();
            }
        }

        public void Dispose()
        {
            _unsubscribe = true;
        }
    }
}