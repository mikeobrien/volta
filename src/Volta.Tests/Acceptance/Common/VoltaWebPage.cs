using System;
using System.Linq.Expressions;
using Volta.Core.Infrastructure;
using WatiN.Core;
using WatiN.Core.Constraints;

namespace Volta.Tests.Acceptance.Common
{
    public abstract class VoltaWebPage : WebPage
    {
        public WebPage Login(string username, string password)
        {
            throw new NotImplementedException();
        }

        public bool HasMessage
        {
            get
            {
                var message = GetMessage();
                return message.Exists && !string.IsNullOrEmpty(message.Text);
            }
        }

        public string MessageText { get { return GetMessage().Text; } }

        private Div GetMessage()
        {
            return Browser.Div(Find.ById("message"));
        }
    }

    public abstract class VoltaWebPage<TInputModel> : VoltaWebPage
    {
        public bool HasFormLabelAndInput(Expression<Func<TInputModel, object>> property)
        {
            var form = Browser.Form(FindById());
            return form.Label(FindById(property)).Exists &&
                   form.Label(FindById(property)).Exists;
        }

        public Form Form { get { return Browser.Form(FindById()); } }

        public AttributeConstraint FindById()
        {
            return Find.ById(typeof(TInputModel).Name);
        }

        public AttributeConstraint FindById(Expression<Func<TInputModel, object>> id)
        {
            return Find.ById(id.GetPropertyName());
        }
    }
}