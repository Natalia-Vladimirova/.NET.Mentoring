using System.Collections.Generic;
using Subtask3.QuestEditor.Interfaces;

namespace Subtask3.QuestEditor.Entities
{
    public abstract class State : IState
    {
        protected readonly IDictionary<string, IState> _states = new Dictionary<string, IState>();

        public void AddState(string key, IState block)
        {
            _states.Add(key, block);
        }

        public virtual IState Handle(Block block)
        {
            HandleInner(block);

            return block?.Command == null
                ? null
                : _states[block.Command];
        }

        protected abstract void HandleInner(Block block);
    }
}
