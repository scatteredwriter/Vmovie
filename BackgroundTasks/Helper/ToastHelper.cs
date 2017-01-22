using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace BackgroundTasks.Helper
{
    public sealed class ToastHelper
    {
        public static ToastNotification PopToast(string title, string content)
        {
            string xml = @"<toast><visual><binding template=""ToastGeneric""><text>"
+ title + @"</text><text>"
+ content + @"</text><image placement=""appLogoOverride"" src=""Assets/Square44x44Logo.scale-400.png""/></binding></visual></toast>";
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            return PopCustomToast(doc);
        }

        public static ToastNotification PopToast(string title, string content, string launch)
        {
            string xml = @"<toast launch="""
    + launch + @"""><visual><binding template=""ToastGeneric""><text>"
    + title + @"</text><text>"
    + content + @"</text><image placement=""appLogoOverride"" src=""Assets/Square44x44Logo.scale-400.png""/></binding></visual></toast>";
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            return PopCustomToast(doc);
        }

        public static ToastNotification PopCustomToast(XmlDocument doc)
        {
            ToastNotification toast = new ToastNotification(doc);
            ToastNotificationManager.CreateToastNotifier().Show(toast);
            return toast;
        }
    }
}
