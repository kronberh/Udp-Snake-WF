﻿using System.Text.Json;
using ColorConverter = ns_Converters.ColorConverter;

namespace ns_Data
{
    internal record PackageData
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Color Color { get; set; }
        public static JsonSerializerOptions SerializerOptions { get; } = new() { Converters = { new ColorConverter() } };
        public PackageData(int X, int Y, Color Color)
        {
            this.X = X;
            this.Y = Y;
            this.Color = Color;
        }
    }
}