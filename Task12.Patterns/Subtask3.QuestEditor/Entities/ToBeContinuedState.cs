using System;
using Subtask3.QuestEditor.Interfaces;

namespace Subtask3.QuestEditor.Entities
{
    public class ToBeContinuedState : IState
    {
        public void AddState(string key, IState block)
        {
        }

        public IState Handle(Block block)
        {
            Console.WriteLine("To be continued...");
            return null;
        }
    }
}
