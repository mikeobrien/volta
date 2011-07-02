using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using WatiN.Core;

namespace Volta.Tests.Acceptance.Common
{
    public abstract class WebPage
    {
        protected IE Browser { get; set; }
        protected Uri BaseUrl { get; set; }

        public void Open(bool newProcess)
        {
            Settings.MakeNewIeInstanceVisible = false;
            Browser = new IE(BaseUrl, false);
            Browser.ClearCache();
            Browser.ClearCookies();
        }

        public bool IsOnPage()
        {
            return Browser.Uri.AbsolutePath == BaseUrl.AbsolutePath;
        }

        [DllImport("user32.dll")]
        static extern int GetWindowThreadProcessId(IntPtr windowHandle, IntPtr processId);

        public void Close()
        {
            var threadId = GetWindowThreadProcessId(Browser.hWnd, IntPtr.Zero);
            Process.GetProcessesByName("iexplore").First(x => x.Threads.Cast<ProcessThread>().Any(y => y.Id == threadId)).Kill();
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
            return Browser.ActiveElement.Parent.OuterHtml;
        }

        public string GetCurrentUrl()
        {
            return Browser.Url;
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