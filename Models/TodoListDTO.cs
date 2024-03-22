namespace TodoApi.Models;

public class TodoListDTO
{
    public long Id { get; set; }
    public string? Owner { get; set; }
    public string? Name { get; set; }
    public bool Completed { get; set; }
}