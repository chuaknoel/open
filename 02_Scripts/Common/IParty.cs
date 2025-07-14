using Enums;

public interface IParty
{
    PartySystem MyParty { get;}
    bool IsLeader { get;}
    PartyRole PartyRole { get;}

    void ConnectParty(PartySystem partySystem);
}
