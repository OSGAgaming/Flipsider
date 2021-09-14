namespace FlipEngine
{
    public partial class TileManager
    {
        //Put Tile Types in here and assign them a value
        public void LoadTileTypes()
        {
            AddTileType(0, TextureCache.TileSet1);
            AddTileType(1, TextureCache.TileSet2);
            AddTileType(2, TextureCache.TileSet3);
            AddTileType(3, TextureCache.TileSet4);
        }

    }
}
