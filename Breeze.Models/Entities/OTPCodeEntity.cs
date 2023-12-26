using Breeze.Models.Constants;
using System.ComponentModel.DataAnnotations.Schema;

namespace Breeze.Models.Entities;

public class OTPCodeEntity : BaseEntity
{
    public int Id { get; set; }

    [Column(DbColumnNames.USER_NAME)]
    public required string UserName { get; set; }

    [Column(DbColumnNames.OTP_CODE)]
    public required string OTPCode { get; set; }

    [Column(DbColumnNames.EXPIRATION_TIME)]
    public DateTime ExpirationTime { get; set; }

    [Column(DbColumnNames.OTP_USE_CASE)]
    public required string OTPUseCase { get; set; }
}
