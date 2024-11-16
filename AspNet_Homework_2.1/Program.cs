// �������������� ������� 1
//������� ���� �� ��������� ��� �������� ��������� ������.
//����������� �������� ������ � ���������� �� � �����.
//�������� ������ ��������, ��� �������� �� �������, ����� ��������� ��� ����� ����������� � �����.
//��� �������� ������������ �����, �������� ����.

using System.Web.Helpers;
using System.Web.WebPages;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();
app.Run(async (context) =>
{
    var response = context.Response;
    var request = context.Request;
    // ���� � �����, ��� ����� ��������� �����
    var uploadPath = $"{Directory.GetCurrentDirectory()}/uploads";


    response.ContentType = "text/html; charset=utf-8";

    if (request.Path == "/upload" && request.Method == "POST")
    {
        IFormFileCollection files = request.Form.Files;
        // ���� ����� ����� ���, �� ������� ��
        if (!Directory.Exists(uploadPath)) Directory.CreateDirectory(uploadPath);

        foreach (var file in files)
        {
            // ���� � ����� uploads
            string fullPath = $"{uploadPath}/{file.FileName}";

            // ��������� ���� � ����� uploads
            using (var fileStream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
        }
        await response.WriteAsync("����� ������� ���������");
    }
    else if (request.Path == "/getFile" && request.Method == "GET")
    {
        var f = ViewFiles();
        if (f != null)
        {
            context.Response.ContentType = "text/html; charset=utf-8";
            foreach (var file in f)
            {
                await context.Response.WriteAsync($"<p>����: {file}</p>");
            }
        }
        else
        {
            await response.WriteAsync("��� ����������� ������!");
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
