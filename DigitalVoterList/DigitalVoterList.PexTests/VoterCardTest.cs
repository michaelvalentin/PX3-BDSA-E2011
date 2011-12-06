// <copyright file="VoterCardTest.cs">Copyright ©  2011</copyright>

using System;
using DigitalVoterList.Election;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DigitalVoterList.Election
{
    [TestClass]
    [PexClass(typeof(VoterCard))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    public partial class VoterCardTest
    {
        [PexMethod]
        public bool MarkAsInvalid([PexAssumeUnderTest]VoterCard target)
        {
            bool result = target.MarkAsInvalid();
            return result;
            // TODO: add assertions to method VoterCardTest.MarkAsInvalid(VoterCard)
        }
        [PexMethod]
        public VoterCard Constructor(ElectionEvent electionEvent, Citizen citizen)
        {
            VoterCard target = new VoterCard(electionEvent, citizen);
            return target;
            // TODO: add assertions to method VoterCardTest.Constructor(ElectionEvent, Citizen)
        }
    }
}
