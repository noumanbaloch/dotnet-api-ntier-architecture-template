using Breeze.Models.ApplicationEnums;
using Breeze.Models.Constants;

namespace Breeze.Models.GenericResponses;
public static class GenericResponseHelper
{
    public static GenericResponse GenerateEnumToGenericResponse(ResponseEnums result)
    {
        return result switch
        {
            ResponseEnums.Success => GenericResponse.Success(ApiResponseMessages.USER_LOGIN_SUCCESSFULLY, ApiStatusCodes.USER_LOGIN_SUCCESSFULLY)

            _ => GenericResponse.Failure(ApiResponseMessages.SOMETHING_WENT_WRONG, ApiStatusCodes.SOMETHING_WENT_WRONG),
        }; ;
    }
}