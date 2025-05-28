using SQLite;

public class TodoTask
{

    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Text { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime DueDate { get; set; }
}