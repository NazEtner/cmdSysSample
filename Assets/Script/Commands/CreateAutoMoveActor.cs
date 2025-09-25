using System.Collections.Generic;
using Nananami.Actors;
using Nananami.Lib.CmdSys;

namespace Nananami.Commands
{
    // 指定したパスのプレハブにAutoMoveActorを継承したスクリプトがアタッチされてる必要がある
    public class CreateAutoMoveActor : Command
    {
        public CreateAutoMoveActor(string prefabPath, AutoMoveActorInitializationParameter initParam, List<string> patterns = null)
        {
            patterns ??= new List<string>();

            m_prefab_path = prefabPath;
            m_patterns = patterns;
            m_initialization_parameter = initParam;
        }
        public override CommandResult Execute(ref ScheduleStatus status)
        {
            var instance = GameMain.Instance;

            if (instance != null)
            {
                var actor = instance.prefabInstantiator.InstantiatePrefab<AutoMoveActor>(m_prefab_path);
                actor.AutoMoveInitialize(m_initialization_parameter);

                foreach (var pattern in m_patterns)
                {
                    actor.scheduler.EnqueueCommand(new EnqueueCommandsFromPattern(pattern));
                }
            }
            else
            {
                UnityEngine.Debug.LogError("Failed to get game instance.");
            }

            return new CommandResult
            {
                endPeriod = false,
                expired = true,
                recordable = true,
            };
        }

        public override void Reset()
        {
            // nothing to do.
        }

        private string m_prefab_path;
        private List<string> m_patterns;
        private AutoMoveActorInitializationParameter m_initialization_parameter;
    }
}