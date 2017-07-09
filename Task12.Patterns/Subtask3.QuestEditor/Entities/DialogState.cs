using System;

namespace Subtask3.QuestEditor.Entities
{
    public class DialogState : State
    {
        protected override void HandleInner(Block block)
        {
            Console.WriteLine("Dialog window was called");
        }
    }
}
