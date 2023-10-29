using System;
using Unity.Collections;

[Serializable]
public class InspectorKeyValue<TKey, TValue>
{
    public TKey Key;
    public TValue Value;
}