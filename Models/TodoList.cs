namespace TodoApi.Models;

public class TodoList
{
    public long Id { get; set; }
    
    public string? Owner { get; set; }
    public string? Name { get; set; }
    public bool Completed { get; set; }
    public string? Secret { get; set; }
}