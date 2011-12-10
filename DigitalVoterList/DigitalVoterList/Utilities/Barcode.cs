using System.Diagnostics.Contracts;
using System.Drawing;
using System.Text.RegularExpressions;

namespace DigitalVoterList.Utilities
{

    /// <summary>
    /// An optical, machine readable, representation of simple data, with the ability to output both the data and the optical representation.
    /// </summary>
    public class Barcode
    {
        private string _data;
        private Bitmap _image;

        /// <summary>
        /// May i have a new barcode object, representing this data?
        /// </summary>
        /// <param name="data">The data to be represented</param>
        public Barcode(string data)
        {
            Contract.Requires(!string.IsNullOrEmpty(data));
            Contract.Requires(data.Length <= 10);
            Contract.Requires(IsBarcodeReady(data));
            _data = data;
            _image = GenerateBarcode(_data);
        }

        /// <summary>
        /// What data do you hold?
        /// </summary>
        public string Data
        {
            get { return _data; }
        }

        /// <summary>
        /// Can i have a optical representation of the data you hold?
        /// </summary>
        public Bitmap Image
        {
            get { return _image; }
        }

        private Bitmap GenerateBarcode(string data)
        {
            throw new System.NotImplementedException();
            //http://www.techrepublic.com/blog/howdoi/how-do-i-generate-barcodes-using-c/173
        }

        private bool IsBarcodeReady(string data)
        {
            Regex barcode = new Regex("[0-9A-Z]");
            return barcode.IsMatch(data);
        }

        [ContractInvariantMethod]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required for code contracts.")]
        private void ObjectInvariant()
        {
            Contract.Invariant(_data.Length <= 10);
            Contract.Invariant(IsBarcodeReady(_data));
        }
    }
}
