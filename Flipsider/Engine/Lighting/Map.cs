using Flipsider.Engine.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics;
using static Flipsider.PropManager;

namespace Flipsider
{
    public class Map
    {
        internal Dictionary<string, MapPass> MapPasses = new Dictionary<string, MapPass>();
        public void OrderedRenderPass(SpriteBatch sb, GraphicsDevice GD)
        {
            int i = 0; 
            foreach(KeyValuePair<string, MapPass> Map in MapPasses)
            {
                var Pass = Map.Value;

                if(Pass.Index == i) Pass.Render(sb, GD);

                i++;
            }

            Main.graphics.GraphicsDevice.SetRenderTarget(null);
        }

        public void OrderedShaderPass()
        {
            int i = 0;
            foreach (KeyValuePair<string, MapPass> Map in MapPasses)
            {
                var Pass = Map.Value;

                if (Pass.Index == i) Pass.ApplyShader();

                i++;
            }

            Main.graphics.GraphicsDevice.SetRenderTarget(null);
        }
        public void DrawToMap(string Map, MapRender MR) => MapPasses[Map].DrawToTarget(MR);

        public void AddMap(string MapName,int Index, MapPass MP) 
        {
            MP.Parent = this;
            MP.Index = Index;
            MapPasses.Add(MapName, MP); 
        }

        public MapPass Get(string MapName) => MapPasses[MapName];
    }
}