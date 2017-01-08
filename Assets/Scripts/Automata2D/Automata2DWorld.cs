using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Automata2D
{
    public struct Tile
    {
        public byte m_Type; // Type - defined by application
        public byte m_Attrib1;  // First Attribute - use is automata specific
        public byte m_Attrib2;  // Second Attribute - use is automata specific
        public byte m_Attrib3;  // Third Attribute - use is automata specific
    };

 

    public class World
    {
        private Tile[,] m_Tiles;    // tiles that make up the world

        private List<AutomataJob> m_ActiveJobs; // jobs which are currently active in the world

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
        int m_MinX, m_MinY;
        int m_MaxX, m_MaxY;
    }

    // Automata job - this is a process which runs on the world
    public class AutomataJob
    {
        private World   m_World;    // The world we belong to
        private BBox2D  m_Bounds;    // region of world that job runs over

        public void SetWorld(World world) { m_World = world; }

        public virtual void Process() { }
    };
}
