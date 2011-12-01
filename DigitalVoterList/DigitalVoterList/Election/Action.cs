namespace DigitalVoterList.Election
{
    public enum Action
    {
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

        //UPDATE
        SavePerson,
        SaveUser,
        SaveVoterCard,
        SetHasVoted,
        SetHasVotedManually,
        ChangeOwnPassword,
        ChangeOthersPassword,

        //DELETE
        MarkUserInvalid,
        RestoreUser,
        MarkVoteCardInvalid
    }
}
