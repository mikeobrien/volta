using System;
using System.Diagnostics;
using NUnit.Framework;

namespace Volta.Tests.Acceptance.Common
{
    [TestFixture]
    public abstract class WebPageTestBase<T> where T : WebPage, new()
    {
        private readonly bool _newProcess;

        protected WebPageTestBase() {}
        protected WebPageTestBase(bool newProcess) { _newProcess = newProcess; }

        protected T Page { get; private set; }

        [SetUp]
        public virtual void Setup()
        {
            Page = new T();
            Page.Open(_newProcess);
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
        }
    }
}