// <copyright file="ElectionEventTest.cs">Copyright ©  2011</copyright>

using System;
using DigitalVoterList.Election;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DigitalVoterList.Election
{
    [TestClass]
    [PexClass(typeof(ElectionEvent))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    public partial class ElectionEventTest
    {
        [PexMethod]
        public VotingVenue VotingVenueForCitizen([PexAssumeUnderTest]ElectionEvent target, Citizen citizen)
        {
            VotingVenue result = target.VotingVenueForCitizen(citizen);
            return result;
            // TODO: add assertions to method ElectionEventTest.VotingVenueForCitizen(ElectionEvent, Citizen)
        }
        [PexMethod]
        public void NameSet([PexAssumeUnderTest]ElectionEvent target, string value)
        {
            target.Name = value;
            // TODO: add assertions to method ElectionEventTest.NameSet(ElectionEvent, String)
        }
        [PexMethod]
        public string NameGet([PexAssumeUnderTest]ElectionEvent target)
        {
            string result = target.Name;
            return result;
            // TODO: add assertions to method ElectionEventTest.NameGet(ElectionEvent)
        }
        [PexMethod]
        public void DateSet([PexAssumeUnderTest]ElectionEvent target, DateTime value)
        {
            target.Date = value;
            // TODO: add assertions to method ElectionEventTest.DateSet(ElectionEvent, DateTime)
        }
        [PexMethod]
        public DateTime DateGet([PexAssumeUnderTest]ElectionEvent target)
        {
            DateTime result = target.Date;
            return result;
            // TODO: add assertions to method ElectionEventTest.DateGet(ElectionEvent)
        }
        [PexMethod]
        public ElectionEvent Constructor(DateTime date, string name)
        {
            ElectionEvent target = new ElectionEvent(date, name);
            return target;
            // TODO: add assertions to method ElectionEventTest.Constructor(DateTime, String)
        }
    }
}
