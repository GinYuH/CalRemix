using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.Dusts;
using CalamityMod.Items.Placeables.Furniture;
using CalamityMod.Items.Placeables.FurnitureAbyss;
using CalRemix.Content.Items.Materials;
using CalRemix.Content.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace CalRemix.Content.Items.Weapons;

public class Driftorcher : ModItem
{
    private int torch = -1;
    public override void SetStaticDefaults()
    {
        // DisplayName.SetDefault("Driftorcher");
        CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
    }

    public override void SetDefaults()
    {
        Item.damage = 65;
        Item.DamageType = DamageClass.Melee;
        Item.width = 66;
        Item.height = 66;
        Item.useTime = 19;
        Item.useAnimation = 19;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.knockBack = 2;
        Item.rare = ItemRarityID.Pink;
        Item.value = Item.sellPrice(silver: 40);
        Item.UseSound = SoundID.Item1;
        Item.shoot = ProjectileType<DriftSpark>();
        Item.autoReuse = true;

    }
    public override bool? UseItem(Player player)
    {
        if (player.ItemAnimationJustStarted)
        {
            if (player.ConsumeItem(ItemID.RainbowTorch))
                torch = 1;
            else if (player.ConsumeItem(ItemType<AstralTorch>()))
                torch = 2;
            else if (player.ConsumeItem(ItemType<SulphurousTorch>(), true))
                torch = 3;
            else if (player.ConsumeItem(ItemType<AbyssTorch>(), true))
                torch = 4;
            else if (player.ConsumeItem(ItemType<GloomTorch>(), true))
                torch = 5;
            else if (player.ConsumeItem(ItemID.HallowedTorch, true))
                torch = 6;
            else if (player.ConsumeItem(ItemID.IchorTorch, true))
                torch = 7;
            else if (player.ConsumeItem(ItemID.BoneTorch, true))
                torch = 8;
            else if (player.ConsumeItem(ItemID.UltrabrightTorch))
                torch = 9;
            else if (player.ConsumeItem(ItemID.CursedTorch, true))
                torch = 10;
            else if (player.ConsumeItem(ItemType<AlgalPrismTorch>(), true))
                torch = 11;
            else if (player.ConsumeItem(ItemType<NavyPrismTorch>(), true))
                torch = 11;
            else if (player.ConsumeItem(ItemType<RefractivePrismTorch>(), true))
                torch = 11;
            else if (player.ConsumeItem(ItemID.DemonTorch, true))
                torch = 12;
            else if (player.ConsumeItem(ItemID.IceTorch, true))
                torch = 13;
            else if (player.ConsumeItem(ItemID.JungleTorch, true))
                torch = 14;
            else if (player.ConsumeItem(ItemID.CrimsonTorch, true))
                torch = 15;
            else if (player.ConsumeItem(ItemID.CorruptTorch, true))
                torch = 16;
            else if (player.ConsumeItem(ItemID.Torch, true))
                torch = 17;
            else
                torch = -1;
        }
        return true;
    }
    public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
    {
        if (GetDebuff() > -1)
            target.AddBuff(GetDebuff(), 120);
        else if (GetDebuff() == -2)
        {
            target.AddBuff(BuffType<BrimstoneFlames>(), 120);
            target.AddBuff(BuffID.OnFire3, 120);
            target.AddBuff(BuffType<HolyFlames>(), 120);
            target.AddBuff(BuffID.Frostburn2, 120);
            target.AddBuff(BuffID.Venom, 120);
        }
    }
    public override void MeleeEffects(Player player, Rectangle hitbox)
    {
        if (Main.rand.NextBool(3))
            Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, GetDust());
    }
    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        Vector2 d = (player.direction > 0) ? new Vector2(10f, 0f) : new Vector2(0f, -10f);
        for (int i = 0; i < 5; i++)
        {
            Projectile proj = Projectile.NewProjectileDirect(source, position, d.RotatedBy(MathHelper.ToRadians(-22.5f * i)), type, damage, knockback);
            DriftSpark spark = proj.ModProjectile as DriftSpark;
            spark.buff = GetDebuff();
            spark.dust = GetDust();
        }
        return false;
    }
    public override void AddRecipes()
    {
        CreateRecipe().
            AddIngredient(ItemID.Torch, 30).
            AddIngredient<EssenceofBabil>(5).
            AddRecipeGroup("AnyMythrilBar", 5).
            AddTile(TileID.MythrilAnvil).
            Register();
    }
    private int GetDebuff()
    {
        int buff = -1;
        switch (torch)
        {
            case 1:
                buff = -2;
                break;
            case 2:
                buff = BuffType<AstralInfectionDebuff>();
                break;
            case 3:
                buff = BuffType<Irradiated>();
                break;
            case 4:
                buff = BuffType<CrushDepth>();
                break;
            case 5:
                buff = BuffType<BrimstoneFlames>();
                break;
            case 6:
                buff = BuffType<MarkedforDeath>();
                break;
            case 7:
                buff = BuffID.Ichor;
                break;
            case 8:
                buff = BuffType<Crumbling>();
                break;
            case 9:
                buff = BuffType<Vaporfied>();
                break;
            case 10:
                buff = BuffID.CursedInferno;
                break;
            case 11:
                buff = BuffType<RiptideDebuff>();
                break;
            case 12:
                buff = BuffID.ShadowFlame;
                break;
            case 13:
                buff = BuffID.Frostburn;
                break;
            case 14:
                buff = BuffID.Poisoned;
                break;
            case 15:
                buff = BuffType<BurningBlood>();
                break;
            case 16:
                buff = BuffType<BrainRot>();
                break;
            case 17:
                buff = BuffID.OnFire;
                break;
        }
        return buff;
    }
    private int GetDust()
    {
        int dust = DustID.Smoke;
        switch (torch)
        {
            case 1:
                dust = Main.rand.Next(59, 65);
                break;
            case 2:
                dust = DustType<AstralOrange>();
                break;
            case 3:
                dust = (int)CalamityDusts.SulphurousSeaAcid;
                break;
            case 4:
                dust = DustID.DungeonSpirit;
                break;
            case 5:
                dust = DustID.LifeDrain;
                break;
            case 6:
                dust = DustID.HallowedTorch;
                break;
            case 7:
                dust = DustID.IchorTorch;
                break;
            case 8:
                dust = DustID.BoneTorch;
                break;
            case 9:
                dust = DustID.UltraBrightTorch;
                break;
            case 10:
                dust = DustID.CursedTorch;
                break;
            case 11:
                dust = Main.rand.Next(68, 71);
                break;
            case 12:
                dust = DustID.DemonTorch;
                break;
            case 13:
                dust = DustID.IceTorch;
                break;
            case 14:
                dust = DustID.JungleTorch;
                break;
            case 15:
                dust = DustID.CrimsonTorch;
                break;
            case 16:
                dust = DustID.CorruptTorch;
                break;
            case 17:
                dust = DustID.Torch;
                break;
        }
        return dust;
    }
}

