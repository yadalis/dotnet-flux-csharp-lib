using System;
using System.Collections.Generic;
using System.Linq;

namespace MHC.Architecture.Flux
{
    public static class Dispatcher
    {
        private static Dictionary<string, Action<BusinessAction>> _processActionsListDict = new Dictionary<string, Action<BusinessAction>>();


        public static void Dispatch(BusinessAction BusinessAction)
        {
            //Read all stores registered with this Dispatcher and exectue the ProcessAction method on each store.
            foreach (var processAction in _processActionsListDict.ToList())
            {
                processAction.Value(BusinessAction);
            }
        }

        public static void Register(string Key, Action<BusinessAction> func)
        {
            if (_processActionsListDict.ContainsKey(Key))
            {
                UnRegister(Key);
            }

            _processActionsListDict.Add(Key, func);
        }

        public static void Register(string Key, Action<BusinessAction> func, bool SkipRegistration)
        {
            if (SkipRegistration) return;

            if (_processActionsListDict.ContainsKey(Key))
            {
                UnRegister(Key);
            }

            _processActionsListDict.Add(Key, func);
        }

        public static void UnRegister(string Key) => _processActionsListDict.Remove(Key);
    }
}
