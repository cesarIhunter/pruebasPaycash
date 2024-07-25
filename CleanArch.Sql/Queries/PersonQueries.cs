using System.Diagnostics.CodeAnalysis;

namespace CleanArch.Sql.Queries
{
    [ExcludeFromCodeCoverage]
	public static class PersonQueries
	{
		public static string AllContact => "SELECT * FROM [Person] (NOLOCK)";

		public static string ContactById => "SELECT * FROM [Person] (NOLOCK) WHERE [ContactId] = @ContactId";

		public static string AddContact =>
            @"INSERT INTO [Person] ([FirstName], [LastName], [Email], [PhoneNumber]) 
				VALUES (@FirstName, @LastName, @Email, @PhoneNumber)";

		public static string UpdateContact =>
            @"UPDATE [Person] 
            SET [FirstName] = @FirstName, 
				[LastName] = @LastName, 
				[Email] = @Email, 
				[PhoneNumber] = @PhoneNumber
            WHERE [ContactId] = @ContactId";

		public static string DeleteContact => "DELETE FROM [Person] WHERE [ContactId] = @ContactId";
	}
}
