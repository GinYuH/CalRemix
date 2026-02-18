using CalamityMod.Rarities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class BlankStare : ModItem
    {
        public override void SetStaticDefaults()
        {
            if (Main.netMode != NetmodeID.Server)
            {
                ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = false;
                ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = false;
            }
        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ModContent.RarityType<CosmicPurple>();
            Item.vanity = true;
        }
    }
    public class BlankDrawLayer : PlayerDrawLayer
    {
        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            return Main.LocalPlayer.armor[0].type == ModContent.ItemType<BlankStare>() || Main.LocalPlayer.armor[10].type == ModContent.ItemType<BlankStare>();
        }
        public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.Head);
        protected override void Draw(ref PlayerDrawSet drawinfo)
        {
            Texture2D texture = ModContent.Request<Texture2D>("CalRemix/Content/Items/Armor/BlankStare_Head").Value; 
            bool armorOn = drawinfo.drawPlayer.head > 0 && !ArmorIDs.Head.Sets.DrawHead[drawinfo.drawPlayer.head];
            if (!drawinfo.drawPlayer.invis && !armorOn)
            {
                DrawData drawData = new DrawData(texture, new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.drawPlayer.bodyFrame, drawinfo.colorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
                drawData.shader = drawinfo.skinDyePacked;
                DrawData item = drawData;
                drawinfo.DrawDataCache.Add(item);
            }
        }
    }
}