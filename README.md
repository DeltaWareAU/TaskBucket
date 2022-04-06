# TaskBucket

TaskBucket adds the ability to run background tasks in AspNet. Each task being executed is given its own Scope, this scope exists during the lifetime of the background task being executed and is disposed of when the task ends.

## Adding Task Bucket to AspNet

To enable Task Bucket simply call the below code.

```csharp
public void ConfigureServices(IServiceCollection services)
{
	services.AddTaskBucket();
	
	// -- OR --
	
	service.AddTaskBucket(o => 
	{
		// Sets the Time Zone.
		o.TimeZone;
		// Sets how many task instances can be executed at any given time.
		o.WorkerThreadCount;
		// Sets how often the Task Queue is checked for pending tasks.
		o.SetTaskQueueCheckingInterval();
		// Sets how often the Task Queue is trimmed.
		o.SetTaskQueueTrimInterval();
	});
}
```

## Adding an Impromptu Task

```csharp
public class FooService: IFooService
{
	private readonly ITaskBucket _taskBucket;

	public FooService(ITaskBucket taskBucket)
	{
		_taskBucket = taskBucket;
	}

	public void ExampleMethod()
	{
		_taskBucket.AddBackgroundTask<IBarService>
		(
			async b => await b.DoSomethingAsync()
		);
	}
}
```

## Adding a Scheduled Task

In the below example, we're adding a scheduled task which will execute the DoSomethingAsync method every minute. We've also specified that only one instance of that task may be executing at any given time. If the schedule triggers the task and a previous instance is still running it will skipped.

```csharp
public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
	app.AddScheduledTask<IBarService>
	(
		// The action to be executed.
		async b => await b.DoSomethingAsync(),
		// The Task Options
		o => 
		{
			// Only a single instance of this task may be executed at any given time.
			o.InstanceLimitation = InstanceLimit.Single;
		}
	)
	// The scheduling of the job. 
	// Run every minute.
	.RunAsCronJob("*/1 * * * *");
}
```

## Awaiting Background tasks

It is possible to await background tasks in TaskBucket.

```csharp
public async Task ExampleMethodAsync()
{
	ITask backgroundTask = _taskBucket.AddBackgroundTask<IBarService>	
	(
		async b => await b.DoSomethingAsync()
	);

	await backgroundTask.WaitAsync();
}
```
