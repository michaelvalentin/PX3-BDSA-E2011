// <copyright file="UserTest.cs">Copyright ©  2011</copyright>

using System;
using DigitalVoterList.Election;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace DigitalVoterList.Election
{
    [TestClass]
    [PexClass(typeof(User))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    public partial class UserTest
    {
        [PexMethod]
        public bool WorksHere([PexAssumeUnderTest]User target, VotingVenue v)
        {
            bool result = target.WorksHere(v);
            return result;
            // TODO: add assertions to method UserTest.WorksHere(User, VotingVenue)
        }
        [PexMethod]
        public HashSet<VotingVenue> WorkplacesGet([PexAssumeUnderTest]User target)
        {
            HashSet<VotingVenue> result = target.Workplaces;
            return result;
            // TODO: add assertions to method UserTest.WorkplacesGet(User)
        }
        [PexMethod]
        public bool ValidatedGet([PexAssumeUnderTest]User target)
        {
            bool result = target.Validated;
            return result;
            // TODO: add assertions to method UserTest.ValidatedGet(User)
        }
        [PexMethod]
        public string ToString01([PexAssumeUnderTest]User target)
        {
            string result = target.ToString();
            return result;
            // TODO: add assertions to method UserTest.ToString01(User)
        }
        [PexMethod]
        public HashSet<SystemAction> PermissionsGet([PexAssumeUnderTest]User target)
        {
            HashSet<SystemAction> result = target.Permissions;
            return result;
            // TODO: add assertions to method UserTest.PermissionsGet(User)
        }
        [PexMethod]
        public bool HasPermission([PexAssumeUnderTest]User target, SystemAction a)
        {
            bool result = target.HasPermission(a);
            return result;
            // TODO: add assertions to method UserTest.HasPermission(User, SystemAction)
        }
        [PexMethod]
        public bool FetchPermissions(
            [PexAssumeUnderTest]User target,
            string uname,
            string pwd
        )
        {
            bool result = target.FetchPermissions(uname, pwd);
            return result;
            // TODO: add assertions to method UserTest.FetchPermissions(User, String, String)
        }
        [PexMethod]
        public User Constructor01()
        {
            User target = new User();
            return target;
            // TODO: add assertions to method UserTest.Constructor01()
        }
        [PexMethod]
        public User Constructor(int id)
        {
            User target = new User(id);
            return target;
            // TODO: add assertions to method UserTest.Constructor(Int32)
        }
        [PexMethod]
        public void ChangePassword01([PexAssumeUnderTest]User target, string newPwd)
        {
            target.ChangePassword(newPwd);
            // TODO: add assertions to method UserTest.ChangePassword01(User, String)
        }
        [PexMethod]
        public void ChangePassword(
            [PexAssumeUnderTest]User target,
            string oldPwd,
            string newPwd
        )
        {
            target.ChangePassword(oldPwd, newPwd);
            // TODO: add assertions to method UserTest.ChangePassword(User, String, String)
        }
    }
}
