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
            public static int GlyphWidth { get { return (int)(FontSize * 1.5f); } }
            public static int GlyphHeight { get { return GlyphWidth * 2; } }

            public static int CharXSpacing = 22;

            public static string Text = "GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);";

            // Used to offset rendering glyphs to bitmap
            public static float AtlasOffsetX = -5.5f, AtlassOffsetY = -2;
            public static int FontSize = 28;
            public static bool BitmapFont = false;
            public static string FromFile; //= "joystix monospace.ttf";
            public static string FontName = "Consolas";

        }

        [Serializable]
        private class FontSettings
        {
            public int fontX_Max;
            public int fontX_Min;
            public int fontY_Max;
            public int fontY_Min;
            public int bitmapX;
            public int bitmapY;

            public FontSettings(int fontX_Max, int fontX_Min, int fontY_Max, int fontY_Min)
            {
                this.fontX_Max = fontX_Max;
                this.fontX_Min = fontX_Min;
                this.fontY_Max = fontY_Max;
                this.fontY_Min = fontY_Min;
                bitmapX = Math.Abs(fontX_Min - fontX_Max) + 2;
                bitmapY = Math.Abs(fontY_Min - fontY_Max) + 4;
            }
        }

        private const string FONT_PROPERTY = "FontProperty";
        public static void InitFont()
        {
            bool fontProperty = File.Exists("./res/" + FONT_PROPERTY);

            int bitmapWidth = Settings.GlyphWidth;
            int bitmapHeight = Settings.GlyphHeight;

            if(fontProperty)
            {
                Console.WriteLine("FontExist");
                var bytes2 = File.ReadAllBytes("./res/" + FONT_PROPERTY);
                var setting = FileManager.FromByteArray<FontSettings>(bytes2);
                bitmapWidth = setting.bitmapX;
                bitmapHeight = setting.bitmapY;
            }

            int fontX_Max = int.MinValue;
            int fontX_Min = int.MaxValue;
            int fontY_Max = int.MinValue;
            int fontY_Min = int.MaxValue;

            for (int p = 0; p < Settings.GlyphLineCount; p++)
            {
                for (int n = 0; n < Settings.GlyphsPerLine; n++)
                {
                    var bitmap = new Bitmap(bitmapWidth, bitmapHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

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

                    g.DrawString(c.ToString(), font, Brushes.White, 0, 0);
                    bitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);

                    var texture = new Texture2D(bitmapWidth, bitmapHeight);
                    for(int x = 0; x < texture.width; x++)
                        for(int y = 0; y < texture.height; y++)
                        {
                            Color fontColor = bitmap.GetPixel(x, y);
                            if (fontColor.A == 0 && c == 'W')
                                fontColor = Color.FromArgb(255, 0, 0, 0);
                            if (fontColor.A == 0 && c == 'i')
                                fontColor = Color.FromArgb(255, 0, 255, 255);
                            texture.SetPixel(x, y, fontColor);
                        }

                    texture.Apply();
                    
                    if(!fontProperty)
                    {
                        for (int x = 0; x < texture.width; x++)
                            for (int y = 0; y < texture.height; y++)
                            {
                                var color = texture.GetPixel(x, y);
                                if (color.A > 0)
                                {
                                    if (x > fontX_Max)
                                        fontX_Max = x;
                                    if (x < fontX_Min)
                                        fontX_Min = x;
                                    if (y > fontY_Max)
                                        fontY_Max = y;
                                    if (y < fontY_Min)
                                        fontY_Min = y;
                                }
                            }

                        characters.Add(c, texture);
                    }
                    else
                    {
                        characters.Add(c, texture);
                    }                    
                }
            }

            if(!fontProperty)
            {
                Console.WriteLine("Font xMax = {0} | xMin = {1} | yMax = {2} | yMin = {3}", fontX_Max, fontX_Min, fontY_Max, fontY_Min);
                var bytes = FileManager.ToByteArray(new FontSettings(fontX_Max, fontX_Min, fontY_Max, fontY_Min));
                File.WriteAllBytes("./res/" + FONT_PROPERTY, bytes);
            }
        }

        public static Texture2D GetTexture2DFromChar(char c)
        {
            return characters[c];
        }
    }
}