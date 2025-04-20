using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Weapons.Ranged;
using CalRemix.Content.DamageClasses;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace CalRemix.Content.Items.Weapons.Stormbow
{
    public class StormbowRework : GlobalItem
    {
        public override void SetDefaults(Item item)
        {
            string key = "Rename.Items.Stormbow.";
            if (item.type == ItemID.DaedalusStormbow)
            {
                item.SetNameOverride(CalRemixHelper.LocalText($"{key}DaedalusStormbow").Value);
            }
            else if (item.type == ItemID.Starfury)
            {
                item.SetNameOverride(CalRemixHelper.LocalText($"{key}Starfury").Value);
                item.DamageType = GetInstance<StormbowDamageClass>();
                item.mana = 5;
            }
            else if (item.type == ItemID.StarWrath)
            {
                item.SetNameOverride(CalRemixHelper.LocalText($"{key}StarWrath").Value);
                item.DamageType = GetInstance<StormbowDamageClass>();
                item.mana = 27;
            }
            else if (item.type == ItemID.BloodRainBow)
            {
                item.SetNameOverride(CalRemixHelper.LocalText($"{key}BloodRainBow").Value);
                item.DamageType = GetInstance<StormbowDamageClass>();
            }
            else if (item.type == ItemID.MeteorStaff)
            {
                item.DamageType = GetInstance<StormbowDamageClass>();
                item.mana = 0;
            }
            else if (item.type == ItemID.BlizzardStaff)
            {
                item.DamageType = GetInstance<StormbowDamageClass>();
                item.mana = 0;
            }
            else if (item.type == ItemID.LunarFlareBook)
            {
                item.DamageType = GetInstance<StormbowDamageClass>();
                item.mana = 0;
            }
            else if (item.type == ItemType<TheBurningSky>())
            {
                item.DamageType = GetInstance<StormbowDamageClass>();
            }
            else if (item.type == ItemType<StarShower>())
            {
                item.SetNameOverride(CalRemixHelper.LocalText($"{key}StarShower").Value);
                item.DamageType = GetInstance<StormbowDamageClass>();
            }
            else if (item.type == ItemType<ArterialAssault>())
            {
                item.DamageType = GetInstance<StormbowDamageClass>();
            }
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            string key = "Items.Tooltips.";
            if (item.DamageType == GetInstance<StormbowDamageClass>() && item.damage < 30 && item.rare <= ItemRarityID.Blue)
            {
                if (tooltips.Exists((TooltipLine t) => t.Name.Equals("Tooltip0")))
                {
                    TooltipLine line = tooltips.Find((TooltipLine t) => t.Name.Equals("Tooltip0"));
                    if (string.IsNullOrWhiteSpace(line.Text))
                    {
                        TooltipLine tip = new(Mod, "CalRemix:Stormbow", CalRemixHelper.LocalText($"{key}StormbowTip").Value);
                        tooltips.Add(tip);
                    }
                }
                else
                {
                    TooltipLine tip = new(Mod, "CalRemix:Stormbow", CalRemixHelper.LocalText($"{key}StormbowTip").Value);
                    tooltips.Add(tip);
                }
            }
        }
    }
}