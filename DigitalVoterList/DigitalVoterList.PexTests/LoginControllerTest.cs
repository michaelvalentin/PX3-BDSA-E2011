// <copyright file="LoginControllerTest.cs">Copyright ©  2011</copyright>

using System;
using DigitalVoterList.Controllers;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DigitalVoterList.Views;

namespace DigitalVoterList.Controllers
{
    [TestClass]
    [PexClass(typeof(LoginController))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    public partial class LoginControllerTest
    {
        [PexMethod]
        public LoginController Constructor(LoginWindow view)
        {
            LoginController target = new LoginController(view);
            return target;
            // TODO: add assertions to method LoginControllerTest.Constructor(LoginWindow)
        }
    }
}
