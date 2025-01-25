using System.Diagnostics;

namespace Common.Domain.ValueObjects;

[DebuggerDisplay("Value = {Value}; Type = {_typeName}")]
public abstract record NewValueType<TValueType, T>(T Value)
    where TValueType : NewValueType<TValueType, T>
{
    private static readonly string _typeName = typeof(TValueType).Name!;

    public static TValueType FromValue(T value)
        => (TValueType)Activator.CreateInstance(typeof(TValueType), value)!;

    public static explicit operator T(NewValueType<TValueType, T> value) => value.Value;
}
