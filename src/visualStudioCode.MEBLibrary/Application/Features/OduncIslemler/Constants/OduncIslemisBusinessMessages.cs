namespace Application.Features.OduncIslemler.Constants;

public static class OduncIslemisBusinessMessages
{
    public const string SectionName = "OduncIslemi";

    public const string OduncIslemiNotExists = "OduncIslemiNotExists";
    public const string NushaAlreadyBorrowed = "NushaAlreadyBorrowed";
    public const string UserHasExceededLoanLimit = "UserHasExceededLoanLimit";
    public const string UserHasOverdueLoans = "UserHasOverdueLoans";
    public const string LoanExtensionLimitExceeded = "LoanExtensionLimitExceeded";
    public const string LoanNotActive = "LoanNotActive";
    public const string NushaNotAvailableForLoan = "NushaNotAvailableForLoan";
    public const string ReservationNotFound = "ReservationNotFound";
    public const string ReservationAlreadyFulfilled = "ReservationAlreadyFulfilled";
    public const string InvalidLoanPeriod = "InvalidLoanPeriod";
    public const string LateFeeCalculationError = "LateFeeCalculationError";
}
