using CalamityMod;
using CalamityMod.Items;
using CalRemix.Content.Cooldowns;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Tools
{
    public class Quadnoculars : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            DisplayName.SetDefault("Quadnoculars");
        }

        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 10;
            Item.useTime = 120;
            Item.useAnimation = 120;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.value = CalamityGlobalItem.RarityYellowBuyPrice;
            Item.rare = ItemRarityID.Yellow;
        }
        public override void UseItemFrame(Player player)
        {
            player.itemRotation = MathHelper.PiOver4 / 2f;
        }
        public override void HoldItem(Player player)
        {
            player.scope = true;
        }
        public override bool? UseItem(Player player)
        {
            if (player.scope && Main.myPlayer == player.whoAmI && !player.HasCooldown(QuadnocularsCooldown.ID))
            {
                NPC lastnpc = null;
                for (int i = 0; i < Main.npc.Length; i++)
                {
                    if (Main.npc[i].active)
                    {
                        NPC npc = Main.npc[i];
                        bool inBox = npc.Center.X >= Main.screenPosition.X && npc.Center.X <= (Main.screenPosition.X + Main.screenWidth) && npc.Center.Y >= Main.screenPosition.Y && npc.Center.Y <= (Main.screenPosition.Y + Main.screenHeight);
                        if (inBox && npc.life > 0 && !npc.friendly && !npc.townNPC && !npc.dontTakeDamage)
                        {
                            if (lastnpc == null)
                                lastnpc = npc;
                            else
                            {
                                if (npc.Distance(Main.MouseWorld) < lastnpc.Distance(Main.MouseWorld))
                                    lastnpc = npc;
                            }
                        }
                    }
                }
                if (lastnpc != null)
                {
                    SoundEngine.PlaySound(BetterSoundID.ItemInfernoExplosion);
                    lastnpc.AddBuff(BuffID.OnFire, 1800);
                    lastnpc.AddBuff(BuffID.CursedInferno, 1800);
                    lastnpc.AddBuff(BuffID.Frostburn, 1800);
                    lastnpc.AddBuff(BuffID.ShadowFlame, 1800);
                    player.AddCooldown(QuadnocularsCooldown.ID, CalamityUtils.SecondsToFrames(30));
                }
            }
            return true;
        }
    }
}
