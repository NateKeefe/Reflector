namespace Scribe.Connector.Common.Reflection.PropertyType
{

    public interface IProp
    {
        object Get(object target);
        void Set(object target, object val);

    }

    public interface IProp<out T>
    {
        void Set(object target, object val);
        T Get(object target);

    }
}