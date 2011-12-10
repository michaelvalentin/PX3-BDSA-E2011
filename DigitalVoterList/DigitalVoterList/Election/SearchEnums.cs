// -----------------------------------------------------------------------
// <copyright file="CitizenParams.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace DigitalVoterList.Election
{

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public enum CitizenSearchParam
    {
        Name, Cpr, Address, HasVoted, EligibleToVote, VotingPlace
    }

    public enum UserSearchParam
    {
        Name, Cpr, Address, HasVoted, EligibleToVote, VotingPlace
    }

    public enum VoterCardSearchParam
    {
        IdKey, CitizenId, Valid
    }
}
