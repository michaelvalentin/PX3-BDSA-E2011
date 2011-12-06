// <copyright file="ManualRegistrationControllerTest.cs">Copyright ©  2011</copyright>

using System;
using DigitalVoterList.Controllers;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DigitalVoterList.Views;

namespace DigitalVoterList.Controllers
{
    [TestClass]
    [PexClass(typeof(ManualRegistrationController))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    public partial class ManualRegistrationControllerTest
    {
        [PexMethod]
        public ManualRegistrationController Constructor(ManualRegistrationView view)
        {
            ManualRegistrationController target = new ManualRegistrationController(view);
            return target;
            // TODO: add assertions to method ManualRegistrationControllerTest.Constructor(ManualRegistrationView)
        }
    }
}
