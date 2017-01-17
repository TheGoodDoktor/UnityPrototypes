using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FireAutomata
{
    // Tile type enum, types have different properties & behaviours
    public enum ETileType
    {
        Air,
        Wood,
        Stone,
        Water
    }

    // tile type - this is the working data for the automata
    public struct Tile
    {
        public ETileType    m_Type;             // type - see enum above
        public bool         m_bBurning;         // is the tile on fire?
        public byte         m_Temperature;      // how hot this tile has got, threshold will determine when it catches alight
        public byte         m_Health;           // how close tile it to breaking
    };

    // interface to interact with a tilemap
    public interface ITileMap
    {
        void SetTileType(int xp, int yp, ETileType newType);
        ETileType GetTileType(int xp, int yp);
        void SetTileBurning(int xp, int yp, bool bBurning);
    }

    // 2D Bounding box type - TODO: move somewhere better or use existing type
    public struct BBox2D
    {
        public int m_MinX, m_MinY;
        public int m_MaxX, m_MaxY;
    }

   
}
