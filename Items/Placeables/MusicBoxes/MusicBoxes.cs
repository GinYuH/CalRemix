using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace CalRemix.Items.Placeables.MusicBoxes
{
    public abstract class RemixMusicBox : ModItem
    {
        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.autoReuse = true;
            Item.consumable = true;
            Item.width = 24;
            Item.height = 24;
            Item.value = Item.buyPrice(gold: 10);
            Item.rare = ItemRarityID.LightRed;
            Item.accessory = true;
        }
        public override bool? PrefixChance(int pre, UnifiedRandom rand)
        {
            if (this != null)
                return false;
            else
                return true;
        }
    }
    public class AcidsighterMusicBox : RemixMusicBox
    {
        public override void SetStaticDefaults()
        {
            MusicLoader.AddMusicBox(Mod, MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Opticatalysis"), Type, ModContent.TileType<Tiles.MusicBoxes.AcidsighterMusicBox>());
        }
        public override void SetDefaults()
        {
            Item.createTile = ModContent.TileType<Tiles.MusicBoxes.AcidsighterMusicBox>();
            base.SetDefaults();
        }
        public override bool? PrefixChance(int pre, UnifiedRandom rand)
        {
            return base.PrefixChance(pre, rand);
        }
    }
    public class CryoSlimeMusicBox : RemixMusicBox
    {
        public override void SetStaticDefaults()
        {
            MusicLoader.AddMusicBox(Mod, MusicLoader.GetMusicSlot(Mod, "Sounds/Music/AntarcticReinsertion"), Type, ModContent.TileType<Tiles.MusicBoxes.CryoSlimeMusicBox>());
        }
        public override void SetDefaults()
        {
            Item.createTile = ModContent.TileType<Tiles.MusicBoxes.CryoSlimeMusicBox>();
            base.SetDefaults();
        }
        public override bool? PrefixChance(int pre, UnifiedRandom rand)
        {
            return base.PrefixChance(pre, rand);
        }
    }
    public class DerellectMusicBox : RemixMusicBox
    {
        public override void SetStaticDefaults()
        {
            MusicLoader.AddMusicBox(Mod, MusicLoader.GetMusicSlot(Mod, "Sounds/Music/SignalInterruption"), Type, ModContent.TileType<Tiles.MusicBoxes.DerellectMusicBox>());
        }
        public override void SetDefaults()
        {
            Item.createTile = ModContent.TileType<Tiles.MusicBoxes.DerellectMusicBox>();
            base.SetDefaults();
        }
        public override bool? PrefixChance(int pre, UnifiedRandom rand)
        {
            return base.PrefixChance(pre, rand);
        }
    }
    public class ExcavatorMusicBox : RemixMusicBox
    {
        public override void SetStaticDefaults()
        {
            MusicLoader.AddMusicBox(Mod, MusicLoader.GetMusicSlot(Mod, "Sounds/Music/ScourgeoftheScrapyard"), Type, ModContent.TileType<Tiles.MusicBoxes.ExcavatorMusicBox>());
        }
        public override void SetDefaults()
        {
            Item.createTile = ModContent.TileType<Tiles.MusicBoxes.ExcavatorMusicBox>();
            base.SetDefaults();
        }
        public override bool? PrefixChance(int pre, UnifiedRandom rand)
        {
            return base.PrefixChance(pre, rand);
        }
    }
    public class LaRugaMusicBox : RemixMusicBox
    {
        public override void SetStaticDefaults()
        {
            MusicLoader.AddMusicBox(Mod, MusicLoader.GetMusicSlot(Mod, "Sounds/Music/LaRuga"), Type, ModContent.TileType<Tiles.MusicBoxes.LaRugaMusicBox>());
        }
        public override void SetDefaults()
        {
            Item.createTile = ModContent.TileType<Tiles.MusicBoxes.LaRugaMusicBox>();
            base.SetDefaults();
        }
        public override bool? PrefixChance(int pre, UnifiedRandom rand)
        {
            return base.PrefixChance(pre, rand);
        }
    }
    public class PlaguedJungleMusicBox : RemixMusicBox
    {
        public override void SetStaticDefaults()
        {
            MusicLoader.AddMusicBox(Mod, MusicLoader.GetMusicSlot(Mod, "Sounds/Music/PlaguedJungle"), Type, ModContent.TileType<Tiles.MusicBoxes.PlaguedJungleMusicBox>());
        }
        public override void SetDefaults()
        {
            Item.createTile = ModContent.TileType<Tiles.MusicBoxes.PlaguedJungleMusicBox>();
            base.SetDefaults();
        }
        public override bool? PrefixChance(int pre, UnifiedRandom rand)
        {
            return base.PrefixChance(pre, rand);
        }
    }
    public class PolyphemalusMusicBox : RemixMusicBox
    {
        public override void SetStaticDefaults()
        {
            MusicLoader.AddMusicBox(Mod, MusicLoader.GetMusicSlot(Mod, "Sounds/Music/EyesofFlame"), Type, ModContent.TileType<Tiles.MusicBoxes.PolyphemalusMusicBox>());
        }
        public override void SetDefaults()
        {
            Item.createTile = ModContent.TileType<Tiles.MusicBoxes.PolyphemalusMusicBox>();
            base.SetDefaults();
        }
        public override bool? PrefixChance(int pre, UnifiedRandom rand)
        {
            return base.PrefixChance(pre, rand);
        }
    }
    public class PolyphemalusGFBMusicBox : RemixMusicBox
    {
        public override void SetStaticDefaults()
        {
            MusicLoader.AddMusicBox(Mod, MusicLoader.GetMusicSlot(Mod, "Sounds/Music/TheEyesareAnger"), Type, ModContent.TileType<Tiles.MusicBoxes.PolyphemalusGFBMusicBox>());
        }
        public override void SetDefaults()
        {
            Item.createTile = ModContent.TileType<Tiles.MusicBoxes.PolyphemalusGFBMusicBox>();
            base.SetDefaults();
        }
        public override bool? PrefixChance(int pre, UnifiedRandom rand)
        {
            return base.PrefixChance(pre, rand);
        }
    }
}
