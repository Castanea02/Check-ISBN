//MainWindow 기능 구현
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;

namespace ISBN_BookInfo
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void CheckISBN_Click(object sender, RoutedEventArgs e) //버튼클릭 이벤트
        {
            string isbn = isbnTextBox.Text.Replace("-", "").Replace(" ", "");//ISBN 입력 값 중 '-' 필터링, 공백 필터링

            if (IsValidISBN(isbn))//ISBN 체크 함수
            {
                // 유효한 ISBN인 경우 Google Books API를 호출하여 도서 정보 가져오기
                string bookInfo = await GetBookInfo(isbn);
                if (!string.IsNullOrEmpty(bookInfo))
                {
                    // 도서 정보를 새 폼에 출력
                    BookInfoWindow bookInfoWindow = new BookInfoWindow(bookInfo);
                    bookInfoWindow.Show();
                }
                else
                {
                    MessageBox.Show("Book not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Invalid ISBN.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool IsValidISBN(string isbn)//ISBN 검사
        {
            // ISBN은 13자리여야 함
            if (isbn.Length != 13)
            {
                return false;
            }

            // ISBN은 숫자로만 구성됨
            if (!System.Text.RegularExpressions.Regex.IsMatch(isbn, "^[0-9]+$"))
            {
                return false;
            }

            // ISBN 체크 디지트 계산
            int sum = 0;
            for (int i = 0; i < 12; i++)
            {
                int digit = int.Parse(isbn[i].ToString());
                sum += (i % 2 == 0) ? digit : digit * 3;
            }

            int checkDigit = (10 - (sum % 10)) % 10;

            // 입력된 ISBN의 마지막 자리와 계산된 체크 디지트 비교
            return checkDigit == int.Parse(isbn[12].ToString());
        }

        private async Task<string> GetBookInfo(string isbn)
        {
            using (HttpClient client = new HttpClient())
            {
                string apiUrl = $"https://www.googleapis.com/books/v1/volumes?q=isbn:{isbn}"; //API End Point

                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    JObject data = JObject.Parse(json);

                    
                    if (data["items"] != null && data["items"].HasValues)
                    {
                        // 도서 정보를 문자열로 반환
                        return data["items"][0]["volumeInfo"].ToString();
                    }
                }
            }

            return null;
        }
    }
}
