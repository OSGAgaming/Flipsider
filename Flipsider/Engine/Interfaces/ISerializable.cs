﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Flipsider.Engine.Interfaces
{
    public interface ISerializable
    {
        public void Serialize(string path);
    }
}