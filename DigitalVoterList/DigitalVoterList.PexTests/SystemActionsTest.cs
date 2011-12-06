// <copyright file="SystemActionsTest.cs">Copyright ©  2011</copyright>

using System;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DigitalVoterList.Election;

[TestClass]
[PexClass(typeof(global::SystemActions))]
[PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
[PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
public partial class SystemActionsTest
{
    [PexMethod]
    public SystemAction getSystemAction(string name)
    {
        SystemAction result = global::SystemActions.getSystemAction(name);
        return result;
        // TODO: add assertions to method SystemActionsTest.getSystemAction(String)
    }
}
