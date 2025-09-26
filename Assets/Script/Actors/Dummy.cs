using Nananami.Commands;
using Nananami.Helpers;
using UnityEngine;

namespace Nananami.Actors
{
    public class Dummy : OffScreenAutoDeletable
    {
        void Awake()
        {
            CollisionInitialize(0.0f, "Dummy");

            scheduler.EnqueueCommand(new SetVariable<float>("offScreenDeleteRate", 30.0f));
            scheduler.Execute();
        }

        protected override void m_updateAfterCommandExecution()
        {
            m_damage = CommandVariableHelper.GetVariable<int>(scheduler, "damage");
            base.m_updateAfterCommandExecution();
        }
    }
}