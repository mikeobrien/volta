//using System;
//using System.Diagnostics;
//using System.Linq;
//using System.Runtime.InteropServices;
//using WatiN.Core;

//namespace Volta.Tests.Acceptance.Common
//{
//    public abstract class WebPage
//    {
//        protected IE Browser { get; set; }
//        protected Uri BaseUrl { get; set; }

//        public void Open()
//        {
//            Settings.MakeNewIeInstanceVisible = false;
//            Browser = new IE(BaseUrl);
//            Browser.ClearCache();
//            Browser.ClearCookies();
//        }

//        public bool IsOnPage()
//        {
//            return Browser.Uri.AbsolutePath == BaseUrl.AbsolutePath;
//        }

//        public bool IsUnderPage()
//        {
//            return Browser.Uri.AbsolutePath.StartsWith(BaseUrl.AbsolutePath + "/");
//        }

//        public void Close()
//        {
//            Browser.Dispose();
//        }

//        public void Kill()
//        {
//            try { if (Browser == null || Browser.hWnd == IntPtr.Zero) return; }
//            catch (Exception) { return; }
//            var threadId = GetWindowThreadProcessId(Browser.hWnd, IntPtr.Zero);
//            Process.GetProcessesByName("iexplore").First(x => x.Threads.Cast<ProcessThread>().Any(y => y.Id == threadId)).Kill();
//        }

//        public WebPage GoHome()
//        {
//            Browser.GoTo(BaseUrl);
//            return this;
//        }

//        public WebPage TakeScreenshot(string path)
//        {
//            Browser.CaptureWebPageToFile(path);
//            return this;
//        }

//        public string GetHtml()
//        {
//            return Browser != null ? Browser.ActiveElement.Parent.OuterHtml : "No browser found.";
//        }

//        public string GetCurrentUrl()
//        {
//            return Browser != null ? Browser.Url : "No browser found.";
//        }

//        public WebPage NavigateTo<T>(params string[] parts) where T : WebPage, new()
//        {
//            return NavigateTo<T>(null, parts);
//        }

//        public WebPage NavigateTo<T>(Uri relativeUrl, params string[] parts) where T : WebPage, new()
//        {
//            return new T().NavigateTo(this, relativeUrl, parts);
//        }

//        public T SwitchTo<T>() where T : WebPage, new()
//        {
//            if (this is T) return (T)this;
//            return new T { Browser = Browser };
//        }

//        protected WebPage NavigateTo(WebPage fromPage, Uri url, params string[] parts)
//        {
//            Browser = fromPage.Browser;
//            Browser.GoTo((url != null ? new Uri(BaseUrl, url) : BaseUrl) + parts.Aggregate(string.Empty, (a, i) => a + "/" + i));
//            return this;
//        }

//        [DllImport("user32.dll")]
//        private static extern int GetWindowThreadProcessId(IntPtr windowHandle, IntPtr processId);
//    }
//}