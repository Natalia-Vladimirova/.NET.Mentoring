using System;

namespace Subtask3.QuestEditor.Entities
{
    public class MeetingState : State
    {
        protected override void HandleInner(Block block)
        {
            Console.WriteLine("The hero meets a character");
        }
    }
}
