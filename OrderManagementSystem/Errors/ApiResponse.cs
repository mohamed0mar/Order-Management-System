namespace OrderManagementSystem.Errors
{
	public class ApiResponse
	{
		public int StatusCode { get; set; }
        public string? Message { get; set; }
        public ApiResponse(int statusCode,string? message=null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefauteMessageForStatusCode(statusCode);
        }

		private string? GetDefauteMessageForStatusCode(int statusCode)
		{
			return statusCode switch
			{
				400 => "Bad Request You Have Made",
				401 => "Authorized,You Are Not",
				404 => "Resource Was Not Found",
				500 => "Error Are The Path To The Dark Side,Error Lead To The Anger,Anger Leads To Hate,Hate Leads To Career Change",
				_ => null
			};
		}
	}
}
