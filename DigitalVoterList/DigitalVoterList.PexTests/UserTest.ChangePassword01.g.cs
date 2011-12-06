// <copyright file="UserTest.ChangePassword01.g.cs">Copyright �  2011</copyright>
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
[PexRaisedException(typeof(PermissionException))]
public void ChangePassword01ThrowsPermissionException58()
{
    User user;
    user = new User(0);
    user.Username = (string)null;
    user.UserSalt = (string)null;
    user.Valid = false;
    user.Title = (string)null;
    ((Person)user).Cpr = (string)null;
    ((Person)user).PassportNumber = (string)null;
    this.ChangePassword01(user, (string)null);
}
[TestMethod]
[PexGeneratedBy(typeof(UserTest))]
[PexRaisedException(typeof(PermissionException))]
public void ChangePasswordThrowsPermissionException163()
{
    User user;
    user = new User(0);
    user.Username = (string)null;
    user.UserSalt = "";
    user.Valid = false;
    user.Title = (string)null;
    ((Person)user).Cpr = (string)null;
    ((Person)user).PassportNumber = (string)null;
    this.ChangePassword(user, (string)null, "");
}
[TestMethod]
[PexGeneratedBy(typeof(UserTest))]
[PexRaisedException(typeof(PermissionException))]
public void ChangePasswordThrowsPermissionException658()
{
    User user;
    user = new User(0);
    user.Username = (string)null;
    user.UserSalt = "";
    user.Valid = false;
    user.Title = (string)null;
    ((Person)user).Cpr = (string)null;
    ((Person)user).PassportNumber = (string)null;
    this.ChangePassword(user, (string)null, (string)null);
}
[TestMethod]
[PexGeneratedBy(typeof(UserTest))]
[PexRaisedException(typeof(PermissionException))]
public void ChangePasswordThrowsPermissionException341()
{
    User user;
    user = new User(0);
    user.Username = (string)null;
    user.UserSalt = (string)null;
    user.Valid = false;
    user.Title = (string)null;
    ((Person)user).Cpr = (string)null;
    ((Person)user).PassportNumber = (string)null;
    this.ChangePassword(user, (string)null, (string)null);
}
    }
}