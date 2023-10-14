namespace Breeze.Models.ApplicationEnums;
public enum ResponseEnums
{
    //Success
    UserRegisteredSuccessfully,
    UserLoginSuccessfully,
    VerificationCodeSent,
    PasswordChangedSuccessfully,
    ProfileUpdatedSuccessfully,

    //Failure
    UserAlreadyExist,
    InvalidVerificationCode,
    InvalidUsernamePassword,
    UnableToCompleteProcess,
    InvalidPassword,
    SomethingWentWrong,
    UserDoesNotExist
}
