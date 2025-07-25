﻿namespace BossMod.Shadowbringers.Foray.DelubrumReginae.DRS2StygimolochWarrior;

class FocusedTremorLarge(BossModule module) : Components.StandardAOEs(module, AID.FocusedTremorAOELarge, new AOEShapeRect(20, 10), 2);
class ForcefulStrike(BossModule module) : Components.StandardAOEs(module, AID.ForcefulStrike, new AOEShapeRect(44, 24));

// combined with flailing strike, first bait should be into first square
class FocusedTremorSmall : Components.StandardAOEs
{
    public FocusedTremorSmall(BossModule module) : base(module, AID.FocusedTremorAOESmall, new AOEShapeRect(10, 5), 1)
    {
        Color = ArenaColor.SafeFromAOE;
        Risky = false;
    }

    public void Activate()
    {
        Color = ArenaColor.AOE;
        Risky = true;
        MaxCasts = 3;
    }
}

class FlailingStrikeBait(BossModule module) : Components.BaitAwayTethers(module, new AOEShapeCone(40, 30.Degrees()), (uint)TetherID.FlailingStrike);

class FlailingStrike(BossModule module) : Components.GenericRotatingAOE(module)
{
    private static readonly AOEShapeCone _shape = new(60, 30.Degrees());

    public override void OnCastStarted(Actor caster, ActorCastInfo spell)
    {
        if ((AID)spell.Action.ID == AID.FlailingStrikeFirst)
        {
            Sequences.Add(new(_shape, caster.Position, spell.Rotation, 60.Degrees(), Module.CastFinishAt(spell), 1.6f, 6, 3));
        }
    }

    public override void OnEventCast(Actor caster, ActorCastEvent spell)
    {
        if ((AID)spell.Action.ID == AID.FlailingStrikeRest && Sequences.Count > 0)
        {
            AdvanceSequence(0, WorldState.CurrentTime);
        }
    }
}
