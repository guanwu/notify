namespace Guanwu.Notify.Widget
{
    public interface IProfile : ICrossDomainWidget
    {
        void Refresh();
        dynamic Get(string name, string key = "");
    }
}
