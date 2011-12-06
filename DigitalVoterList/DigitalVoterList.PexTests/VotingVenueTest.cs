// <copyright file="VotingVenueTest.cs">Copyright ©  2011</copyright>

using System;
using DigitalVoterList.Election;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DigitalVoterList.Election
{
    [TestClass]
    [PexClass(typeof(VotingVenue))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    public partial class VotingVenueTest
    {
        [PexMethod]
        public bool op_Inequality(VotingVenue a, VotingVenue b)
        {
            bool result = a != b;
            return result;
            // TODO: add assertions to method VotingVenueTest.op_Inequality(VotingVenue, VotingVenue)
        }
        [PexMethod]
        public bool op_Equality(VotingVenue a, VotingVenue b)
        {
            bool result = a == b;
            return result;
            // TODO: add assertions to method VotingVenueTest.op_Equality(VotingVenue, VotingVenue)
        }
        [PexMethod]
        public VotingVenue Constructor(
            int dbid,
            string name,
            string address
        )
        {
            VotingVenue target = new VotingVenue(dbid, name, address);
            return target;
            // TODO: add assertions to method VotingVenueTest.Constructor(Int32, String, String)
        }
    }
}
