// <copyright file="DAOPermissionProxyTest.Find02.g.cs">Copyright �  2011</copyright>
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
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Pex.Framework.Generated;

namespace DigitalVoterList.Election
{
    public partial class DAOPermissionProxyTest
    {
[TestMethod]
[PexGeneratedBy(typeof(DAOPermissionProxyTest))]
[PexRaisedException(typeof(PermissionException))]
public void Find02ThrowsPermissionException547()
{
    User user;
    DAOPermissionProxy dAOPermissionProxy;
    List<VoterCard> list;
    user = new User(0);
    user.Username = (string)null;
    user.UserSalt = (string)null;
    user.Valid = false;
    user.Title = (string)null;
    ((Person)user).Cpr = (string)null;
    ((Person)user).PassportNumber = (string)null;
    dAOPermissionProxy = new DAOPermissionProxy(user, (IDataAccessObject)null);
    list = this.Find02(dAOPermissionProxy, (VoterCard)null);
}
[TestMethod]
[PexGeneratedBy(typeof(DAOPermissionProxyTest))]
[PexRaisedException(typeof(NullReferenceException))]
public void Find02ThrowsNullReferenceException211()
{
    DAOPermissionProxy dAOPermissionProxy;
    List<VoterCard> list;
    dAOPermissionProxy = new DAOPermissionProxy((User)null, (IDataAccessObject)null);
    list = this.Find02(dAOPermissionProxy, (VoterCard)null);
}
    }
}
