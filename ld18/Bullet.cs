/* All Rights Reserved. Copyright 2010 Philip Ludington */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace LD18
{
    public class Bullet
    {
        public Vector2 Position;

        public void Update()
        {
            Position.Y += 1.1f;
        }
    }
}
