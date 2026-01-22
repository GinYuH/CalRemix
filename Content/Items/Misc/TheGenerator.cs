using static Terraria.ModLoader.ModContent;
using static CalRemix.Core.CustomGen;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using CalamityMod.NPCs.Cryogen;
using CalRemix.Core;
using Terraria.Map;
namespace CalRemix.Content.Items.Misc;
public class TheGenerator : ModItem
{
    public override void SetStaticDefaults()
    {
        Main.RegisterItemAnimation(Type, new DrawAnimationVertical(8, 10));
        ItemID.Sets.AnimatesAsSoul[Type] = true;
    }
    public override void SetDefaults()
    {
        Item.width = 10;
        Item.height = 10;
        Item.useTime = 30;
        Item.useAnimation = 30;
        Item.useStyle = ItemUseStyleID.Shoot;
        Item.value = Item.buyPrice(10);
        Item.autoReuse = false;
        Item.uniqueStack = true;
        Item.rare = ItemRarityID.Cyan;
    }
    public override bool AltFunctionUse(Player player) => true;
    public override bool CanUseItem(Player player) => !player.GetModPlayer<CalRemixPlayer>().generatingGen;
    public override bool? UseItem(Player player)
    {
        if (player.altFunctionUse == 2)
        {
            player.GetModPlayer<CalRemixPlayer>().customGen = new(0, Color.White, false, true, 0, Color.White, false, true);
            if (Main.myPlayer == player.whoAmI)
                SoundEngine.PlaySound(Cryogen.DeathSound);
        }
        else if (player.altFunctionUse != 2 && player.ItemAnimationJustStarted && !player.GetModPlayer<CalRemixPlayer>().generatingGen)
            player.GetModPlayer<CalRemixPlayer>().generatingGen = true;
        return true;
    }
    public override bool ConsumeItem(Player player) => false;
    public override bool CanRightClick() => true;
    public override void RightClick(Player player)
    {
        Item.ChangeItemType(ItemType<TheInactiveGenerator>());
    }
    public override void UpdateInventory(Player player)
    {
        player.GetModPlayer<CalRemixPlayer>().genActive = true;
    }
    public override Color? GetAlpha(Color lightColor) => Main.DiscoColor;
}
public class TheInactiveGenerator : ModItem
{
    public override void SetStaticDefaults()
    {
        ItemID.Sets.IsLavaImmuneRegardlessOfRarity[Type] = true;
    }
    public override void SetDefaults()
    {
        Item.width = 10;
        Item.height = 10;
        Item.useTime = 30;
        Item.useAnimation = 30;
        Item.useStyle = ItemUseStyleID.Shoot;
        Item.value = 0;
        Item.autoReuse = false;
        Item.uniqueStack = true;
        Item.rare = ItemRarityID.Gray;
    }
    public override bool CanUseItem(Player player) => !player.GetModPlayer<CalRemixPlayer>().generatingGen;
    public override bool? UseItem(Player player)
    {
        if (player.altFunctionUse == 2)
        {
            player.GetModPlayer<CalRemixPlayer>().customGen = new(0, Color.White, false, true, 0, Color.White, false, true);
            if (Main.myPlayer == player.whoAmI)
                SoundEngine.PlaySound(Cryogen.DeathSound);
        }
        else if (player.altFunctionUse != 2 && player.ItemAnimationJustStarted && !player.GetModPlayer<CalRemixPlayer>().generatingGen)
            player.GetModPlayer<CalRemixPlayer>().generatingGen = true;
        return true;
    }
    public override bool ConsumeItem(Player player) => false;
    public override bool CanRightClick() => true;
    public override void RightClick(Player player)
    {
        Item.ChangeItemType(ItemType<TheGenerator>());
    }
}
public class GeneratorLayer : PlayerDrawLayer
{
    public override bool IsHeadLayer => false;
    public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) => drawInfo.drawPlayer.GetModPlayer<CalRemixPlayer>().genActive && !drawInfo.drawPlayer.GetModPlayer<CalRemixPlayer>().generatingGen;
    public override Position GetDefaultPosition() => new Between(PlayerDrawLayers.FaceAcc, PlayerDrawLayers.MountFront);
    protected override void Draw(ref PlayerDrawSet drawInfo)
    {
        Player player = drawInfo.drawPlayer;
        CustomGen gen = player.GetModPlayer<CalRemixPlayer>().customGen;

        float rotation = MathHelper.TwoPi / 2 * (Main.GlobalTimeWrappedHourly % 2);

        Texture2D shield = GetTexture2D("Shield", gen.ShieldTexture, false);
        Texture2D core = GetTexture2D(string.Empty, gen.CoreTexture, false);
        Texture2D shieldGlow = GetTexture2D("Shield", gen.ShieldTexture, true);
        Texture2D coreGlow = GetTexture2D(string.Empty, gen.CoreTexture, true);


        var position = (player.mount.Type != MountID.None) ? player.MountedCenter - Vector2.UnitY * (player.height * 0.45f) : player.Center;
        position -= Main.screenPosition;
        position = new Vector2((int)position.X, (int)position.Y + player.gfxOffY);

        Color color = DrawColor(gen.CoreColor, player);
        Color color2 = DrawColor(gen.ShieldColor, player);
        Color glowColor = DrawColor(Color.White, player);
        if (gen.ShieldVisible)
        {
            drawInfo.DrawDataCache.Add(new DrawData(shield, position, null, color2, rotation, shield.Size() * 0.5f, 1f, SpriteEffects.None, 0));
            if (gen.ShieldGlow)
                drawInfo.DrawDataCache.Add(new DrawData(shieldGlow, position, null, glowColor, rotation, shield.Size() * 0.5f, 1f, SpriteEffects.None, 0));
        }
        if (gen.CoreVisible)
        {
            drawInfo.DrawDataCache.Add(new DrawData(core, position, null, color, player.fullRotation, core.Size() * 0.5f, 1f, SpriteEffects.None, 0));
            if (gen.CoreGlow)
                drawInfo.DrawDataCache.Add(new DrawData(coreGlow, position, null, glowColor, player.fullRotation, core.Size() * 0.5f, 1f, SpriteEffects.None, 0));
        }
    }
    private static Color DrawColor(Color c, Player p)
    {
        float lighting = Lighting.GetColor((int)p.Center.X / 16, (int)p.Center.Y / 16).ToVector3().Length();
        Color lColor = c * lighting;
        if (c.R > lColor.R)
            c.R = lColor.R;
        if (c.B > lColor.B)
            c.B = lColor.B;
        if (c.G > lColor.G)
            c.G = lColor.G;
        c.A = 255;
        return c;
    }
}
public class GeneratorMapIcon : ModMapLayer
{
    public override void Draw(ref MapOverlayDrawContext context, ref string text)
    {
        foreach (Player player in Main.ActivePlayers)
        {
            CalRemixPlayer p = player.GetModPlayer<CalRemixPlayer>();
            if (p.genActive)
                context.Draw(GetTexture2D(string.Empty, p.customGen.CoreTexture, false), player.Bottom.ToTileCoordinates().ToVector2(), p.customGen.CoreColor, new SpriteFrame(1, 1), 0.5f, 0.5f, Terraria.UI.Alignment.Center);
        }
    }
}