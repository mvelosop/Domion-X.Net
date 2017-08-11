using System.Collections.Generic;
using cloudscribe.Web.Common.Extensions;
using cloudscribe.Web.Common.Models;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Domion.Web.Helpers
{
    public interface IViewAlerts
    {
        void Danger(string message, params object[] args);

        void Danger(string message, bool dismissable, params object[] args);

        void Information(string message, params object[] args);

        void Information(string message, bool dismissable, params object[] args);

        ViewAlerts SetTempData(ITempDataDictionary tempData);

        void Success(string message, params object[] args);

        void Success(string message, bool dismissable, params object[] args);

        void Warning(string message, params object[] args);

        void Warning(string message, bool dismissable, params object[] args);
    }

    public class ViewAlerts : IViewAlerts
    {
        private List<Alert> _alerts;


        public void Danger(string message, params object[] args)
        {
            Danger(message, false, args);
        }

        public void Danger(string message, bool dismissable, params object[] args)
        {
            _alerts.Add(new Alert
            {
                AlertStyle = AlertStyles.Danger,
                Message = string.Format(message, args),
                Dismissable = dismissable
            });
        }

        public void Information(string message, params object[] args)
        {
            Information(message, false, args);
        }

        public void Information(string message, bool dismissable, params object[] args)
        {
            _alerts.Add(new Alert
            {
                AlertStyle = AlertStyles.Information,
                Message = string.Format(message, args),
                Dismissable = dismissable
            });
        }

        public ViewAlerts SetTempData(ITempDataDictionary tempData)
        {
            _alerts = tempData.GetAlerts();

            return this;
        }

        public void Success(string message, params object[] args)
        {
            Success(message, true, args);
        }

        public void Success(string message, bool dismissable, params object[] args)
        {
            _alerts.Add(new Alert
            {
                AlertStyle = AlertStyles.Success,
                Message = string.Format(message, args),
                Dismissable = dismissable
            });
        }

        public void Warning(string message, params object[] args)
        {
            Warning(message, false, args);
        }

        public void Warning(string message, bool dismissable, params object[] args)
        {
            _alerts.Add(new Alert
            {
                AlertStyle = AlertStyles.Warning,
                Message = string.Format(message, args),
                Dismissable = dismissable
            });
        }
    }
}
