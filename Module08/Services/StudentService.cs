using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Module08.Model;

namespace Module08.Services
{
	public class StudentService
	{
		private readonly HttpClient _httpClient;
		private const string BaseUrl = "http://localhost/pdc50/";

		public StudentService()
		{
			_httpClient = new HttpClient();
		}

		public async Task<List<Student>> GetStudentsAsync()
		{
			try
			{
				return await _httpClient.GetFromJsonAsync<List<Student>>($"{BaseUrl}get_students.php") ?? new List<Student>();
			}
			catch
			{
				return new List<Student>();
			}
		}
	}
}