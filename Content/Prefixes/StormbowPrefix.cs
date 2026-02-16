using CalamityMod;
using CalRemix.Content.DamageClasses;
using CalRemix.Content.Items.Weapons.Stormbow;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalRemix.Content.Prefixes
{
    public class Turbulent : StormbowPrefix
    {
        public override float useTimeMult => 0.8f;
    }

    public class Focused : StormbowPrefix
    {
        public override float shootSpeedMult => 1.2f;
    }

    public class Bountiful : StormbowPrefix
    {
        public override float useTimeMult => 0.75f;
        public override float shootSpeedMult => 0.9f;

        public override float projAmount => 4;
    }

    public class Torrential : StormbowPrefix
    {
        public override float damageMult => 1.1f;
        public override float useTimeMult => 0.9f;

        public override int critBonus => 3;

        public override float projAmount => 4;
    }

    public class Unyielding : StormbowPrefix
    {
        public override float damageMult => 1.1f;
        public override float useTimeMult => 0.8f;

        public override float projAmount => 3;
    }

    public class Persistent : StormbowPrefix
    {
        public override float damageMult => 1.1f;
        public override float shootSpeedMult => 1.1f;
    }

    public class Steady : StormbowPrefix
    {
        public override float useTimeMult => 1.05f;
        public override float shootSpeedMult => 1.2f;
    }

    public class Infinite : StormbowPrefix
    {
        public override float damageMult => 1.15f;
        public override float useTimeMult => 0.7f;
        public override float shootSpeedMult => 1.3f;

        public override int critBonus => 5;

        public override float projAmount => 5;
    }

    public class Unstable : StormbowPrefix
    {
        public override float damageMult => 0.8f;
        public override float shootSpeedMult => 0.8f;

        public override float projAmount => -2;
    }

    public class Clumsy : StormbowPrefix
    {
        public override float damageMult => 0.9f;
        public override float shootSpeedMult => 0.7f;
    }

    public class Erroneous : StormbowPrefix
    {
        public override float shootSpeedMult => 0.7f;
        public override float useTimeMult => 1.05f;

        public override float projAmount => -1;
    }

    public class Fumbling : StormbowPrefix
    {
        public override float damageMult => 0.95f;
        public override float shootSpeedMult => 0.8f;
        public override float useTimeMult => 1.05f;

        public override float projAmount => -2;
    }

    public class Erratic : StormbowPrefix
    {
        public override float damageMult => 0.85f;
        public override float shootSpeedMult => 0.8f;
    }

    public class Nervous : StormbowPrefix
    {
        public override float damageMult => 0.7f;
        public override float shootSpeedMult => 0.6f;

        public override float projAmount => -2;
    }

    public class Jittery : StormbowPrefix
    {
        public override float useTimeMult => 0.95f;
        public override float shootSpeedMult => 0.95f;
    }

    public class Clear : StormbowPrefix
    {
        public override float useTimeMult => 0.6f;
        public override float shootSpeedMult => 0.6f;

        public override float projAmount => -4;
    }

    public abstract class StormbowPrefix : ModPrefix, ILocalizedModType
    {
        public new string LocalizationCategory => "Prefixes";

        // Stats
        public virtual float damageMult => 1f;
        public virtual float useTimeMult => 1f;
        public virtual int critBonus => 0;
        public virtual float shootSpeedMult => 1f;

        public virtual float projAmount => 1f;


        // Prefix roll logic -- Can also be rolled by throwing weapons, even if stealth strikes don't exist
        public override PrefixCategory Category => PrefixCategory.AnyWeapon;
        public override bool CanRoll(Item item) => item.CountsAsClass<StormbowDamageClass>() && (item.maxStack == 1 || item.AllowReforgeForStackableItem) && GetType() != typeof(StormbowPrefix);

        // Applying normal weapon stats
        public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
        {
            damageMult = this.damageMult;
            useTimeMult = this.useTimeMult;
            critBonus = this.critBonus;
            shootSpeedMult = this.shootSpeedMult;
        }


        // Applying stealth strike damage
        public override void Apply(Item item)
        {
            int arrowNum = 1;
            if (item.ModItem != null)
            {
                if (item.ModItem is StormbowAbstract abs)
                {
                    if (abs.arrowAmount <= 1)
                        return;
                    else
                        arrowNum = abs.arrowAmount;
                }
            }
            int finalAmt = (int)projAmount;
            if ((int)(arrowNum + projAmount) < 1)
                finalAmt = 1 - arrowNum;
            if (item.CountsAsClass<StormbowDamageClass>())
                item.Remix().arrowAmount = finalAmt;
        }

        // Changing value based on prefix tier (rarity is set automatically around value multiplier)
        public override void ModifyValue(ref float valueMult)
        {
            float extraStealthDamage = projAmount - 2;
            float stealthDamageValueMultiplier = 1f;
            float extraValue = 1f + stealthDamageValueMultiplier * extraStealthDamage;
            valueMult *= extraValue;
        }

        // Extra tooltip for new modifier stats
        public LocalizedText StormbowArrowTooltip => CalRemixHelper.LocalText($"{LocalizationCategory}.StormbowArrowTooltip");
        public override IEnumerable<TooltipLine> GetTooltipLines(Item item)
        {
            // Ignore this if there's no mult
            if (projAmount == 1f)
                yield break;

            if (item.ModItem != null)
            {
                if (item.ModItem is StormbowAbstract abs)
                {
                    if (abs.arrowAmount <= 1)
                        yield break;
                }
            }

            yield return new TooltipLine(Mod, "PrefixStormbowArrowBoost", StormbowArrowTooltip.Format((projAmount >= 1f ? "+" : string.Empty) + (projAmount - 1).ToString("N0"), projAmount > 1f ? GetPrefixText("MoreArrow") : GetPrefixText("LessArrow")))
            {
                IsModifier = true,
                IsModifierBad = projAmount < 1f
            };
        }

        public string GetPrefixText(string key)
        {
            return CalRemixHelper.LocalText($"{LocalizationCategory}." + key).Value;
        }
    }
}
