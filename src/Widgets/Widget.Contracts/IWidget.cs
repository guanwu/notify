namespace Guanwu.Notify.Widget
{
    public interface IWidget
    {
        string WidgetName { get; }
    }

    public interface ICrossDomainWidget : IWidget
    {

    }

    public interface IDomainWidget : IWidget
    {
        
    }
}
