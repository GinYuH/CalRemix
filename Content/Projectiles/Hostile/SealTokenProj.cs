using CalRemix.Content.NPCs.Subworlds.Sealed;
using Terraria;

namespace CalRemix.Content.Projectiles.Hostile
{
    public class SealTokenProj : MinecraftArrow
    {
        public override string Texture => "CalRemix/Content/Items/Materials/SealToken";
        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.hostile = true;
            Projectile.extraUpdates = 0;
        }

        public override bool? CanHitNPC(NPC target)
        {
            NPC owner = Main.npc[(int)Projectile.ai[1]];
            return target.type != owner.type && SealedPuppet.infighting.Contains(target.type);
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.SourceDamage *= 3;
        }
    }
}