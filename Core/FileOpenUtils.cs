using System.Windows.Forms;
using Terraria.Utilities.FileBrowser;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using System;
using Terraria;

namespace CalRemix
{
    public class FileOpenUtils
    {
        public delegate void LoadImageResultDelegate(Texture2D tex);

        public static bool OpenImage(LoadImageResultDelegate imageLoadedCallback, string backupPath = "", string backupSaveName = "")
        {
            ExtensionFilter[] extensions = { new ExtensionFilter("Image files", "png", "jpg", "jpeg") };

            string text = FileBrowser.OpenFilePanel("We want to know all about you!", extensions);
            if (text != null)
            {
                Main.QueueMainThreadAction(() => LoadTexture(text, imageLoadedCallback, backupPath, backupSaveName));
                return true;
            }

            return false;
        }

        public static void LoadTexture(string path, LoadImageResultDelegate imageLoadedCallback, string backupPath = "", string backupSaveName = "")
        {
            Texture2D tex = null;

            try
            {
                FileStream stream = File.OpenRead(path);
                tex = Texture2D.FromStream(Main.graphics.GraphicsDevice, stream);
                stream.Dispose();
            }
            catch
            {
                MessageBox.Show("Couldn't open file!", "Our studio apologizes for the inconvenience");
            }

            imageLoadedCallback.Invoke(tex);

            if (backupPath == "" || tex == null)
                return;

            //Backup the file for when we reload the mod
            try
            {
                if (Utils.TryCreatingDirectory(backupPath))
                {
                    Stream saveStream = File.OpenWrite(backupPath + backupSaveName + ".png");
                    tex.SaveAsPng(saveStream, tex.Width, tex.Height);
                    saveStream.Dispose();
                }
            }
            catch
            {

            }
        }
    }
}
