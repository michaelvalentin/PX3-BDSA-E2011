// -----------------------------------------------------------------------
// <copyright file="RandomStringGenerator.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace DigitalVoterList.Utilities
{
    using System;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class RandomStringGenerator
    {
        private static readonly Random random = new Random((int)DateTime.Now.Ticks);
        public static string RandomString(int size)
        {
            var builder = new StringBuilder();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }

            return builder.ToString();
        }

    }
}
