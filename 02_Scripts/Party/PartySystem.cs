using System;
using System.Collections.Generic;

public class PartySystem
{
    public IParty leader;
    public List<IParty> members= new List<IParty>();

    public PartySystem(BaseCreature partyLeader)
    {
        if (partyLeader.TryGetComponent<IParty>(out IParty leader))
        {
            this.leader = leader;
            leader.ConnectParty(this);
            ApplySynergy();
        }
    }

    public void AddMember(BaseCreature partyMember)
    {
        if (partyMember.TryGetComponent<IParty>(out IParty member))
        {
            if (members.Contains(member)) return;

            members.Add(member);
            ApplySynergy();
        }
    }

    private void ApplySynergy()
    {
        // 동료 직업/속성 기반 시너지 효과
    }
}
