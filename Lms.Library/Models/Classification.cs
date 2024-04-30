using System.Text.Json.Serialization;

namespace Lms.Library.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Classification
{
    Freshman,
    Sophomore,
    Junior,
    Senior,
    Graduate
}