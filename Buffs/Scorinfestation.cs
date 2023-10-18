﻿using CalamityMod;
using CalamityMod.CalPlayer;
using CalamityMod.Items.Accessories.Wings;
using CalamityMod.Items.Armor.Hydrothermic;
using CalamityMod.Items.Fishing.FishingRods;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables;
using CalamityMod.Items.Placeables.Ores;
using CalamityMod.Items.Placeables.Walls;
using CalamityMod.Items.Tools;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Items.Weapons.Rogue;
using CalRemix.NPCs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Buffs
{
    public class Scorinfestation : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Scorinfestation");
            Description.SetDefault("Consumed by magma");
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (!NPC.AnyNPCs(ModContent.NPCType<LaRuga>()))
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
            if (player.buffTime[buffIndex] % 120 == 0 && Main.rand.NextBool(2))
            {
                ScoriaDestruction(player);
            }
        }

        public static void ScoriaDestruction(Player player)
        {
            int slotToDestroy = Main.rand.Next(0, player.inventory.Length);
            Item victim = player.inventory[slotToDestroy];
            int stack = victim.stack;
            if (victim.DamageType == DamageClass.Melee && victim.useStyle == ItemUseStyleID.Swing)
                victim.SetDefaults(ModContent.ItemType<HellfireFlamberge>());
            else if (victim.DamageType == DamageClass.Melee && victim.useStyle != ItemUseStyleID.Swing)
                victim.SetDefaults(ModContent.ItemType<VulcaniteLance>());
            else if (victim.DamageType == DamageClass.Ranged && victim.useAmmo == AmmoID.Arrow)
                victim.SetDefaults(ModContent.ItemType<ContinentalGreatbow>());
            else if (victim.DamageType == DamageClass.Ranged && victim.ammo <= 0)
                victim.SetDefaults(ModContent.ItemType<Helstorm>());
            else if (victim.DamageType == DamageClass.Magic)
                victim.SetDefaults(ModContent.ItemType<ForbiddenSun>());
            else if (victim.DamageType == ModContent.GetInstance<RogueDamageClass>())
                victim.SetDefaults(ModContent.ItemType<SubductionSlicer>());
            else if (victim.damage > 0)
                victim.SetDefaults(ModContent.ItemType<FaultLine>());
            else if (victim.createTile > -1 && victim.type != ModContent.ItemType<ScoriaBar>())
                victim.SetDefaults(ModContent.ItemType<ScoriaBrick>());
            else if (victim.createWall > -1)
                victim.SetDefaults(ModContent.ItemType<ScoriaBrickWall>());
            else if (victim.pick > 0 || victim.hammer > 0)
                victim.SetDefaults(ModContent.ItemType<SeismicHampick>());
            else if (victim.axe > 0)
                victim.SetDefaults(ModContent.ItemType<TectonicTruncator>());
            else if (victim.fishingPole > 0)
                victim.SetDefaults(ModContent.ItemType<RiftReeler>());
            else if (victim.accessory)
                victim.SetDefaults(ModContent.ItemType<HadalMantle>());
            else if (victim.defense > 0)
                victim.SetDefaults(ModContent.ItemType<HydrothermicArmor>());
            else if (victim.potion)
                victim.SetDefaults(ModContent.ItemType<ScoriaOre>());
            else
                victim.SetDefaults(ModContent.ItemType<ScoriaBar>());
            victim.stack = stack;
        }
    }
}