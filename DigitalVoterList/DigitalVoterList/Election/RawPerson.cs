﻿// -----------------------------------------------------------------------
// <copyright file="RawPerson.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace DigitalVoterList.Election
{
    using System;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class RawPerson
    {
        public string CPR { get; set; }
        public string Address { get; set; }
        public string AddressPrevious { get; set; }
        public DateTime? Birthday { get; set; }
        public string Birthplace { get; set; }
        public DateTime? Deathdate { get; set; }
        public int Age { get; set; }
        public string MotherName { get; set; }
        public string FatherName { get; set; }
        public string Education { get; set; }
        public string Workplace { get; set; }
        public bool MilitaryServed { get; set; }
        public string DriverID { get; set; }
        public string TelephoneNumber { get; set; }

        public string PassportNumber { get; set; }
        public string City { get; set; }
        public int PostNumber { get; set; }
        public string Nationality { get; set; }
        public bool Disempowered { get; set; }
}
