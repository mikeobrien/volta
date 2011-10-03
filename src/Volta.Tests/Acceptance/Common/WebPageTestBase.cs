using System;
using NUnit.Framework;

namespace Volta.Tests.Acceptance.Common
{
    [TestFixture]
    public abstract class WebPageTestBase<T> where T : WebPage, new()
    {
        protected T Page { get; private set; }

        [SetUp]
        public virtual void Setup()
        {
            Page = new T();
            Page.Open();
        }

        [TearDown]
        public virtual void TearDown()
        {
            Console.WriteLine("-------------------------------------------------------------");
            Console.WriteLine(Page.GetCurrentUrl());
            Console.WriteLine("-------------------------------------------------------------");
            Console.WriteLine(Page.GetHtml());
            Console.WriteLine("-------------------------------------------------------------");
            Page.Close();
            Page.Kill();
        }
    }
}