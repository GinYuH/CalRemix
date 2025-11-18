using CalRemix.Content.DamageClasses;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Weapons.Stormbow
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
    }

    public class Torrential : StormbowPrefix
    {
        public override float damageMult => 1.1f;
        public override float useTimeMult => 0.9f;

        public override int critBonus => 3;
    }

    public class Unyielding : StormbowPrefix
    {
        public override float damageMult => 1.1f;
        public override float useTimeMult => 0.8f;
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
    }

    public class Unstable : StormbowPrefix
    {
        public override float damageMult => 0.8f;
        public override float shootSpeedMult => 0.8f;
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
    }

    public class Fumbling : StormbowPrefix
    {
        public override float damageMult => 0.95f;
        public override float shootSpeedMult => 0.8f;
        public override float useTimeMult => 1.05f;
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
    }

    public abstract class StormbowPrefix : ModPrefix, ILocalizedModType
    {
        public new string LocalizationCategory => "Prefixes";

        // Stats
        public virtual float damageMult => 1f;
        public virtual float useTimeMult => 1f;
        public virtual int critBonus => 0;
        public virtual float shootSpeedMult => 1f;

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
    }
}
