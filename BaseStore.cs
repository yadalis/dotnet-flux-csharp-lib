using System;
namespace MHC.Architecture.Flux
{
    public abstract class BaseStore
    {
        private Action<string> _viewListenerWithStringParam = null;

        public void SetChangeListener(Action<string> func) => _viewListenerWithStringParam = func;

        public void EmitChange(string MessageName) => _viewListenerWithStringParam(MessageName);
    }
}
