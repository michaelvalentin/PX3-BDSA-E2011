// <copyright file="VoterListAppTest.RunApp.g.cs">Copyright �  2011</copyright>
// <auto-generated>
// This file contains automatically generated unit tests.
// Do NOT modify this file manually.
// 
// When Pex is invoked again,
// it might remove or update any previously generated unit tests.
// 
// If the contents of this file becomes outdated, e.g. if it does not
// compile anymore, you may delete this file and invoke Pex again.
// </auto-generated>
using System;
using DigitalVoterList.Election;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Pex.Framework.Generated;

namespace DigitalVoterList
{
    public partial class VoterListAppTest
    {
[TestMethod]
[PexGeneratedBy(typeof(VoterListAppTest))]
[PexRaisedException(typeof(InvalidOperationException))]
public void RunAppThrowsInvalidOperationException707()
{
    User user;
    user = new User(0);
    user.Username = (string)null;
    user.UserSalt = (string)null;
    user.Valid = false;
    user.Title = (string)null;
    ((Person)user).Cpr = (string)null;
    ((Person)user).PassportNumber = (string)null;
    this.RunApp(user);
}
[TestMethod]
[PexGeneratedBy(typeof(VoterListAppTest))]
[PexRaisedException(typeof(InvalidOperationException))]
public void RunAppThrowsInvalidOperationException676()
{
    this.RunApp((User)null);
}
    }
}