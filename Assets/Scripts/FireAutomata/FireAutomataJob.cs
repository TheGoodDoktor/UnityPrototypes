using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FireAutomata
{
    // automata job for fire
    public class FireJob : AutomataJob
    {
        private bool UpdateAdjacentTile(int xp, int yp)
        {
            if (m_World.TileInWorld(xp, yp) == false)   // reject if not in world
                return false;

            Tile adjacentTile = m_World.Tiles[xp, yp];
            if (adjacentTile.m_bBurning == true)    // early out if tile is already burning
                return false;

            adjacentTile.m_Temperature++;
            if (adjacentTile.m_Type == ETileType.Wood && adjacentTile.m_Temperature > 20)   // TODO: check against tile type info
            {
                // Tile is now burning - update tilemap
                adjacentTile.m_bBurning = true;
                m_World.SetTileBurning(xp, yp, true);
                return true;
            }

            return false;
        }

        public override void Process()
        {
            // Iterate over bounds
            BBox2D newBounds = m_Bounds;

            for (int yp = m_Bounds.m_MinY; yp <= m_Bounds.m_MaxY; yp++)
            {
                for (int xp = m_Bounds.m_MinX; xp <= m_Bounds.m_MaxX; xp++)
                {
                    Tile tile = m_World.Tiles[xp, yp];

                    if (tile.m_bBurning) // flaming tile will spread fire
                    {
                        // Try to spread in 4 directions, increase bounding box if new tiles are affected
                        if (UpdateAdjacentTile(xp - 1, yp))
                            newBounds.m_MinX = Mathf.Min(newBounds.m_MinX, xp - 1);
                        if (UpdateAdjacentTile(xp + 1, yp))
                            newBounds.m_MaxX = Mathf.Max(newBounds.m_MaxX, xp + 1);
                        if (UpdateAdjacentTile(xp, yp - 1))
                            newBounds.m_MinY = Mathf.Min(newBounds.m_MinY, yp - 1);
                        if (UpdateAdjacentTile(xp, yp + 1))
                            newBounds.m_MaxY = Mathf.Max(newBounds.m_MaxY, yp + 1);

                        // damage tile
                        tile.m_Health--;
                        if (tile.m_Health == 0)
                        {
                            // TODO: change tile type based on what type it is, currently we turn all tiles to air
                            tile.m_Type = ETileType.Air;
                            tile.m_bBurning = false;
                            m_World.SetTileBurning(xp, yp, false);
                            m_World.UpdateTilemapForTile(xp, yp, tile.m_Type);
                            // do we need to write it back because it is a struct?
                        }

                    }
                }

            }

            // write back new bounding box
            m_Bounds = newBounds;
        }
    }
}
