using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Subtask3.QuestEditor.Entities;
using Subtask3.QuestEditor.Interfaces;

namespace Subtask3.QuestEditor.App
{
    [TestClass]
    public class QuestEditorTest
    {
        [TestMethod]
        public void Test()
        {
            var editor = new QuestEditor();
            editor.SetState(InitializeStates());

            editor.UpdateState(new Block {Command = StandardCommands.MeetCharacter});
            editor.UpdateState(new Block {Command = StandardCommands.OpenDialog});
            editor.UpdateState(new Block {Command = StandardCommands.DialogOptionThree});
            editor.UpdateState(new Block {Command = StandardCommands.ToBeContinued});
            editor.UpdateState();

        }

        private IState InitializeStates()
        {
            var toBeContinued = new ToBeContinuedState();

            var dialogOptions = new List<IState>()
            {
                new DialogOptionState("Option 1 was chosen"),
                new DialogOptionState("Option 2 was chosen"),
                new DialogOptionState("Option 3 was chosen")
            };

            dialogOptions.ForEach(x => x.AddState(StandardCommands.ToBeContinued, toBeContinued));

            var dialogState = new DialogState();
            dialogState.AddState(StandardCommands.DialogOptionOne, dialogOptions[0]);
            dialogState.AddState(StandardCommands.DialogOptionTwo, dialogOptions[1]);
            dialogState.AddState(StandardCommands.DialogOptionThree, dialogOptions[2]);

            var meetingState = new MeetingState();
            meetingState.AddState(StandardCommands.OpenDialog, dialogState);
            meetingState.AddState(StandardCommands.AnotherOption, new SomeState());

            var startState = new StartState();
            startState.AddState(StandardCommands.MeetCharacter, meetingState);

            return startState;
        }
    }
}
