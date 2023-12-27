using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CalRemix.ElementalSystem
{
	public class ElementDisplay : InfoDisplay
	{
		public override bool Active() 
		{
			return Main.LocalPlayer.GetModPlayer<ElementPlayer>().EDisplay;
		}
		public override string DisplayValue(ref Color displayColor, ref Color displayShadowColor)
		{
			NPC npc = Main.LocalPlayer.GetModPlayer<ElementPlayer>().ETrack;
			if (npc is null)
			{
				displayColor = InactiveInfoTextColor;
                return "No entities tracked";
            }
			if (npc.GetGlobalNPC<ElementNPC>().weak is null && npc.GetGlobalNPC<ElementNPC>().resist is null)
            {
                displayColor = InactiveInfoTextColor;
                return "No elemental info available";
            }
            string s = string.Empty;
            if (npc.GetGlobalNPC<ElementNPC>().weak != null)
			{
				s += "Weak to";
				for (int i = 0; i < npc.GetGlobalNPC<ElementNPC>().weak.Length; i++)
				{
					s += $" {npc.GetGlobalNPC<ElementNPC>().weak[i]}";
				}
            }
            if (npc.GetGlobalNPC<ElementNPC>().resist != null)
            {
                s += "\nResists";
				for (int i = 0; i < npc.GetGlobalNPC<ElementNPC>().resist.Length; i++)
				{
					s += $" {npc.GetGlobalNPC<ElementNPC>().resist[i]}";
				}
            }
			return s;
		}
	}
	public class ElementPlayer : ModPlayer
	{
		public bool EDisplay;
		public NPC ETrack;
		public override void ResetEffects() 
		{
            EDisplay = false;
		}
		public override void UpdateEquips() 
		{
            EDisplay = true;
		}
		public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            ETrack = target;
        }
		public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
		{
			ETrack = target;
		}
	}
}
