// <copyright file="VoterListAppTest.cs">Copyright ©  2011</copyright>

using System;
using DigitalVoterList;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DigitalVoterList.Election;

namespace DigitalVoterList
{
    [TestClass]
    [PexClass(typeof(VoterListApp))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    public partial class VoterListAppTest
    {
        [PexMethod]
        public void RunApp(User user)
        {
            VoterListApp.RunApp(user);
            // TODO: add assertions to method VoterListAppTest.RunApp(User)
        }
        [PexMethod]
        public void Main()
        {
            VoterListApp.Main();
            // TODO: add assertions to method VoterListAppTest.Main()
        }
        [PexMethod]
        public void CurrentUserSet(User value)
        {
            VoterListApp.CurrentUser = value;
            // TODO: add assertions to method VoterListAppTest.CurrentUserSet(User)
        }
        [PexMethod]
        public User CurrentUserGet()
        {
            User result = VoterListApp.CurrentUser;
            return result;
            // TODO: add assertions to method VoterListAppTest.CurrentUserGet()
        }
    }
}
