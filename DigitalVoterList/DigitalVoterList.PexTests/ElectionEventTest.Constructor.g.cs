// <copyright file="ElectionEventTest.Constructor.g.cs">Copyright �  2011</copyright>
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
    public partial class ElectionEventTest
    {
[TestMethod]
[PexGeneratedBy(typeof(ElectionEventTest))]
public void Constructor25()
{
    ElectionEvent electionEvent;
    electionEvent = this.Constructor(default(DateTime), (string)null);
    Assert.IsNotNull((object)electionEvent);
    Assert.AreEqual<int>(1, electionEvent.Date.Day);
    Assert.AreEqual<DayOfWeek>(DayOfWeek.Monday, electionEvent.Date.DayOfWeek);
    Assert.AreEqual<int>(1, electionEvent.Date.DayOfYear);
    Assert.AreEqual<int>(0, electionEvent.Date.Hour);
    Assert.AreEqual<DateTimeKind>(DateTimeKind.Unspecified, electionEvent.Date.Kind);
    Assert.AreEqual<int>(0, electionEvent.Date.Millisecond);
    Assert.AreEqual<int>(0, electionEvent.Date.Minute);
    Assert.AreEqual<int>(1, electionEvent.Date.Month);
    Assert.AreEqual<int>(0, electionEvent.Date.Second);
    Assert.AreEqual<int>(1, electionEvent.Date.Year);
    Assert.AreEqual<string>((string)null, electionEvent.Name);
}
    }
}
