
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using CalamityMod;
using CalamityMod.Particles;
using Terraria.ModLoader.IO;

namespace CalRemix
{
	public class CalRemixPlayer : ModPlayer
	{
		public bool brimPortal;
		public bool arcanumHands;
		public bool marnite;
		public bool roguebox;
		public int eclipseaura = -1;
		public int marnitetimer = 1200;
		public Particle ring;
		public Particle ring2;
		public Particle aura;

		public override void ProcessTriggers(TriggersSet triggersSet)
		{
			if (CalamityMod.CalamityKeybinds.SpectralVeilHotKey.JustPressed && roguebox)
			{
				if (!Player.HasCooldown(EclipseAuraCooldown.ID) && Player.GetModPlayer<CalRemixPlayer>().eclipseaura <= -1)
				{
					Player.GetModPlayer<CalRemixPlayer>().eclipseaura = 300;
				}
			}
		}

        public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource, ref int cooldownCounter)
        {
            if (eclipseaura > 0)
            {
				return false;
            }
			return true;
        }

        public override void PostUpdateMiscEffects()
		{
			if (ring2 != null)
			{
				ring2.Position = Player.Center;
				ring2.Velocity = Player.velocity;
				ring2.Scale *= 1.05f;
				ring2.Time += 1;
			}
			if (eclipseaura != -1)
            {
				aura = new StrongBloom(Player.Center, Player.velocity, Color.Purple * 1.1f, 1f + Main.rand.NextFloat(0f, 1.5f) * 1.5f, 40);
				ring = new BloomRing(Player.Center, Player.velocity, Color.Yellow * 1.2f, 1.5f, 40);
				if (ring != null)
				{
					ring.Position = Player.Center;
					ring.Velocity = Player.velocity;
					ring.Time = 0;
				}
				if (aura != null)
				{
					aura.Position = Player.Center;
					aura.Velocity = Player.velocity;
					aura.Time = 0;
				}
				for (int i = 0; i < Main.maxNPCs; i++)
                {
					NPC target = Main.npc[i];
					if (Player.Center.Distance(target.Center) < 100 && !target.friendly)
                    {
						target.StrikeNPC(200, 0, 0);
                    }
                }
				if (eclipseaura % 10 == 0)
				{
					GeneralParticleHandler.SpawnParticle(aura);
					GeneralParticleHandler.SpawnParticle(ring);
				}
				eclipseaura--;
            }
			else
			{
				if (ring != null)
				{
					ring.Kill();
				}
				if (aura != null)
				{
					aura.Kill();
				}
			}
			if (eclipseaura == 0)
			{
				Main.LocalPlayer.AddCooldown(EclipseAuraCooldown.ID, CalamityUtils.SecondsToFrames(20));
			}
        }
        public override void ResetEffects()
		{
			brimPortal = false;
			arcanumHands = false;
			marnite = false;
			roguebox = false;
			marnitetimer = 0;
		}
    }
}