// <copyright file="PersonTest.Constructor01.g.cs">Copyright �  2011</copyright>
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
    public partial class PersonTest
    {
[TestMethod]
[PexGeneratedBy(typeof(PersonTest))]
public void Constructor01583()
{
    Person person;
    person = this.Constructor01();
    Assert.IsNotNull((object)person);
    Assert.AreEqual<string>((string)null, person.Cpr);
    Assert.AreEqual<string>((string)null, person.PassportNumber);
    Assert.AreEqual<string>("", person.Name);
    Assert.AreEqual<string>("", person.Address);
    Assert.AreEqual<string>("", person.PlaceOfBirth);
    Assert.AreEqual<int>(0, person.DbId);
}
[TestMethod]
[PexGeneratedBy(typeof(PersonTest))]
public void Constructor687()
{
    Person person;
    person = this.Constructor(0);
    Assert.IsNotNull((object)person);
    Assert.AreEqual<string>((string)null, person.Cpr);
    Assert.AreEqual<string>((string)null, person.PassportNumber);
    Assert.AreEqual<string>("", person.Name);
    Assert.AreEqual<string>("", person.Address);
    Assert.AreEqual<string>("", person.PlaceOfBirth);
    Assert.AreEqual<int>(0, person.DbId);
}
    }
}
