using CalamityMod;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace CalRemix.Content.Items.Weapons
{
    public class ShardofGlass : ModItem
    {
        public int durability = 10;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shard of Glass");
            Tooltip.SetDefault("Durability");
        }

        public override void SetDefaults()
        {
            Item.width = 70;
            Item.damage = 1035;
            Item.DamageType = DamageClass.Melee;
            Item.useAnimation = 22;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 22;
            Item.useTurn = true;
            Item.knockBack = 3f;
            Item.UseSound = BetterSoundID.ItemSwing;
            Item.autoReuse = true;
            Item.height = 72;
            Item.value = Item.buyPrice(100);
            Item.rare = RarityHelper.Oxygen;
        }

        public override void UseItemFrame(Player player)
        {
            player.itemLocation = (Vector2)player.HandPosition;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            durability--;
            if (durability <= 0)
            {
                SoundEngine.PlaySound(SoundID.Shatter with { Volume = 2 }, player.position);
                for (int i = 0; i < 30; i++)
                {
                    Gore.NewGore(player.GetSource_FromThis(), target.Center, Main.rand.NextVector2Circular(10, 10).SafeNormalize(Vector2.UnitY) * Main.rand.Next(4, 8), Mod.Find<ModGore>("GlassShard" + Main.rand.Next(1, 5)).Type);
                }
                player.HeldItem.SetDefaults();
            }
            else
            {
                SoundEngine.PlaySound(SoundID.Shatter with { Volume = 0.7f, Pitch = 1 }, player.position);

                for (int i = 0; i < 2; i++)
                    Gore.NewGore(player.GetSource_FromThis(), target.Center, Main.rand.NextVector2Circular(10, 10).SafeNormalize(Vector2.UnitY) * Main.rand.Next(4, 8), Mod.Find<ModGore>("GlassShard" + Main.rand.Next(1, 5)).Type);
            }
        }

        public override void NetSend(BinaryWriter writer)
        {
            writer.Write(durability);
        }

        public override void NetReceive(BinaryReader reader)
        {
            durability = reader.ReadInt32();
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips) =>  tooltips.FindAndReplace("Durability", "Current Durability: " + durability);

        public override void SaveData(TagCompound tag)
        {
            tag.Add("durability", durability);
        }

        public override void LoadData(TagCompound tag)
        {
            durability = tag.GetInt("durability");
        }
    }
}