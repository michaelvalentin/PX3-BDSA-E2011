// <copyright file="BarcodeTest.cs">Copyright ©  2011</copyright>

using System;
using DigitalVoterList.Utilities;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DigitalVoterList.Utilities
{
    [TestClass]
    [PexClass(typeof(Barcode))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    public partial class BarcodeTest
    {
        [PexMethod]
        public Barcode Constructor(string data)
        {
            Barcode target = new Barcode(data);
            return target;
            // TODO: add assertions to method BarcodeTest.Constructor(String)
        }
    }
}
