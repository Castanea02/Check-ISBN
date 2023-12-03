using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ISBN_BookInfo
{
    public partial class BookInfoWindow : Window
    {
        public BookInfoWindow(string bookInfo)
        {
            InitializeComponent();
            DisplayBookInfo(bookInfo);

            // SizeToContent 속성을 자동으로 창 크기 조절을 위해 설정
            SizeToContent = SizeToContent.WidthAndHeight;
        }

        private void DisplayBookInfo(string bookInfo)
        {
            if (bookInfo != null)
            {
                // bookInfo를 JSON으로 파싱
                JObject data = JObject.Parse(bookInfo);

                // 도서 제목 출력
                string title = data["title"]?.ToString();
                if (!string.IsNullOrEmpty(title))
                    BookTitleLabel.Content = $"제목: {title}";

                // 도서 저자 출력
                string author = data["authors"]?.FirstOrDefault()?.ToString();
                if (!string.IsNullOrEmpty(author))
                    AuthorLabel.Content = $"저자: {author}";

                // 출판사 출력
                string publisher = data["publisher"]?.ToString();
                if (!string.IsNullOrEmpty(publisher))
                    PublisherLabel.Content = $"출판사: {publisher}";

                // 썸네일 출력
                string thumbnailUrl = data["imageLinks"]?["thumbnail"]?.ToString();
                if (!string.IsNullOrEmpty(thumbnailUrl))
                {
                    // 이미지가 있는 경우
                    BookThumbnail.Source = new BitmapImage(new Uri(thumbnailUrl));
                }
                else
                {
                    // 이미지가 없는 경우, 기본 이미지 사용
                    BookThumbnail.Source = new BitmapImage(new Uri("https://i.namu.wiki/i/DIWQPMFg_xE7JxIv0-4M5PbXco2d-BynsivSWqt6enqDgXOKw0nuZznBUGV-7FtJilQEY7zxodg1kZcYlQXDJw.webp"));
                }

                // 추가적인 도서 정보를 필요에 따라 출력 가능
                // ...
            }
        }
    }
}
