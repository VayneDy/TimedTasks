# TimedTasks

#### introduce
WindowsService timed task template,
Executing scheduled tasks is a very common requirement. This project is to create a Windows service and run scheduled tasks in the service. It has been tested by multiple projects and runs stably.
The timer basic project is decoupled from business logic, and business logic task classes are created through reflection. This basic project can be directly used in other projects.
It supports daily timing execution, monthly timing execution, timing loop execution, and loop execution within a time period, custom loop interval, time unit hours, minutes and seconds. The configuration is sufficient to support most application scenarios.


#### Instructions for use

1. WindowsService1 is a blank service program used to create Windows services. You only need to modify the three attributes of service name, display name and description. If you do not know how to change, you can refer to this: https://www.cnblogs.com/v-dai/p/15829479.html

There is a service installer.exe under WindowsService1\bin\Debug, you can use this installation service, select WindowsService1.exe for the installation path, fill in your custom service name for the service name, and click Install.

2. MyTimedTasks is the base class library of timers, the core code is here, read the configuration file and create task instances through reflection to execute regularly.
   The configuration file is written in detail in WindowsService1\bin\Debug\XmlConfig\MyTasks.json. You can refer to the annotation configuration task.

3. MyTask_LB is an example of a task. You can directly write business logic tasks in this class library, or you can create a new class library.
   Each task class should inherit TaskAct_Base , then override the Do() method of the parent class, and write your business logic in the Do() method.

4. TaskTest is a console program for debugging business logic.
