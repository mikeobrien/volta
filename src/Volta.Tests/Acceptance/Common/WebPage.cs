using System;
using WatiN.Core;

namespace Volta.Tests.Acceptance.Common
{
    public abstract class WebPage
    {
        protected IE Browser { get; set; }
        protected Uri BaseUrl { get; set; }

        public void Open()
        {
            Settings.MakeNewIeInstanceVisible = false;
            Browser = new IE(BaseUrl);
            Browser.ClearCache();
            Browser.ClearCookies();
        }

        public bool IsOnPage()
        {
            return Browser.Uri == BaseUrl;
        }

        public void Close()
        {
            Browser.Dispose();
        }

        public WebPage GoHome()
        {
            Browser.GoTo(BaseUrl);
            return this;
        }

        public WebPage TakeScreenshot(string path)
        {
            Browser.CaptureWebPageToFile(path);
            return this;
        }

        public string GetHtml()
        {
            return Browser.Html;
        }

        public WebPage NavigateTo<T>() where T : WebPage, new()
        {
            return NavigateTo<T>(null);
        }

        public WebPage NavigateTo<T>(Uri relativeUrl) where T : WebPage, new()
        {
            return new T().NavigateTo(this, relativeUrl);
        }

        public T SwitchTo<T>() where T : WebPage, new()
        {
            if (this is T) return (T)this;
            return new T { Browser = Browser };
        }

        protected WebPage NavigateTo(WebPage fromPage, Uri url)
        {
            Browser = fromPage.Browser;
            Browser.GoTo(url != null ? new Uri(BaseUrl, url) : BaseUrl);
            return this;
        }
    }
}