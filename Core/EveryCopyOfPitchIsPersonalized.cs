using System;
using System.Reflection;

using Microsoft.Xna.Framework.Audio;

using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.Social;

namespace CalRemix.Core;

public class EveryCopyOfPitchIsPersonalized : ModSystem
{
    private static float hilariousPitchMultiplier = 1f;

    public override void Load()
    {
        On_LegacyAudioSystem.Update += (orig, self) =>
        {
            foreach (var track in self.AudioTracks)
            {
                if (track is not ASoundEffectBasedAudioTrack)
                {
                    continue;
                }

                track.SetVariable("Pitch", hilariousPitchMultiplier);
            }

            orig(self);
        };

        var essentiallyUniqueId = SocialAPI.Mode == SocialMode.Steam
            ? Steamworks.SteamUser.GetSteamID().m_SteamID
            : GetConsistentHash(Environment.UserName);

        // vary by 10% (90% to 110%)
        hilariousPitchMultiplier = 1f + 0.1f * (essentiallyUniqueId % 100 / 100f);

        // for testing
        hilariousPitchMultiplier = -1f;

        return;

        static ulong GetConsistentHash(string value)
        {
            const ulong fnv_offset_basis = 14695981039346656037UL;
            const ulong fnv_prime = 1099511628211UL;

            var hash = fnv_offset_basis;

            foreach (var b in value)
            {
                hash ^= b;
                hash *= fnv_prime;
            }

            return hash;
        }
    }

    /*public override void PostSetupContent()
    {
        base.PostSetupContent();

        if (Main.audioSystem is not LegacyAudioSystem audioSystem)
        {
            return;
        }

        foreach (var track in audioSystem.AudioTracks)
        {
            if (track is not null)
            {
                track.SetVariable("Pitch", hilariousPitchMultiplier);
            }
        }
    }*/

    /*private static void UseModifiedPitch(ILContext il)
    {
        var c = new ILCursor(il);

        c.GotoNext(MoveType.After, x => x.MatchLdfld<SoundEffectInstance>("INTERNAL_pitch"));
        c.EmitLdsfld(
            typeof(EveryCopyOfPitchIsPersonalized).GetField("hilariousPitchMultiplier", BindingFlags.Static | BindingFlags.NonPublic)!
        );
        c.EmitAdd();
    }*/

    public void Unload() { }
}