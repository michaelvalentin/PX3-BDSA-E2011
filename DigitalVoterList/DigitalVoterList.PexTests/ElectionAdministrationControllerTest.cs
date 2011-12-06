// <copyright file="ElectionAdministrationControllerTest.cs">Copyright ©  2011</copyright>

using System;
using DigitalVoterList.Controllers;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DigitalVoterList.Views;

namespace DigitalVoterList.Controllers
{
    [TestClass]
    [PexClass(typeof(ElectionAdministrationController))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    public partial class ElectionAdministrationControllerTest
    {
        [PexMethod]
        public ElectionAdministrationController Constructor(ElectionAdministrationView view)
        {
            ElectionAdministrationController target = new ElectionAdministrationController(view);
            return target;
            // TODO: add assertions to method ElectionAdministrationControllerTest.Constructor(ElectionAdministrationView)
        }
    }
}
