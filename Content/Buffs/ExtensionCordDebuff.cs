using CalRemix.Content.Projectiles.Hostile;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Buffs
{
    public class ExtensionCordDebuff : ModBuff
    {
        public override string Texture => "CalamityMod/Buffs/StatBuffs/AdrenalineMode";

        public static readonly int TagDamage = 22;

        public override void SetStaticDefaults()
        {
            // This allows the debuff to be inflicted on NPCs that would otherwise be immune to all debuffs.
            // Other mods may check it for different purposes.
            BuffID.Sets.IsATagBuff[Type] = true;
        }
    }

    public class ExtensionCableNPC : GlobalNPC
    {
        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers)
        {
            // Only player attacks should benefit from this buff, hence the NPC and trap checks.
            if (projectile.npcProj || projectile.trap || !projectile.IsMinionOrSentryRelated)
                return;


            // SummonTagDamageMultiplier scales down tag damage for some specific minion and sentry projectiles for balance purposes.
            var projTagMultiplier = ProjectileID.Sets.SummonTagDamageMultiplier[projectile.type];
            if (npc.HasBuff<ExtensionCordDebuff>())
            {
                // Apply a flat bonus to every hit
                modifiers.FlatBonusDamage += ExtensionCordDebuff.TagDamage * projTagMultiplier;
                int p = Projectile.NewProjectile(projectile.GetSource_FromThis(), npc.Center - new Microsoft.Xna.Framework.Vector2(Main.rand.Next(-10, 10), 3200f), Vector2.UnitY, ModContent.ProjectileType<IonogenLightning>(), projectile.damage, 0, Main.player[projectile.owner].whoAmI, 0f, -1, 61);
                Main.projectile[p].hostile = false;
                Main.projectile[p].friendly = true;
                Main.projectile[p].timeLeft = 22;
                SoundEngine.PlaySound(CalamityMod.Sounds.CommonCalamitySounds.LightningSound);
            }
        }
    }
}