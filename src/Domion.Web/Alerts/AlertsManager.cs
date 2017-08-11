using System;
using System.Collections.Generic;
using cloudscribe.Web.Common.Extensions;
using cloudscribe.Web.Common.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;

namespace Domion.Web.Alerts
{
    /// <summary>
    ///     Extends cloudscribe's alerts management so it can be injected
    /// </summary>
    public class AlertsManager : IAlertsManager
    {
        private readonly ITempDataDictionary _tempData;

        public AlertsManager(IHttpContextAccessor contextAccessor)
        {
            HttpContext httpContext = contextAccessor.HttpContext;

            var factory = httpContext?.RequestServices?.GetRequiredService<ITempDataDictionaryFactory>();

            _tempData = factory?.GetTempData(httpContext);

            if (_tempData == null) throw new InvalidOperationException("Could not get TempData");
        }

        public void Danger(string message, params object[] args)
        {
            AddAlert(AlertStyles.Danger, string.Format(message, args), true);
        }

        public void Danger(string message, bool dismissable, params object[] args)
        {
            AddAlert(AlertStyles.Danger, string.Format(message, args), dismissable);
        }

        public void Information(string message, params object[] args)
        {
            AddAlert(AlertStyles.Information, string.Format(message, args), true);
        }

        public void Information(string message, bool dismissable, params object[] args)
        {
            AddAlert(AlertStyles.Information, string.Format(message, args), dismissable);
        }

        public void Success(string message, params object[] args)
        {
            AddAlert(AlertStyles.Success, string.Format(message, args), true);
        }

        public void Success(string message, bool dismissable, params object[] args)
        {
            AddAlert(AlertStyles.Success, string.Format(message, args), dismissable);
        }

        public void Warning(string message, params object[] args)
        {
            AddAlert(AlertStyles.Warning, string.Format(message, args), true);
        }

        public void Warning(string message, bool dismissable, params object[] args)
        {
            AddAlert(AlertStyles.Warning, string.Format(message, args), dismissable);
        }

        private void AddAlert(string alertStyle, string message, bool dismissable)
        {
            List<Alert> alerts = _tempData.GetAlerts();

            alerts.Add(new Alert
            {
                AlertStyle = alertStyle,
                Message = message,
                Dismissable = dismissable
            });

            _tempData.AddAlerts(alerts);
        }
    }
}
