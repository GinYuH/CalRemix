using System;
using CalRemix.Core.World;

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
            float value = (CalRemixWorld.musicPitch || Main.gameMenu) ? hilariousPitchMultiplier : 0;
            foreach (var track in self.AudioTracks)
            {
                if (track is ASoundEffectBasedAudioTrack)
                {
                    track.SetVariable("Pitch", value);
                }

                // MonoStereo support!!!  This mod reimplements Cue support for
                // vanilla tracks and lets us modify the pitch as well.
                if (track?.GetType().FullName?.Equals("MonoStereoMod.MonoStereoAudioTrack") ?? false)
                {
                    track.SetVariable("Pitch", value);
                }
            }

            orig(self);
        };

        var essentiallyUniqueId = SocialAPI.Mode == SocialMode.Steam
            ? Steamworks.SteamUser.GetSteamID().m_SteamID
            : GetConsistentHash(Environment.UserName);

        // vary by 10% (90% to 110%)
        hilariousPitchMultiplier = 0.1f * (essentiallyUniqueId % 100 / 100f);

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
}