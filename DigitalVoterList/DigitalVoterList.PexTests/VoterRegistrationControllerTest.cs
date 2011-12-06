// <copyright file="VoterRegistrationControllerTest.cs">Copyright ©  2011</copyright>

using System;
using DigitalVoterList.Controllers;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DigitalVoterList.Views;

namespace DigitalVoterList.Controllers
{
    [TestClass]
    [PexClass(typeof(VoterRegistrationController))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    public partial class VoterRegistrationControllerTest
    {
        [PexMethod]
        public VoterRegistrationController Constructor(VoterRegistrationView view)
        {
            VoterRegistrationController target = new VoterRegistrationController(view);
            return target;
            // TODO: add assertions to method VoterRegistrationControllerTest.Constructor(VoterRegistrationView)
        }
    }
}
