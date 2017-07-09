using System;

namespace Subtask3.QuestEditor.Entities
{
    public class DialogOptionState : State
    {
        private readonly string _optionText;

        public DialogOptionState(string optionText)
        {
            _optionText = optionText;
        }

        protected override void HandleInner(Block block)
        {
            Console.WriteLine($"Chosen option: {_optionText}");
        }
    }
}
