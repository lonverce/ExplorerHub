using System;
using System.Windows.Media;
using Microsoft.Win32;

namespace ExplorerHub.Infrastructure
{
    public class SystemColorManager : ISystemColorManager
    {
        public SystemColorManager()
        {
            SystemEvents.UserPreferenceChanged += SystemEventsOnUserPreferenceChanged;
        }

        private void SystemEventsOnUserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
        {
            if (e.Category != UserPreferenceCategory.Color && e.Category != UserPreferenceCategory.General)
            {
                return;
            }

            var color = GetAccentColor();
            SystemColorChanged?.Invoke(this, color);
        }

        public Color GetSystemColor()
        {
            
            return GetAccentColor();
        }

        public event EventHandler<Color> SystemColorChanged;

        private Color GetAccentColor()
        {
            var key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\DWM");
            var value = (int)(key?.GetValue(@"ColorizationColor") ?? 0xc4744da9);

            return Color.FromArgb(
                (byte)(value >> 24),
                (byte)(value >> 16),
                (byte)(value >> 8),
                (byte)value
            ).RemoveAlpha();
        }
    }

    public static class ColorExtensions
    {
        public static Color RemoveAlpha(this Color c)
        {
            if (c.A == byte.MaxValue)
            {
                return c;
            }

            var k = c.A * 1.0f / byte.MaxValue;
            return Color.FromRgb(Convert(k, c.R), Convert(k, c.G), Convert(k, c.B));
        }

        private static byte Convert(float k, byte v)
        {
            return (byte)((255 - v) * (1 - k) + v);
        }
    }
}
