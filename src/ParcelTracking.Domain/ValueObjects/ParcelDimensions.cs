using System;
using System.Collections.Generic;
using System.Text;

namespace ParcelTracking.Domain.ValueObjects
{
    public class ParcelDimensions
    {
        public double Length { get; }

        public double Width { get; }

        public double Height { get; }

        public double Weight { get; }

        public ParcelDimensions(double length, double width, double height, double weight)
        {
            Length = length;
            Width = width;
            Height = height;
            Weight = weight;
        }
    }
}
