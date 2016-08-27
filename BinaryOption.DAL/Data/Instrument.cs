﻿using System;

namespace BinaryOptions.DAL.Data
{
    public class Instrument
    {
        public Instrument(string name, double min, double max)
        {
            Id = Guid.NewGuid();
            Name = name;
            Min = min;
            Max = max;
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public double Min { get; private set; }
        public double Max { get; private set; }
        public double Rate { get; private set; }

        public void Update(double rate) 
        {
            Rate = rate;
        }
    }
}