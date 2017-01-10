using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Automata2D
{
    public enum ETileType
    {
        Air,
        Wood,
        Stone,
        Water
    }

    public struct Tile
    {
        public ETileType    m_Type;             // type - see enum above
        public bool         m_bBurning;         // is the tile on fire?
        public byte         m_Temperature;      // how hot this tile has got, threshold will determine when it catches alight
        public byte         m_Health;           // how close tile it to breaking
    };
    
    public class World
    {
        private int m_SizeX, m_SizeY;
        private Tile[,] m_Tiles;    // tiles that make up the world

        private List<AutomataJob> m_ActiveJobs; // jobs which are currently active in the world

        public Tile[,] Tiles { get { return m_Tiles; } }

        public void Init(int xSize,int ySize)
        {
            m_Tiles = new Tile[xSize, ySize];
            m_SizeX = xSize;
            m_SizeY = ySize;
        }

        public bool TileInWorld(int xp,int yp)
        {
            if (xp < 0 || yp < 0 || xp > m_SizeX - 1 || yp > m_SizeY - 1)
                return false;
            else
                return true;
        }

        public void AddAutomataJob(AutomataJob job)
        {
            // TODO: check if job is already in world?

            job.SetWorld(this);
            m_ActiveJobs.Add(job);
        }

        public void ProcessAutomataJobs()
        {
            for(int jobNo=0;jobNo<m_ActiveJobs.Count;jobNo++)
            {
                m_ActiveJobs[jobNo].Process();
            }
        }
    };

    // 2D Bounding box type - TODO: move somewhere better
    public struct BBox2D
    {
        public int m_MinX, m_MinY;
        public int m_MaxX, m_MaxY;
    }

    // Automata job - this is a process which runs on the world
    public class AutomataJob
    {
        protected World   m_World;    // The world we belong to
        protected BBox2D  m_Bounds;    // region of world that job runs over

        public void SetWorld(World world) { m_World = world; }

        public virtual void Process() { }
    };

    // automata job for fire
    public class FireJob : AutomataJob
    {
        private bool UpdateAdjacentTile(int xp,int yp)
        {
            if (m_World.TileInWorld(xp, yp) == false)
                return false;

            Tile adjacentTile = m_World.Tiles[xp, yp];
            if (adjacentTile.m_bBurning == true)    // early out if tile is already burning
                return false;

            adjacentTile.m_Temperature++;
            if (adjacentTile.m_Type == ETileType.Wood && adjacentTile.m_Temperature > 20)
            {
                adjacentTile.m_bBurning = true;
                return true;
            }

            return false;
        }

        public override void Process()
        {
            // Iterate over bounds
            BBox2D newBounds = m_Bounds;

            for (int yp = m_Bounds.m_MinY; yp<= m_Bounds.m_MaxY;yp++)
            {
                for(int xp = m_Bounds.m_MinX; xp <= m_Bounds.m_MaxX;xp++)
                {
                    Tile tile = m_World.Tiles[xp, yp];

                    if(tile.m_bBurning) // flaming tile will spread fire
                    {
                        // Try to spread in 4 directions
                        if (UpdateAdjacentTile(xp - 1, yp))
                            newBounds.m_MinX = Mathf.Min(newBounds.m_MinX,xp-1);
                        if (UpdateAdjacentTile(xp + 1, yp))
                            newBounds.m_MaxX = Mathf.Max(newBounds.m_MaxX, xp + 1);
                        if (UpdateAdjacentTile(xp, yp - 1))
                            newBounds.m_MinY = Mathf.Min(newBounds.m_MinY, yp - 1);
                        if(UpdateAdjacentTile(xp, yp + 1))
                            newBounds.m_MaxY = Mathf.Max(newBounds.m_MaxY, yp + 1);
                    }
                }

            }

            // write back new bounding box
            m_Bounds = newBounds;
        }
    }
}
