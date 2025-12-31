using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Dusts;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Projectiles.BaseProjectiles;
using CalamityMod.Projectiles.DraedonsArsenal;
using CalamityMod.Projectiles.Rogue;
using CalamityMod.Projectiles.Typeless;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using rail;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Weapons
{
    public abstract class RogueSpear : BaseSpearProjectile
    {
        public virtual int ProjType => 0;
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 40;
            Projectile.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Projectile.timeLeft = 90;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.ownerHitCheck = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 6;
        }

        public override float InitialSpeed => 1;
        public override float ReelbackSpeed => Main.player[Projectile.owner].HeldItem.useTime * 0.005f;
        public override float ForwardSpeed => Main.player[Projectile.owner].HeldItem.useTime * 0.005f;

        public override Action<Projectile> EffectBeforeReelback => (proj) =>
        {
            int ai0 = proj.type == ModContent.ProjectileType<AtomSplitter>() ? -1 : 0;
            int p = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity.SafeNormalize(Vector2.UnitY) * Main.player[Projectile.owner].HeldItem.shootSpeed, ProjType, Projectile.damage, Projectile.knockBack, Projectile.owner, ai0);
            if (proj.Calamity().stealthStrike)
            {
                Main.projectile[p].Calamity().stealthStrike = true;
            }
        };

        public override void PostAI()
        {
            //Projectile.rotation -= MathHelper.PiOver2;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
            Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation - MathHelper.PiOver2, tex.Size() / 2, Projectile.scale, SpriteEffects.None);
            return false;
        }
    }

    public class ScourgeDesert : RogueSpear
    {
        public override string Texture => "CalamityMod/Items/Weapons/Rogue/ScourgeoftheDesert";
        public override int ProjType => ModContent.ProjectileType<ScourgeoftheDesertProj>();
    }

    public class Turbulance : RogueSpear
    {
        public override string Texture => "CalamityMod/Items/Weapons/Rogue/Turbulance";
        public override int ProjType => ModContent.ProjectileType<TurbulanceProjectile>();
    }

    public class IchorSpear : RogueSpear
    {
        public override string Texture => "CalamityMod/Items/Weapons/Rogue/IchorSpear";
        public override int ProjType => ModContent.ProjectileType<IchorSpearProj>();
    }

    public class CursedSpear : RogueSpear
    {
        public override string Texture => "CalRemix/Content/Items/Weapons/CursedSpear";
        public override int ProjType => ModContent.ProjectileType<CursedSpearProj>();
    }

    public class PalJav : RogueSpear
    {
        public override string Texture => "CalamityMod/Items/Weapons/Rogue/PalladiumJavelin";
        public override int ProjType => ModContent.ProjectileType<PalladiumJavelinProjectile>();
    }

    public class CrystalPiercer : RogueSpear
    {
        public override string Texture => "CalamityMod/Items/Weapons/Rogue/CrystalPiercer";
        public override int ProjType => ModContent.ProjectileType<CrystalPiercerProjectile>();
    }

    public class ScourgeSea : RogueSpear
    {
        public override string Texture => "CalamityMod/Items/Weapons/Rogue/ScourgeoftheSeas";
        public override int ProjType => ModContent.ProjectileType<ScourgeoftheSeasProjectile>();
    }

    public class Paleolith : RogueSpear
    {
        public override string Texture => "CalamityMod/Items/Weapons/Rogue/SpearofPaleolith";
        public override int ProjType => ModContent.ProjectileType<SpearofPaleolithProj>();
    }

    public class WaveSkipper : RogueSpear
    {
        public override string Texture => "CalamityMod/Items/Weapons/Rogue/WaveSkipper";
        public override int ProjType => ModContent.ProjectileType<WaveSkipperProjectile>();
    }

    public class SpearDestiny : RogueSpear
    {
        public override string Texture => "CalamityMod/Items/Weapons/Rogue/SpearofDestiny";
        public override int ProjType => ModContent.ProjectileType<SpearofDestinyProjectile>();
    }

    public class PhantasmalRuin : RogueSpear
    {
        public override string Texture => "CalamityMod/Items/Weapons/Rogue/PhantasmalRuin";
        public override int ProjType => ModContent.ProjectileType<PhantasmalRuinProj>();
    }

    public class Antumbra : RogueSpear
    {
        public override string Texture => "CalamityMod/Items/Weapons/Rogue/ShardofAntumbra";
        public override int ProjType => ModContent.ProjectileType<AntumbraShardProjectile>();
    }

    public class ProfanedPartisan : RogueSpear
    {
        public override string Texture => "CalamityMod/Items/Weapons/Rogue/ProfanedPartisan";
        public override int ProjType => ModContent.ProjectileType<ProfanedPartisanProj>();
    }

    public class RealityRapture : RogueSpear
    {
        public override string Texture => "CalamityMod/Items/Weapons/Rogue/RealityRupture";
        public override int ProjType => ModContent.ProjectileType<RealityRuptureLance>();
    }

    public class NightsGaze : RogueSpear
    {
        public override string Texture => "CalamityMod/Items/Weapons/Rogue/Vega";
        public override int ProjType => ModContent.ProjectileType<VegaProjectile>();
    }

    public class EclipseFall : RogueSpear
    {
        public override string Texture => "CalamityMod/Items/Weapons/Rogue/EclipsesFall";
        public override int ProjType => ModContent.ProjectileType<EclipsesFallMain>();
    }

    public class AtomSplitter : RogueSpear
    {
        public override string Texture => "CalamityMod/Items/Weapons/Rogue/TheAtomSplitter";
        public override int ProjType => ModContent.ProjectileType<TheAtomSplitterProjectile>();
    }

    public class Wrathwing : RogueSpear
    {
        public override string Texture => "CalamityMod/Items/Weapons/Rogue/Wrathwing";
        public override int ProjType => ModContent.ProjectileType<Wrathwing>();
    }

    public class ScarletDevil : RogueSpear
    {
        public override string Texture => "CalamityMod/Items/Weapons/Rogue/ScarletDevil";
        public override int ProjType => ModContent.ProjectileType<ScarletDevilProjectile>();
    }
}
