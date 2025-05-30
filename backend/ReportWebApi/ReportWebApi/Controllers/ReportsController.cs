using Microsoft.AspNetCore.Mvc;
using ReportWebApi.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace ReportWebApi.Controllers;

/// <summary>
/// Контроллер для работы с отчетами
/// </summary>
[ApiController]
[Route("[controller]")]
public class ReportsController : ControllerBase
{
    [HttpGet]
    [Produces("application/json")]
    [SwaggerResponse(StatusCodes.Status200OK, Description = "Запрос успешно прошел")]
    public Task<ReportModel[]> GetReports()
    {
        var reports = new[]
        {
            CreateReport(Guid.NewGuid(), "Report 1", "Description 1", "Value1"),
            CreateReport(Guid.NewGuid(), "Report 2", "Description 2", "Value2"),
            CreateReport(Guid.NewGuid(), "Report 3", "Description 3", "Value3"),
            CreateReport(Guid.NewGuid(), "Report 4", "Description 4", "Value4")
        };
        return Task.FromResult(reports);
    }

    private static ReportModel CreateReport(Guid id, string name, string description, string value)
    {
        return new ReportModel
        {
            Id = id,
            Name = name,
            Description = description,
            Value = value,
        };
    }
}