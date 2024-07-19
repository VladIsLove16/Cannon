using System;
using Unity.Properties;

public class BindableProperty<T>
{
    readonly Func<T> getter;
    BindableProperty(Func<T> getter)
    {
        this.getter = getter;
    }
    [CreateProperty]
    public T Value => getter();
    public static BindableProperty<T> Bind(Func<T> getter) => new BindableProperty<T>(getter);
}