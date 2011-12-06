// <copyright file="PersonTest.cs">Copyright ©  2011</copyright>

using System;
using DigitalVoterList.Election;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DigitalVoterList.Election
{
    [TestClass]
    [PexClass(typeof(Person))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    public partial class PersonTest
    {
        [PexMethod]
        public string ToString01([PexAssumeUnderTest]Person target)
        {
            string result = target.ToString();
            return result;
            // TODO: add assertions to method PersonTest.ToString01(Person)
        }
        [PexMethod]
        public Person Constructor01()
        {
            Person target = new Person();
            return target;
            // TODO: add assertions to method PersonTest.Constructor01()
        }
        [PexMethod]
        public Person Constructor(int id)
        {
            Person target = new Person(id);
            return target;
            // TODO: add assertions to method PersonTest.Constructor(Int32)
        }
    }
}
