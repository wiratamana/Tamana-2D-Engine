using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;

namespace TamanaEngine.Core
{
    public class DefaultFont
    {
        private static Dictionary<char, Bitmap> characters = new Dictionary<char, Bitmap>();

        private static class Settings
        {
            public static string FontBitmapFilename = "test.png";
            public static int GlyphsPerLine = 16;
            public static int GlyphLineCount = 16;
            public static int GlyphWidth = 22;
            public static int GlyphHeight = 44;

            public static int CharXSpacing = 22;

            public static string Text = "GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);";

            // Used to offset rendering glyphs to bitmap
            public static float AtlasOffsetX = -5.5f, AtlassOffsetY = -2;
            public static int FontSize = 28;
            public static bool BitmapFont = false;
            public static string FromFile; //= "joystix monospace.ttf";
            public static string FontName = "Consolas";

        }

        public static void InitFont()
        {
            int bitmapWidth = Settings.GlyphsPerLine * Settings.GlyphWidth;
            int bitmapHeight = Settings.GlyphLineCount * Settings.GlyphHeight;


            for (int p = 0; p < Settings.GlyphLineCount; p++)
            {
                for (int n = 0; n < Settings.GlyphsPerLine; n++)
                {
                    var bitmap = new Bitmap(Settings.GlyphWidth, Settings.GlyphHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                    Font font;
                    if (!String.IsNullOrWhiteSpace(Settings.FromFile))
                    {
                        var collection = new PrivateFontCollection();
                        collection.AddFontFile(Settings.FromFile);                        
                        var fontFamily = new FontFamily(Path.GetFileNameWithoutExtension(Settings.FromFile), collection);
                        
                        font = new Font(fontFamily, Settings.FontSize);
                    }
                    else
                    {
                        font = new Font(new FontFamily(Settings.FontName), Settings.FontSize);
                    }
                    
                    var g = Graphics.FromImage(bitmap);

                    if (Settings.BitmapFont)
                    {
                        g.SmoothingMode = SmoothingMode.None;
                        g.TextRenderingHint = TextRenderingHint.SingleBitPerPixel;
                    }
                    else
                    {
                        g.SmoothingMode = SmoothingMode.HighQuality;
                        g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                        //g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                    }

                    char c = (char)(n + p * Settings.GlyphsPerLine);
                    //g.DrawString(c.ToString(), font, Brushes.Black,
                    //    n * Settings.GlyphWidth + Settings.AtlasOffsetX, p * Settings.GlyphHeight + Settings.AtlassOffsetY);
                    g.DrawString(c.ToString(), font, Brushes.Red, Settings.AtlasOffsetX, Settings.AtlassOffsetY);

                    characters.Add(c, bitmap);
                }
            }
        }

        public static Bitmap GetVertexDataFromChar(char c)
        {
            return characters[c];
        }
    }
}