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
        private static Dictionary<char, Texture2D> characters = new Dictionary<char, Texture2D>();

        private static class Settings
        {
            public static string FontBitmapFilename = "test.png";
            public static int GlyphsPerLine = 16;
            public static int GlyphLineCount = 16;
            public static int GlyphWidth { get { return (int)(FontSize * 1.35f); } }
            public static int GlyphHeight { get { return GlyphWidth * 2; } }

            public static int CharXSpacing = 22;

            public static string Text = "GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);";

            // Used to offset rendering glyphs to bitmap
            public static float AtlasOffsetX = -5.5f, AtlassOffsetY = -2;
            public static int FontSize = 14;
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
                    if (!string.IsNullOrWhiteSpace(Settings.FromFile))
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
                    }

                    char c = (char)(n + p * Settings.GlyphsPerLine);

                    g.DrawString(c.ToString(), font, Brushes.White, Settings.AtlasOffsetX, Settings.AtlassOffsetY);
                    bitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);

                    var texture = new Texture2D(Settings.GlyphWidth, Settings.GlyphHeight);
                    for(int x = 0; x < texture.width; x++)
                        for(int y = 0; y < texture.height; y++)
                        {
                            texture.SetPixel(x, y, bitmap.GetPixel(x, y));
                        }

                    texture.Apply();
                    
                    characters.Add(c, texture);
                }
            }
        }

        public static Texture2D GetTexture2DFromChar(char c)
        {
            return characters[c];
        }
    }
}