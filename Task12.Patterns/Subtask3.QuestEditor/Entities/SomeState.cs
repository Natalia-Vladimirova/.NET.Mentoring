using System;

namespace Subtask3.QuestEditor.Entities
{
    public class SomeState : State
    {
        protected override void HandleInner(Block block)
        {
            Console.WriteLine("Something happened here");
        }
    }
}
