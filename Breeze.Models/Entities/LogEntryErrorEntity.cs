using Breeze.Models.Constants;
using System.ComponentModel.DataAnnotations.Schema;

namespace Breeze.Models.Entities;

public class LogEntryErrorEntity : BaseEntity
{
    public int Id { get; set; }

    [Column(DbColumnNames.EXCEPTION)]
    public required string Exception { get; set; }

    [Column(DbColumnNames.MESSAGE)]
    public required string Message { get; set; }

    [Column(DbColumnNames.STATUS_CODE)]
    public int StatusCode { get; set; }

    [Column(DbColumnNames.STACK_TRACE)]
    public required string StackTrace { get; set; }

    [Column(DbColumnNames.USER_DESCRIPTION)]
    public string? UserDescription { get; set; }

    [Column(DbColumnNames.REQUEST_METHOD)]
    public required string RequestMethod { get; set; }

    [Column(DbColumnNames.REQUEST_PATH)]
    public required string RequestPath { get; set; }

    [Column(DbColumnNames.REQUEST_HEADERS)]
    public required string RequestHeaders { get; set; }

    [Column(DbColumnNames.SOURCE)]
    public required string Source { get; set; }
}
