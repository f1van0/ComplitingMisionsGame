using System;

namespace Utility
{
    [Serializable]
    public class InspectorKeyValue<TKey, TValue>
    {
        public TKey Key;
        public TValue Value;
    }
}