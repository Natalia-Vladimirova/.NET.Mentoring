using System;

namespace Subtask3.QuestEditor.Entities
{
    public class StartState : State
    {
        protected override void HandleInner(Block block)
        {
            Console.WriteLine("The game started");
        }
    }
}
