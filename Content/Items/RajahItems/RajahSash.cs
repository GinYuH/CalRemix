using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria;
using Terraria.Localization;
using System.Collections.Generic;
using CalamityMod;

namespace CalRemix.Content.Items.RajahItems
{
    public class RajahSash : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Rajah Rabbit's Sash of Vengeance");
            /* Tooltip.SetDefault(@"Every 10% of health lost gives you 8% extra attack power to your highest damage type boost
40% increased movement speed
Increased Jump Height and Speed
Grants Autojump
Immunity to fall damage"); */
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.sellPrice(0, 12, 0, 0);
            Item.rare = 9;
            Item.accessory = true;
            Item.expertOnly = true;
            Item.expert = true;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Player player = Main.player[Item.playerIndexTheItemIsReservedFor];
            Color damageColor = Color.Firebrick;
            string DamageType = "";

            DamageClass best = CalamityUtils.GetBestClass(player);

            if (best.Type == DamageClass.Melee.Type)
            {
                DamageType = Language.GetTextValue("Mods.CalRemix.Common.RajahSPTooltipMelee");
                damageColor = Color.Firebrick;
            }
            else if (best.Type == DamageClass.Ranged.Type)
            {
                DamageType = Language.GetTextValue("Mods.CalRemix.Common.RajahSPTooltipRanged");
                damageColor = Color.SeaGreen;
            }
            else if (best.Type == DamageClass.Magic.Type)
            {
                DamageType = Language.GetTextValue("Mods.CalRemix.Common.RajahSPTooltipMagic");
                damageColor = Color.Violet;
            }
            else if (best.Type == DamageClass.Summon.Type)
            {
                DamageType = Language.GetTextValue("Mods.CalRemix.Common.RajahSPTooltipSummoning");
                damageColor = Color.Cyan;
            }
            else if (best.Type == DamageClass.Throwing.Type)
            {
                DamageType = Language.GetTextValue("Mods.CalRemix.Common.RajahSPTooltipThrowing");
                damageColor = Color.DarkOrange;
            }

            string DamageAmount = (100 * DamageBoost(player)) + "% ";
            TooltipLine DamageTooltip = new TooltipLine(Mod, "Damage Type", Language.GetTextValue("Mods.CalRemix.Common.RajahSPDamageBoost") + DamageAmount + DamageType + Language.GetTextValue("Mods.CalRemix.Common.RajahSPDamageInfo"))
            {
                OverrideColor = damageColor
            };
            tooltips.Add(DamageTooltip);

            base.ModifyTooltips(tooltips);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.autoJump = true;
            Player.jumpHeight = 10;
            player.jumpSpeedBoost += 3.6f;
            player.noFallDmg = true;
            player.moveSpeed += .4f;

            DamageClass best = CalamityUtils.GetBestClass(player);

            if (best.Type == DamageClass.Melee.Type)
                player.GetDamage(DamageClass.Melee) += DamageBoost(player);
            else if (best.Type == DamageClass.Ranged.Type)
                player.GetDamage(DamageClass.Ranged) += DamageBoost(player);
            else if (best.Type == DamageClass.Magic.Type)
                player.GetDamage(DamageClass.Magic) += DamageBoost(player);
            else if (best.Type == DamageClass.Summon.Type)
                player.GetDamage(DamageClass.Summon) += DamageBoost(player);
            else if (best.Type == DamageClass.Throwing.Type)
                player.GetDamage(DamageClass.Throwing) += DamageBoost(player);
        }

        public static float DamageBoost(Player player)
        {
            if (player.statLife <= player.statLifeMax * .1f)
            {
                return .72f;
            }
            else if (player.statLife <= player.statLifeMax * .2f)
            {
                return .64f;
            }
            else if (player.statLife <= player.statLifeMax * .3f)
            {
                return .56f;
            }
            else if (player.statLife <= player.statLifeMax * .4f)
            {
                return .48f;
            }
            else if (player.statLife <= player.statLifeMax * .5f)
            {
                return .4f;
            }
            else if (player.statLife <= player.statLifeMax * .6f)
            {
                return .32f;
            }
            else if (player.statLife <= player.statLifeMax * .7f)
            {
                return .24f;
            }
            else if (player.statLife <= player.statLifeMax * .8f)
            {
                return .16f;
            }
            else if (player.statLife <= player.statLifeMax * .9f)
            {
                return .08f;
            }
            else
            {
                return 0f;
            }
        }
    }
}