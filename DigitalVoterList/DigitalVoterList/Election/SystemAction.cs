using System;
using DigitalVoterList.Election;

namespace DigitalVoterList.Election
{
    public enum SystemAction
    {
        Nothing,
        AllVotingPlaces,

        //CREATE
        CreateUser,
        CreatePerson,
        CreateVoterCard,

        //READ
        LoadPerson,
        LoadUser,
        LoadVoterCard,
        ScanVoterCard,

        //SEARCH
        FindPerson,
        FindUser,
        FindVoterCard,
        FindElegibleVoters,
        FindVotingVenue,

        //UPDATE
        SavePerson,
        SaveUser,
        SaveVoterCard,
        SetHasVoted,
        SetHasVotedManually,
        ChangeOwnPassword,
        ChangeOthersPassword,
        MarkPeopleNotInRawDataUneligibleToVote,
        UpdatePeople,

        //DELETE
        MarkUserInvalid,
        RestoreUser,
        MarkVoteCardInvalid,
    }
}

public static class SystemActions
{
    public static SystemAction getSystemAction(string name)
    {
        SystemAction output;
        return Enum.TryParse(name, true, out output) ? output : SystemAction.Nothing;
    }
}
