﻿using FubuMVC.Core;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Runtime;
using FubuMVC.Core.Urls;
using Volta.Core.Application.Security;
using Volta.Core.Infrastructure.Web;
using Volta.Web.Handlers;

namespace Volta.Web.Behaviors
{
    public class AuthenticationBehavior : IActionBehavior
    {
        private readonly IOutputWriter _writer;
        private readonly IActionBehavior _actionBehavior;
        private readonly SecureRequest<LoginHandler, DashboardHandler> _request;

        public AuthenticationBehavior(IUrlRegistry registry, CurrentRequest request, IOutputWriter writer, IActionBehavior actionBehavior, ISecureSession secureSession)
        {
            _request = new SecureRequest<LoginHandler, DashboardHandler>(registry, request, secureSession, x => x.Query(null), x => x.Query());
            _writer = writer;
            _actionBehavior = actionBehavior;
        }

        public void Invoke()
        {
            if (_request.IsLoggedIn()) WhenLoggedIn(); else WhenNotLoggedIn();
        }

        private void WhenLoggedIn()
        {
            if (_request.IsOnLoginPage()) _writer.RedirectToUrl(_request.DefaultPageUrl());
            else _actionBehavior.Invoke();
        }

        private void WhenNotLoggedIn()
        {
            if (_request.IsOnLoginPage()) _actionBehavior.Invoke();
            else _writer.RedirectToUrl(_request.IsOnDefaultPage() ? _request.LoginPageUrl() : 
                                           _request.LoginPageUrl().AppendQueryStringValueFor<LoginOutputModel>(x => x.RedirectUrl, _request.Url()));
        }

        public void InvokePartial()
        {
            _actionBehavior.InvokePartial();
        }
    }
}