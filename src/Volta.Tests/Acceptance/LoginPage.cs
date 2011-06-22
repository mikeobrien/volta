﻿using System;
using Volta.Tests.Acceptance.Common;
using Volta.Web.Handlers;
using WatiN.Core;

namespace Volta.Tests.Acceptance
{
    public class LoginPage : VoltaWebPage<LoginInputModel>
    {
        public LoginPage()
        {
            BaseUrl = new Uri(Constants.VoltaUrl, "/login");
        }

        public TextField UsernameTextField { get { return Form.TextField(FindById(x => x.Username)); } }
        public TextField PasswordTextField { get { return Form.TextField(FindById(x => x.Password)); } }
        public Button SubmitButton { get { return Form.Button(FindById(x => x.Action)); } }

        public LoginPage Submit()
        {
            SubmitButton.Click();
            return this;
        }
    }
}