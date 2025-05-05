using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.UI.Composition;
using Windows.ApplicationModel.UserDataTasks;



public enum TaskStatus
{
    ToDo,
    Doing,
    Done
}



namespace Gantt
{
    public class Task
    {
        #region Variables and stuff
        public string Name { get; set; }
        public int ID { get; set; }
        public List<int?> DependsOn = new List<int?>();
        public TaskStatus Status { get; set; }

        #endregion

        public Task(string name, int id)
        {
            Name = name;
            ID = id;
            Status = TaskStatus.ToDo;
        }


    }

    public class GanttChart //The actual program is going here I think!
    {
        public int nextTaskID;
        public int alice;
        public List<Task> TaskList = new List<Task>();


        public void AddTask(string TaskName)
        {

            return;
        }
        public void TaskListRead()
        {
            //Opens the csv that contains all the tasks
            string path = "C:\\Program Files\\Liz\\MyGantt\\TaskList.txt";
            if (File.Exists(path))
            {
                using (StreamReader sr = File.OpenText(path))
                {
                    string? s;
                    s = sr.ReadLine();

                    //read the first line, which should be the highest taskID
                    if (s == null) //If it was an empty file, off we go!
                    {
                        nextTaskID = 0;
                        return;
                    }
                    else
                    {
                        string[] comma;
                        comma = s.Split(";");
                        Int32.TryParse(comma[1], out nextTaskID);
                    }
                    int lengthoftasklist = -1;
                    string newTaskName = " ";
                    int thisTaskID = 0;
                    //If we have a taskID, lets go and try to find all the tasks
                    while ((s = sr.ReadLine()) != null)
                    {
                        string[] fileLine;
                        fileLine = s.Split(";");

                        switch (fileLine[0])
                        {
                            case "TaskName":
                                newTaskName = fileLine[1];
                                thisTaskID = 0;
                                lengthoftasklist++;
                                break;

                            case "TaskID":
                                thisTaskID = Int32.Parse(fileLine[1]);
                                Task temptask = new Task(newTaskName, thisTaskID);
                                TaskList.Add(temptask); //stick that new task on the end
                                break;

                            case "TaskStatus":
                                TaskList[lengthoftasklist].Status = (TaskStatus)Int32.Parse(fileLine[1]);
                                break;

                            case "DependsOn":
                                if (fileLine.Length > 1)
                                {
                                    for (int i = 1; i < fileLine.Length; i++)
                                    {
                                        int frank;
                                        frank = Int32.Parse(fileLine[1]);
                                        TaskList[lengthoftasklist].DependsOn.Add(Int32.Parse(fileLine[i]));
                                    }
                                }
                                break;

                            default:
                                break;
                        }
                    }
                }
            }
        }
        public void TaskListWrite()
        {
            //Opens the csv that contains all the tasks
            string path = "C:\\Program Files\\Liz\\MyGantt\\TaskList.txt";
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    string fileLine;
                    //first, write the number of tasks total
                    fileLine = "TaskID;" + nextTaskID.ToString() + "\n";
                    sw.Write(fileLine);

                    //Then, write each task in the format
                    foreach (Task T in TaskList)
                    {
                        fileLine = "TaskName;" + T.Name + "\n";
                        fileLine += "TaskID;" + T.ID.ToString() + "\n";
                        fileLine += "TaskStatus;" + T.Status.ToString() + "\n";
                        fileLine += "DependsOn";
                        foreach (int? D in T.DependsOn)
                        {
                            fileLine += ";" + D.ToString();
                        }
                        sw.Write(fileLine+"\n");
                    } 
                }
            }
        }
    }

}
