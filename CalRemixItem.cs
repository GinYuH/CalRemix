using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.GameContent.Generation;
using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;
using CalamityMod.NPCs.TownNPCs;
using Microsoft.Xna.Framework;
using Terraria.GameContent.ItemDropRules;
using CalamityMod.Items.PermanentBoosters;
using CalamityMod;

namespace CalRemix
{
	public class CalRemixItem : GlobalItem
	{
        public override void UpdateInventory(Item item, Player player)
        {
            if (item.type == ModContent.ItemType<Elderberry>() && item.stack > 1)
            {
                item.stack = 1;
            }
        }
        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            CalRemixPlayer modPlayer = player.GetModPlayer<CalRemixPlayer>();
            if (modPlayer.roguebox && item.CountsAsClass<RogueDamageClass>())
            {
                int p = Projectile.NewProjectile(source, new Vector2(Main.MouseWorld.X, Main.MouseWorld.Y - 400), new Vector2(0, 20), type, (int)(damage * 0.33f), knockback, player.whoAmI);
                {
                    Main.projectile[p].GetGlobalProjectile<CalRemixProjectile>().rogueclone = true;
                    if (p.WithinBounds(Main.maxProjectiles))
                        Main.projectile[p].originalDamage = (int)(damage * 0.33f);
                }
            }
            return true;
        }

        public override void Update(Item item, ref float gravity, ref float maxFallSpeed)
        {
            if (item.type == ItemID.Apple)
            {
                if (item.wet && !item.lavaWet && Main.bloodMoon && NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3)
                {
                    item.type = ModContent.ItemType<BloodOrange>();
                }
            }
            if (item.type == ModContent.ItemType<Elderberry>() && item.stack > 1)
            {
                item.stack = 1;
            }
        }
        public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
        {
            if (item.type == ItemID.FloatingIslandFishingCrate || item.type == ItemID.FloatingIslandFishingCrateHard)
            {
                itemLoot.AddIf(() => NPC.AnyNPCs(NPCID.WyvernHead) && CalamityMod.DownedBossSystem.downedYharon && !Main.LocalPlayer.Calamity().dFruit, ModContent.ItemType<Dragonfruit>(), 1);
                itemLoot.AddIf(() => NPC.AnyNPCs(NPCID.WyvernHead) && CalamityMod.DownedBossSystem.downedYharon && Main.LocalPlayer.Calamity().dFruit, ModContent.ItemType<Dragonfruit>(), 20);
            }
        }
    }
}
