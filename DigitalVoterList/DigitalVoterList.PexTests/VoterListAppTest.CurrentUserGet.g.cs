// <copyright file="VoterListAppTest.CurrentUserGet.g.cs">Copyright �  2011</copyright>
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
public void CurrentUserGet172()
{
    User user;
    user = this.CurrentUserGet();
    Assert.IsNotNull((object)user);
    Assert.AreEqual<string>((string)null, user.Username);
    Assert.AreEqual<string>((string)null, user.UserSalt);
    Assert.AreEqual<bool>(false, user.Valid);
    Assert.AreEqual<int>(0, user.DBId);
    Assert.AreEqual<string>((string)null, user.Title);
    Assert.AreEqual<string>((string)null, ((Person)user).Cpr);
    Assert.AreEqual<string>((string)null, ((Person)user).PassportNumber);
    Assert.AreEqual<string>("", ((Person)user).Name);
    Assert.AreEqual<string>("", ((Person)user).Address);
    Assert.AreEqual<string>("", ((Person)user).PlaceOfBirth);
    Assert.AreEqual<int>(0, ((Person)user).DbId);
}
    }
}
