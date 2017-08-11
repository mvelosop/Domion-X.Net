namespace Domion.Web.Alerts
{
    public interface IAlertsManager
    {
        void Danger(string message, params object[] args);

        void Danger(string message, bool dismissable, params object[] args);

        void Information(string message, params object[] args);

        void Information(string message, bool dismissable, params object[] args);

        void Success(string message, params object[] args);

        void Success(string message, bool dismissable, params object[] args);

        void Warning(string message, params object[] args);

        void Warning(string message, bool dismissable, params object[] args);
    }
}
