// <copyright file="UserTest.ToString01.g.cs">Copyright �  2011</copyright>
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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Pex.Framework.Generated;

namespace DigitalVoterList.Election
{
    public partial class UserTest
    {
[TestMethod]
[PexGeneratedBy(typeof(UserTest))]
public void ToString01159()
{
    User user;
    string s;
    user = new User(0);
    user.Username = (string)null;
    user.UserSalt = (string)null;
    user.Valid = false;
    user.Title = (string)null;
    ((Person)user).Cpr = (string)null;
    ((Person)user).PassportNumber = (string)null;
    s = this.ToString01(user);
    Assert.AreEqual<string>("USER( username :  , title :  )", s);
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