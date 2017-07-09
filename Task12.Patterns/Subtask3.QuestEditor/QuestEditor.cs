using Subtask3.QuestEditor.Entities;
using Subtask3.QuestEditor.Interfaces;

namespace Subtask3.QuestEditor
{
    public class QuestEditor
    {
        private IState _state;

        public void SetState(IState state)
        {
            _state = state;
        }
        
        public void UpdateState(Block block = null)
        {
            _state = _state.Handle(block);
        }
    }
}
