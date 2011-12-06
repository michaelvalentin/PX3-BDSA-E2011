// <copyright file="PermissionExceptionTest.cs">Copyright ©  2011</copyright>

using System;
using DigitalVoterList.Election;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DigitalVoterList.Election
{
    [TestClass]
    [PexClass(typeof(PermissionException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    public partial class PermissionExceptionTest
    {
        [PexMethod]
        public PermissionException Constructor(
            SystemAction systemAction,
            User user,
            string msg
        )
        {
            PermissionException target = new PermissionException(systemAction, user, msg);
            return target;
            // TODO: add assertions to method PermissionExceptionTest.Constructor(SystemAction, User, String)
        }
    }
}
