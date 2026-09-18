using CalamityMod;
using CalamityMod.Schematics;
using CalRemix.Core.Subworlds;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Terraria.ModLoader;

namespace CalRemix.Core.World
{
    public class RemixSchematics : ModSystem
    {
        internal const string HallowShrineName = "Core/Schematics/hallowshrine.csch";

        public static Dictionary<string, TempleRoom> templeRoomTypes = new();

        internal static Dictionary<string, SchematicMetaTile[,]> TileMaps =>
            typeof(SchematicManager).GetField("TileMaps", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null) as Dictionary<string, SchematicMetaTile[,]>;

        internal static readonly MethodInfo ImportSchematicMethod = typeof(CalamitySchematicIO).GetMethod("ImportSchematic", BindingFlags.NonPublic | BindingFlags.Static);

        public static void AddTempleRoom(string key, bool up = false, bool down = false, bool left = false, bool right = false)
        {
            TempleRoom t = new();
            t.schematic = "Temple" + key;
            t.Left = left;
            t.Right = right;
            t.Up = up;
            t.Down = down;
            TileMaps.Add("Temple" + key, LoadSchematic("Core/Schematics/Temple/Temple" + key + ".csch"));
            templeRoomTypes.Add(key, t);
        }

        public override void PostSetupContent()
        {
            TileMaps.Add("Hallow Shrine", LoadSchematic(HallowShrineName).ShaveOffEdge());

            AddTempleRoom("LU", left: true, up: true);
            AddTempleRoom("DR", down: true, right: true);
            AddTempleRoom("LD", left: true, down: true);
            AddTempleRoom("LR", left: true, right: true);
            AddTempleRoom("LR2", left: true, right: true);
            AddTempleRoom("LU2", left: true, up: true);
            AddTempleRoom("U", up: true);
            AddTempleRoom("UR", up: true, right: true);
            AddTempleRoom("LDR", left: true, down: true, right: true);
            AddTempleRoom("LUDR", up: true, right: true, left: true, down: true);
            AddTempleRoom("LUR", up: true, right: true, left: true);
            AddTempleRoom("LUD", up: true, left: true, down: true);
            AddTempleRoom("L", left: true);
            AddTempleRoom("UDR", up: true, right: true, down: true);
            AddTempleRoom("UD", up: true, down: true);
            AddTempleRoom("R", right: true);
            AddTempleRoom("D", down: true);
        }


        public static SchematicMetaTile[,] LoadSchematic(string filename)
        {
            SchematicMetaTile[,] ret = null;
            using (Stream st = CalRemix.instance.GetFileStream(filename, true))
                ret = (SchematicMetaTile[,])ImportSchematicMethod.Invoke(null, [st]);

            return ret;
        }

    }
}