using MahApps.Metro.Controls;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WpfTodoListApp.Models;

namespace WpfTodoListApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        HttpClient client = new HttpClient();
        TodoItemsCollection todoItems = new TodoItemsCollection();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MetroWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            // RestAPI 호출 준비
            client.BaseAddress = new System.Uri("http://localhost:6200"); // API 서버 Url
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // GetDatas 데이터 가져오기
            GetDatas();
        }

        private async Task GetDatas()
        {
            // api/TodoItems GET 메서드 호출
            GrdTodoItems.ItemsSource = todoItems;

            try
            {
                // http://localhost:6200/api/TodoItems
                HttpResponseMessage? response = await client.GetAsync("/api/TodoItems");
                response.EnsureSuccessStatusCode(); // 상태코드 확인

                var items = await response.Content.ReadAsAsync<IEnumerable<TodoItem>>();
                todoItems.CopyFrom(items); // ObservableCollection으로 형변환
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}