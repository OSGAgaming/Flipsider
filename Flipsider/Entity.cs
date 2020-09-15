﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using Flipsider.Weapons;

namespace Flipsider
{
	public abstract class Entity
	{

		public int width;

		public int height;

		public Vector2 position;

		public Vector2 velocity;

		public Vector2 oldPosition;

		public Vector2 oldVelocity;

		public Vector2[] oldPositions;

		protected internal virtual void Update() { }

		protected internal virtual int TrailLength => 5;
		int a;
		public void UpdateTrailCache() 
		{
			a++;
			if(a > TrailLength - 1)
				a = 0;
			oldPositions[a] = position;
		}

		protected internal virtual void Initialize() { }

		public Entity()
        {
			oldPositions = new Vector2[TrailLength];
			Initialize();
		}

		public Vector2 Center
		{
			get
			{
				return new Vector2(position.X + width * 0.5f, position.Y + height * 0.5f);
			}
			set
			{
				position = new Vector2(value.X - width * 0.5f, value.Y - height * 0.5f);
			}
		}
	}
}
