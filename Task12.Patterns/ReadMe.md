## Task 12 - Patterns

### Subtask 1

Describe disadvantages for the following example of the Singleton pattern.  
Create solution to remove these disadvantages.

```c#
public class Singleton
{
     private static Singleton instance; 
 
     private Singleton()
     {}
 
     public static Singleton GetInstance()
     {
         if (instance == null)
             instance = new Singleton();
         return instance;
     }
} 
```

### Subtask 2

There are an object in the application. A large number of resources is needed to create this object which becomes noticeable when creating them more than 5 ones. A customer asked to optimize work with large amount of instances of this type and optimize code. To refactor and optimize use one of the design patterns.  

Use the most appropriate pattern and write a simple example of implementation this pattern.

### Subtask 3

The task is to implement a part of an editor of quests for RPG game. The editor is a set of prepared blocks with the main conditions and results which a user can combine with each other.  
Implement a business layer which contains "blocks" of a quest and their logic. Use appropriate patterns to implement the task.  
Implement a simple example (without UI) for one of the blocks.  
  
Below is an example of a quest in the editor:  
**a character meets a person -> call a dialogue -> print text -> ... -> when selecting the first point add a quest to the quests table to perform**  

Implementation should be extensible easily to add new elements to the editor quickly.
