// <copyright file="LoginEventArgsTest.cs">Copyright ©  2011</copyright>

using System;
using DigitalVoterList.Views;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DigitalVoterList.Views
{
    [TestClass]
    [PexClass(typeof(LoginEventArgs))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    public partial class LoginEventArgsTest
    {
        [PexMethod]
        public LoginEventArgs Constructor(string username, string password)
        {
            LoginEventArgs target = new LoginEventArgs(username, password);
            return target;
            // TODO: add assertions to method LoginEventArgsTest.Constructor(String, String)
        }
    }
}
