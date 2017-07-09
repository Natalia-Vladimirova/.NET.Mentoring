using Subtask3.QuestEditor.Entities;

namespace Subtask3.QuestEditor.Interfaces
{
    public interface IState
    {
        void AddState(string key, IState block);

        IState Handle(Block block);
    }
}
