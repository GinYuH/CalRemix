using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Weapons;

public class NystagmusProjectileBlue : ModProjectile
{
    public override void SetDefaults()
    {
        Projectile.width = 26;
        base.Projectile.height = 26;
        base.Projectile.friendly = true;
        if (Projectile.ai[1] == 2)
        {
            base.Projectile.penetrate = 3;
        }
        else
        {
            base.Projectile.penetrate = 1;
        }
           
        base.Projectile.tileCollide = false;
        base.Projectile.ownerHitCheck = true;
        if (Projectile.ai[0] == 0)
        {
            base.Projectile.DamageType = DamageClass.Magic;
        }
        if (Projectile.ai[0] == 1)
        {
            base.Projectile.DamageType = DamageClass.Summon;
        }
        if (Projectile.ai[0] == 2)
        {
            base.Projectile.DamageType = DamageClass.Melee;
        }
        Projectile.usesLocalNPCImmunity = true;
        Projectile.localNPCHitCooldown = 30;
        Projectile.timeLeft = 600;
        Projectile.scale = 1f;
    }
    public override void AI()
    {
        float maxDetectRadius = 800f;

        NPC closestNPC = FindClosestNPC(maxDetectRadius);
        Projectile.rotation = 45 * (float)(Math.PI / 180) + Projectile.velocity.ToRotation();
        if (closestNPC == null)
            return;
        if (Projectile.ai[1] == 2)
        {
            MoveTowards(closestNPC.Center, 90, 50);
        }else
        {
            MoveTowards(closestNPC.Center, 30, 50);
        }
    }

        private void MoveTowards(Vector2 goal, float speed, float inertia)
    {
        Vector2 moveTo = (goal - Projectile.Center).SafeNormalize(Vector2.UnitY) * speed / 1.5f;
        Projectile.velocity = (Projectile.velocity * (inertia - 1) + moveTo) / inertia;
    }

    public NPC FindClosestNPC(float maxDetectDistance)
    {
        NPC closestNPC = null;
        float sqrMaxDetectDistance = maxDetectDistance * maxDetectDistance;

        // Loop through all NPCs(max always 200)
        for (int k = 0; k < Main.maxNPCs; k++)
        {
            NPC target = Main.npc[k];
            if (target.CanBeChasedBy())
            {
                float sqrDistanceToTarget = Vector2.DistanceSquared(target.Center, Projectile.Center);
                if (sqrDistanceToTarget < sqrMaxDetectDistance)
                {
                    sqrMaxDetectDistance = sqrDistanceToTarget;
                    closestNPC = target;
                }
            }
        }
        return closestNPC;
    }
}

public class NystagmusProjectileRed : ModProjectile
{
    public override void SetDefaults()
    {
        Projectile.width = 26;
        base.Projectile.height = 26;
        base.Projectile.friendly = true;
        if (Projectile.ai[1] == 2)
        {
            base.Projectile.penetrate = 3;
        }
        else
        {
            base.Projectile.penetrate = 1;
        }

        base.Projectile.tileCollide = false;
        base.Projectile.ownerHitCheck = true;
        if (Projectile.ai[0] == 0)
        {
            base.Projectile.DamageType = DamageClass.Magic;
        }
        if (Projectile.ai[0] == 1)
        {
            base.Projectile.DamageType = DamageClass.Summon;
        }
        if (Projectile.ai[0] == 2)
        {
            base.Projectile.DamageType = DamageClass.Melee;
        }
        Projectile.usesLocalNPCImmunity = true;
        Projectile.localNPCHitCooldown = 30;
        Projectile.timeLeft = 600;
        Projectile.scale = 1f;
    }
    public override void AI()
    {
        float maxDetectRadius = 800f;

        NPC closestNPC = FindClosestNPC(maxDetectRadius);
        Projectile.rotation = 45 * (float)(Math.PI / 180) + Projectile.velocity.ToRotation();
        if (closestNPC == null)
            return;
        if (Projectile.ai[1] == 2)
        {
            MoveTowards(closestNPC.Center, 90, 50);
        }
        else
        {
            MoveTowards(closestNPC.Center, 30, 50);
        }
    }

    private void MoveTowards(Vector2 goal, float speed, float inertia)
    {
        Vector2 moveTo = (goal - Projectile.Center).SafeNormalize(Vector2.UnitY) * speed / 1.5f;
        Projectile.velocity = (Projectile.velocity * (inertia - 1) + moveTo) / inertia;
    }

    public NPC FindClosestNPC(float maxDetectDistance)
    {
        NPC closestNPC = null;
        float sqrMaxDetectDistance = maxDetectDistance * maxDetectDistance;

        // Loop through all NPCs(max always 200)
        for (int k = 0; k < Main.maxNPCs; k++)
        {
            NPC target = Main.npc[k];
            if (target.CanBeChasedBy())
            {
                float sqrDistanceToTarget = Vector2.DistanceSquared(target.Center, Projectile.Center);
                if (sqrDistanceToTarget < sqrMaxDetectDistance)
                {
                    sqrMaxDetectDistance = sqrDistanceToTarget;
                    closestNPC = target;
                }
            }
        }
        return closestNPC;
    }
}


public class NystagmusProjectileGreen : ModProjectile
{
    public override void SetDefaults()
    {
        Projectile.width = 26;
        base.Projectile.height = 26;
        base.Projectile.friendly = true;
        if (Projectile.ai[1] == 2)
        {
            base.Projectile.penetrate = 3;
        }
        else
        {
            base.Projectile.penetrate = 1;
        }

        base.Projectile.tileCollide = false;
        base.Projectile.ownerHitCheck = true;
        if (Projectile.ai[0] == 0)
        {
            base.Projectile.DamageType = DamageClass.Magic;
        }
        if (Projectile.ai[0] == 1)
        {
            base.Projectile.DamageType = DamageClass.Summon;
        }
        if (Projectile.ai[0] == 2)
        {
            base.Projectile.DamageType = DamageClass.Melee;
        }
        Projectile.usesLocalNPCImmunity = true;
        Projectile.localNPCHitCooldown = 30;
        Projectile.timeLeft = 600;
        Projectile.scale = 1f;
    }
    public override void AI()
    {
        float maxDetectRadius = 800f;

        NPC closestNPC = FindClosestNPC(maxDetectRadius);
        Projectile.rotation = 45 * (float)(Math.PI / 180) + Projectile.velocity.ToRotation();
        if (closestNPC == null)
            return;
        if (Projectile.ai[1] == 2)
        {
            MoveTowards(closestNPC.Center, 90, 50);
        }
        else
        {
            MoveTowards(closestNPC.Center, 30, 50);
        }
    }

    private void MoveTowards(Vector2 goal, float speed, float inertia)
    {
        Vector2 moveTo = (goal - Projectile.Center).SafeNormalize(Vector2.UnitY) * speed / 1.5f;
        Projectile.velocity = (Projectile.velocity * (inertia - 1) + moveTo) / inertia;
    }

    public NPC FindClosestNPC(float maxDetectDistance)
    {
        NPC closestNPC = null;
        float sqrMaxDetectDistance = maxDetectDistance * maxDetectDistance;

        // Loop through all NPCs(max always 200)
        for (int k = 0; k < Main.maxNPCs; k++)
        {
            NPC target = Main.npc[k];
            if (target.CanBeChasedBy())
            {
                float sqrDistanceToTarget = Vector2.DistanceSquared(target.Center, Projectile.Center);
                if (sqrDistanceToTarget < sqrMaxDetectDistance)
                {
                    sqrMaxDetectDistance = sqrDistanceToTarget;
                    closestNPC = target;
                }
            }
        }
        return closestNPC;
    }
}


public class NystagmusProjectileGray : ModProjectile
{
    public override void SetDefaults()
    {
        Projectile.width = 26;
        base.Projectile.height = 26;
        base.Projectile.friendly = true;
        if (Projectile.ai[1] == 2)
        {
            base.Projectile.penetrate = 3;
        }
        else
        {
            base.Projectile.penetrate = 1;
        }

        base.Projectile.tileCollide = false;
        base.Projectile.ownerHitCheck = true;
        if (Projectile.ai[0] == 0)
        {
            base.Projectile.DamageType = DamageClass.Magic;
        }
        if (Projectile.ai[0] == 1)
        {
            base.Projectile.DamageType = DamageClass.Summon;
        }
        if (Projectile.ai[0] == 2)
        {
            base.Projectile.DamageType = DamageClass.Melee;
        }
        Projectile.usesLocalNPCImmunity = true;
        Projectile.localNPCHitCooldown = 30;
        Projectile.timeLeft = 600;
        Projectile.scale = 1f;
    }
    public override void AI()
    {
        float maxDetectRadius = 800f;

        NPC closestNPC = FindClosestNPC(maxDetectRadius);
        Projectile.rotation = 45 * (float)(Math.PI / 180) + Projectile.velocity.ToRotation();
        if (closestNPC == null)
            return;
        if (Projectile.ai[1] == 2)
        {
            MoveTowards(closestNPC.Center, 90, 50);
        }
        else
        {
            MoveTowards(closestNPC.Center, 30, 50);
        }
    }

    private void MoveTowards(Vector2 goal, float speed, float inertia)
    {
        Vector2 moveTo = (goal - Projectile.Center).SafeNormalize(Vector2.UnitY) * speed / 1.5f;
        Projectile.velocity = (Projectile.velocity * (inertia - 1) + moveTo) / inertia;
    }

    public NPC FindClosestNPC(float maxDetectDistance)
    {
        NPC closestNPC = null;
        float sqrMaxDetectDistance = maxDetectDistance * maxDetectDistance;

        // Loop through all NPCs(max always 200)
        for (int k = 0; k < Main.maxNPCs; k++)
        {
            NPC target = Main.npc[k];
            if (target.CanBeChasedBy())
            {
                float sqrDistanceToTarget = Vector2.DistanceSquared(target.Center, Projectile.Center);
                if (sqrDistanceToTarget < sqrMaxDetectDistance)
                {
                    sqrMaxDetectDistance = sqrDistanceToTarget;
                    closestNPC = target;
                }
            }
        }
        return closestNPC;
    }
}
