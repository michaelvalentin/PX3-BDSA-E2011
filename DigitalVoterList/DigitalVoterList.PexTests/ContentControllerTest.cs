// <copyright file="ContentControllerTest.cs">Copyright ©  2011</copyright>

using System;
using DigitalVoterList.Controllers;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DigitalVoterList.Election;

namespace DigitalVoterList.Controllers
{
    [TestClass]
    [PexClass(typeof(ContentController))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    public partial class ContentControllerTest
    {
        [PexMethod]
        public bool HasPermissionToUse([PexAssumeUnderTest]ContentController target, User u)
        {
            bool result = target.HasPermissionToUse(u);
            return result;
            // TODO: add assertions to method ContentControllerTest.HasPermissionToUse(ContentController, User)
        }
        [PexMethod]
        public ContentController Constructor()
        {
            ContentController target = new ContentController();
            return target;
            // TODO: add assertions to method ContentControllerTest.Constructor()
        }
    }
}
