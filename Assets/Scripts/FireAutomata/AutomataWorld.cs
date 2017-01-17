using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FireAutomata
{
    // This the automata world
    public class World
    {
        private int m_SizeX, m_SizeY;
        private Tile[,] m_Tiles;    // tiles that make up the world
        private ITileMap m_TileMap;

        private List<AutomataJob> m_ActiveJobs; // jobs which are currently active in the world

        public Tile[,] Tiles { get { return m_Tiles; } }

        public void Init(int xSize, int ySize, ITileMap tileMap)
        {
            m_Tiles = new Tile[xSize, ySize];
            m_SizeX = xSize;
            m_SizeY = ySize;
            m_TileMap = tileMap;
        }

        public bool TileInWorld(int xp, int yp)
        {
            if (xp < 0 || yp < 0 || xp > m_SizeX - 1 || yp > m_SizeY - 1)
                return false;
            else
                return true;
        }

        // inform tilemap that tile has changed
        public void UpdateTilemapForTile(int xp, int yp, ETileType newType)
        {
            if (m_TileMap != null)
                m_TileMap.SetTileType(xp, yp, newType);
        }

        // tile burning status has changed
        public void SetTileBurning(int xp, int yp, bool bBurning)
        {
            if (m_TileMap != null)
                m_TileMap.SetTileBurning(xp, yp, bBurning);
        }

        // Automata Jobs

        public void AddAutomataJob(AutomataJob job)
        {
            // TODO: check if job is already in world?

            job.SetWorld(this);
            m_ActiveJobs.Add(job);
        }

        public void ProcessAutomataJobs()
        {
            for (int jobNo = 0; jobNo < m_ActiveJobs.Count; jobNo++)
            {
                m_ActiveJobs[jobNo].Process();
            }
        }
    };
}
