// <copyright file="ManualRegistrationControllerTest.Constructor.g.cs">Copyright �  2011</copyright>
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
using DigitalVoterList.Views;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Pex.Framework.Generated;

namespace DigitalVoterList.Controllers
{
    public partial class ManualRegistrationControllerTest
    {
[TestMethod]
[PexGeneratedBy(typeof(ManualRegistrationControllerTest))]
public void Constructor89()
{
    ManualRegistrationController manualRegistrationController;
    manualRegistrationController = this.Constructor((ManualRegistrationView)null);
    Assert.IsNotNull((object)manualRegistrationController);
    Assert.IsNull(((ContentController)manualRegistrationController).View);
}
    }
}