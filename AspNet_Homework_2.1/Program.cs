// Дополнительное задание 1
//Создать сайт со страницей для загрузки множества файлов.
//Реализовать загрузку файлов и сохранение их в папку.
//Добавить вторую страницу, при переходе на которую, будут выводится все файлы находящееся в папке.
//Для удобства пользованием сайта, создайте меню.

using System.Web.Helpers;
using System.Web.WebPages;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();
app.Run(async (context) =>
{
    var response = context.Response;
    var request = context.Request;
    // путь к папке, где будут храниться файлы
    var uploadPath = $"{Directory.GetCurrentDirectory()}/uploads";


    response.ContentType = "text/html; charset=utf-8";

    if (request.Path == "/upload" && request.Method == "POST")
    {
        IFormFileCollection files = request.Form.Files;
        // если такой папки нет, то создаем ее
        if (!Directory.Exists(uploadPath)) Directory.CreateDirectory(uploadPath);

        foreach (var file in files)
        {
            // путь к папке uploads
            string fullPath = $"{uploadPath}/{file.FileName}";

            // сохраняем файл в папку uploads
            using (var fileStream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
        }
        await response.WriteAsync("Файлы успешно загружены");
    }
    else if (request.Path == "/getFile" && request.Method == "GET")
    {
        var f = ViewFiles();
        if (f != null)
        {
            context.Response.ContentType = "text/html; charset=utf-8";
            foreach (var file in f)
            {
                await context.Response.WriteAsync($"<p>Файл: {file}</p>");
            }
        }
        else
        {
            await response.WriteAsync("Нет загруженных файлов!");
        }
    }
    else
    {
        await response.SendFileAsync("html/index.html");
    }
});

app.Run();


static List<string> ViewFiles ()
{
    var folderPath = $"{Directory.GetCurrentDirectory()}/uploads";
    var files = Directory.GetFiles(folderPath)
                             .Select(file => Path.GetFileName(file))
                             .ToList();
    return files;
}
