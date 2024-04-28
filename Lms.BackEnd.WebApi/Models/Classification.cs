using System.Text.Json.Serialization;

namespace Lms.BackEnd.WebApi.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Classification
{
    Freshman,
    Sophomore,
    Junior,
    Senior,
    Graduate
}