// -----------------------------------------------------------------------
// <copyright file="Barcode.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace DigitalVoterList
{

    /// <summary>
    /// "An optical, machine readable, representation of simple data, with the ability to output both the data and the optical representation."
    /// </summary>
    public class Barcode
    {
        private Image image;
        private string data;

        /// <summary>
        /// "May I have a new barcode object, representing this data?"
        /// </summary>
        /// <param name="data"></param>
        public Barcode(string data)
        {
            this.data = data;
        }

        /// <summary>
        /// "May I have a new barcode, that will produce this optical representation?"
        /// </summary>
        /// <param name="data"></param>
        public Barcode(Image data)
        {

        }

        /// <summary>
        /// "What data do you hold?"
        /// </summary>
        public string Data
        {
            get
            {
                return data;
            }
        }

        /// <summary>
        /// "Can I have an optical representation of the data you hold?"
        /// </summary>
        public string Image
        {
            get
            {
                return image;
            }
        }

        public class Image
        {
        }
    }
}
