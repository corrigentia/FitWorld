using System.ComponentModel.DataAnnotations;

namespace FitWorldApi.Models.Forms
{
    public class StudentUpdateForm
    {
        [Required]
        /*
         * curl -X 'POST' \
  'https://localhost:7076/api/Auth/Register' \
  // -H 'accept: *\/*' \
  -H 'Content-Type: multipart/form-data' \
  -F 'Email=a@bcd' \
  -F 'Password=1'

Request URL

https://localhost:7076/api/Auth/Register
         * 
         * {
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "traceId": "00-0d2e1af7d80c68d454407e686cbdad3e-b112c71589decb81-00",
  "errors": {
    "Email": [
      "The Email field is not a valid e-mail address."
    ]
  }
}

Response headers

 content-type: application/problem+json; charset=utf-8
        date: Wed,11 Jan 2023 07:07:53 GMT
        server: Kestrel
        x-firefox-spdy: h2 
         */
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        [StringLength(maximumLength: 320, MinimumLength = 5)]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,}$")]
        public string? Email { get; set; }
        [Required]
        [StringLength(maximumLength: 14, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}
