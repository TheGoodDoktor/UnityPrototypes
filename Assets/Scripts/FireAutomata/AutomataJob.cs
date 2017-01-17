using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FireAutomata
{
    // Automata job - this is a process which runs on the world
    public class AutomataJob
    {
        protected World m_World;    // The world we belong to
        protected BBox2D m_Bounds;    // region of world that job runs over

        public void SetWorld(World world) { m_World = world; }

        public virtual void Process() { }
    };
}
