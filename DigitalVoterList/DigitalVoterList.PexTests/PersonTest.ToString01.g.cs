// <copyright file="PersonTest.ToString01.g.cs">Copyright �  2011</copyright>
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
public void ToString01765()
{
    Person person;
    string s;
    person = new Person(0);
    person.Cpr = (string)null;
    person.PassportNumber = (string)null;
    s = this.ToString01(person);
    Assert.AreEqual<string>("PERSON( navn :  , cpr :  )", s);
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