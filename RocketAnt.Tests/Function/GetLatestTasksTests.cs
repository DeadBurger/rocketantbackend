using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using RocketAnt;
using RocketAnt.Contract;
using RocketAnt.Function;
using RocketAnt.Repository;
using Xunit;

public class GetLatestTasksTests : IClassFixture<GetLatestTasksTests>
{
    public GetLatestTasksTests()
    {
        InitTasks();
    }

    private readonly ILogger logger = TestFactory.CreateLogger();
    private List<BackgroundTask> tasks;

    [Fact]
    public async void GetLatestTasks_ShouldReturnTasks()
    {
        var request = TestFactory.CreateHttpRequest("name", "Bill");

        Mock<ITaskRepository> taskRepositoryMock = new Mock<ITaskRepository>();
        taskRepositoryMock.Setup(o => o.GetLatest(It.IsAny<int>()))
        .ReturnsAsync(tasks);

        GetLatestTasks target = new GetLatestTasks(taskRepositoryMock.Object);

        var response = (OkObjectResult)await target.Run(request, logger);
        var resultContract = (List<TaskContract>)response.Value;
        Assert.Equal(3, resultContract.Count);
        Assert.Equal(1, resultContract.Count(o => o.Id == "task1"));
        Assert.Equal(1, resultContract.Count(o => o.Id == "task2"));
        Assert.Equal(1, resultContract.Count(o => o.Id == "task3"));
    }
    private void InitTasks()
    {
        this.tasks = new List<BackgroundTask>();
        tasks.Add(new BackgroundTask()
        {
            Id = "task1",
        });
        tasks.Add(new BackgroundTask()
        {
            Id = "task2",
        });
        tasks.Add(new BackgroundTask()
        {
            Id = "task3",
        });
    }
}