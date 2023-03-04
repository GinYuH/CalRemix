using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.CalPlayer;
using System.Collections.Generic;
using CalamityMod.NPCs.NormalNPCs;
using CalamityMod.NPCs.SlimeGod;
using CalamityMod.NPCs.AcidRain;
using CalRemix.NPCs;

namespace CalRemix.Projectiles.Accessories
{
    public class AssortMinion : ModProjectile
    {
        bool latching = false;
        NPC ntarget;
        public override string Texture => "CalamityMod/NPCs/AcidRain/IrradiatedSlime";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Nucleate Gello");
            Main.projFrames[Projectile.type] = 2;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 30;
            Projectile.netImportant = true;
            Projectile.friendly = true;
            Projectile.minionSlots = 0f;
            Projectile.alpha = 75;
            Projectile.aiStyle = ProjAIStyleID.Pet;
            Projectile.timeLeft = 18000;
            Projectile.penetrate = -1;
            Projectile.timeLeft *= 5;
            Projectile.minion = true;
            AIType = ProjectileID.BabySlime;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 5;
            Projectile.DamageType = DamageClass.Summon;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            fallThrough = false;
            return true;
        }

        public override void AI()
        {
            if (latching)
            {
                // A valid target that can take damage and is homable
                if (ntarget.active && ntarget != null && !ntarget.dontTakeDamage && ntarget.chaseable)
                {
                    // If the player has a targer that isn't the minion's target, detatch
                    if (Main.player[Projectile.owner].MinionAttackTargetNPC > 0 && Main.player[Projectile.owner].MinionAttackTargetNPC != ntarget.whoAmI)
                    {
                        latching = false;
                    }
                    // else lock onto em!
                    Projectile.position = ntarget.Center;
                    Projectile.velocity = ntarget.velocity;
                }
                // else stop latching
                else
                {
                    latching = false;
                }
            }
            Player player = Main.player[Projectile.owner];
            CalRemixPlayer modPlayer = player.GetModPlayer<CalRemixPlayer>();
            bool nubert = Projectile.type == ModContent.ProjectileType<NucleateGelloMinion>();
            // you can't stack nuclegel 
            if (!modPlayer.nuclegel)
            {
                Projectile.active = false;
                return;
            }
            if (nubert)
            {
                if (player.dead)
                {
                    modPlayer.nuclegel = false;
                }
                if (modPlayer.nuclegel)
                {
                    Projectile.timeLeft = 2;
                }
            }
            Projectile.frame = 0;
            Projectile.frameCounter = 0;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (!latching && (target.whoAmI == Main.player[Projectile.owner].MinionAttackTargetNPC || Main.player[Projectile.owner].MinionAttackTargetNPC <= 0))
            {
                ntarget = target;
                latching = true;
            }
            target.AddBuff(ModContent.BuffType<Irradiated>(), 600);
            target.AddBuff(ModContent.BuffType<SulphuricPoisoning>(), 1200);
        }

        public override bool OnTileCollide(Vector2 oldVelocity) => false;
        public override Color? GetAlpha(Color lightColor)
        {
            Player owner = Main.player[Projectile.owner];
            CalamityPlayer calPlayer = owner.GetModPlayer<CalamityPlayer>();
            Color recol = Color.White;
            if (owner.ZoneBeach)
            {
                recol = new Color(0, 100, 255, Projectile.alpha);
            }
            else if (calPlayer.ZoneAbyss)
            {
                recol = new Color(10, 30, 255, Projectile.alpha);
            }
            else if (calPlayer.ZoneAstral)
            {
                recol = new Color(100, 30, 100, Projectile.alpha);
            }
            else if (calPlayer.ZoneCalamity)
            {
                recol = new Color(255, 30, 0, Projectile.alpha);
            }
            else if (calPlayer.ZoneSulphur)
            {
                recol = new Color(0, 90, 90, Projectile.alpha);
            }
            else if (calPlayer.ZoneSunkenSea)
            {
                recol = new Color(0, 30, 180, Projectile.alpha);
            }
            else if (owner.ZoneCorrupt)
            {
                recol = new Color(255, 0, 255, Projectile.alpha);
            }
            else if (owner.ZoneCrimson)
            {
                recol = new Color(255, 50, 50, Projectile.alpha);
            }
            else if (owner.ZoneDesert)
            {
                recol = new Color(100, 100, 5, Projectile.alpha);
            }
            else if (owner.ZoneDungeon)
            {
                recol = new Color(100, 20, 255, Projectile.alpha);
            }
            else if (owner.ZoneGlowshroom)
            {
                recol = new Color(0, 0, 255, Projectile.alpha);
            }
            else if (owner.ZoneHallow)
            {
                recol = new Color(70, 20, 180, Projectile.alpha);
            }
            else if (owner.ZoneLihzhardTemple)
            {
                recol = new Color(180, 180, 0, Projectile.alpha);
            }
            else if (owner.ZoneJungle)
            {
                recol = new Color(0, 255, 30, Projectile.alpha);
            }
            else if (owner.ZoneNormalSpace)
            {
                recol = new Color(180, 180, 180, Projectile.alpha);
            }
            else if (owner.ZoneSnow)
            {
                recol = new Color(0, 5, 90, Projectile.alpha);
            }
            else if (owner.ZoneUnderworldHeight)
            {
                recol = new Color(255, 10, 10, Projectile.alpha);
            }
            else
            {
                recol = new Color(10, 180, 30, Projectile.alpha);
            }
            return recol;
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            List<int> Slimes = new List<int>
            {
                1, 16, 59, 71, 81, 138, 121, 122, 141, 147, 183, 184, 204, 225, 244, 302, 333, 335, 334, 336, 537,
                NPCID.SlimeSpiked,
                NPCID.QueenSlimeMinionBlue,
                NPCID.QueenSlimeMinionPink,
                NPCID.QueenSlimeMinionPurple,
                ModContent.NPCType<CalamityMod.NPCs.Astral.AstralSlime>(),
                ModContent.NPCType<CalamityMod.NPCs.PlagueEnemies.PestilentSlime>(),
                ModContent.NPCType<AeroSlime>(),
                ModContent.NPCType<BloomSlime>(),
                ModContent.NPCType<CalamityMod.NPCs.Crags.CharredSlime>(),
                ModContent.NPCType<PerennialSlime>(),
                ModContent.NPCType<CryoSlime>(),
                ModContent.NPCType<GammaSlime>(),
                ModContent.NPCType<IrradiatedSlime>(),
                ModContent.NPCType<AuricSlime>(),
                ModContent.NPCType<CorruptSlimeSpawn>(),
                ModContent.NPCType<CorruptSlimeSpawn2>(),
                ModContent.NPCType<CrimsonSlimeSpawn>(),
                ModContent.NPCType<CrimsonSlimeSpawn2>()
            };
            if (Slimes.Contains(target.type))
            {
                damage = 0;
            }
        }
    }
}
